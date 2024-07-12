// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessments;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class JobSearchResultViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ObservableObject> _sections = new ObservableCollection<ObservableObject>();

        [ObservableProperty]
        private string? _selectedJob;

        private long? _jobId;

        public JobSearchResultViewModel(
            IAssessmentService assessmentService,
            IFavoriteRepository favoriteRepository,
            ISessionService sessionService,
            IPreferenceService preferenceService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<JobSearchResultViewModel> logger)
            : base(
                  dispatcherService,
                  navigationService,
                  dialogService,
                  logger)
        {
            ArgumentNullException.ThrowIfNull(assessmentService);
            ArgumentNullException.ThrowIfNull(favoriteRepository);
            ArgumentNullException.ThrowIfNull(sessionService);
            ArgumentNullException.ThrowIfNull(preferenceService);
            AssessmentService = assessmentService;
            FavoriteRepository = favoriteRepository;
            SessionService = sessionService;
            PreferenceService = preferenceService;
        }

        private IAssessmentService AssessmentService { get; }

        private IFavoriteRepository FavoriteRepository { get; }

        private ISessionService SessionService { get; }

        private IPreferenceService PreferenceService { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var favorites = await FavoriteRepository.GetItemsByTypeAsync(nameof(ModuleTile), worker.Token).ConfigureAwait(false) ?? new List<Favorite>();
                    var assessments = await AssessmentService.GetAssessmentTilesAsync(_jobId, worker.Token).ConfigureAwait(false);
                    var section = new List<ObservableObject>();
                    foreach (var assessment in assessments)
                    {
                        bool isFavorite = assessment.ModuleIds.Count == 1 && favorites.Any(f => assessment.ModuleIds.Contains(f.Id));
                        var route = Routes.ModuleDetailView;
                        var parameters = new NavigationParameters()
                                    {
                                        { NavigationParameter.Id, assessment.ModuleIds.First() },
                                        { NavigationParameter.Type, assessment.Type.ToString() },
                                    };
                        section.Add(AssessmentTileEntry.Import(assessment, route, parameters, isFavorite, Interact, CanInteract, ToggleFavorite, CanToggleFavorite));
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(section), worker.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            SelectedJob = navigationParameters.GetValue<string?>(NavigationParameter.Title);
            _jobId = navigationParameters.GetValue<long?>(NavigationParameter.Id);
        }

        private void LoadonUIThread(List<ObservableObject> section)
        {
            Sections = new ObservableCollection<ObservableObject>(section);
        }

        private bool CanInteract(AssessmentTileEntry tile)
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(tile?.Route);
        }

        private async Task Interact(AssessmentTileEntry tile, CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(Interact)} in {GetType().Name}.");
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    if (tile.NeedsUser && !SessionService.HasRegisteredUser)
                    {
                        var confirmedDataUsage = PreferenceService.GetValue<bool>(SharedContracts.Enums.Preference.ConfirmedDataUsage, false);
                        await NavigationService.RestartAsync(confirmedDataUsage, CancellationToken.None);
                        return;
                    }

                    await NavigationService.NavigateAsync(tile.Route!, worker.Token, tile.Parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Interact)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Interact)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(Interact)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanToggleFavorite(AssessmentTileEntry tile)
        {
            return !IsBusy && (tile?.ModuleIds?.Count() ?? 0) == 1;
        }

        private async Task ToggleFavorite(AssessmentTileEntry tile, CancellationToken token)
        {
            bool result = false;
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var id = tile.ModuleIds.First();
                    var type = nameof(ModuleTile);

                    if (tile.IsFavorite)
                    {
                        result = await FavoriteRepository.SaveAsync(new Favorite(id, type), token).ConfigureAwait(false);
                    }
                    else
                    {
                        result = await FavoriteRepository.DeleteFavoriteAsync(id, type, token).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ToggleFavorite)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ToggleFavorite)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(ToggleFavorite)} in {GetType().Name}.");
                }
                finally
                {
                    if (!result)
                    {
                        await ExecuteOnUIThreadAsync(() => { tile.IsFavorite = !tile.IsFavorite; }, CancellationToken.None).ConfigureAwait(false);
                    }

                    UnscheduleWork(worker);
                }
            }
        }
    }
}
