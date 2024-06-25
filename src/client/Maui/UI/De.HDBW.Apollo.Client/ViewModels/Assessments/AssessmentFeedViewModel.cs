// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessments;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class AssessmentFeedViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ObservableObject> _sections = new ObservableCollection<ObservableObject>();

        public AssessmentFeedViewModel(
            IAssessmentService assessmentService,
            IFavoriteRepository favoriteRepository,
            ISessionService sessionService,
            IPreferenceService preferenceService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<AssessmentFeedViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(assessmentService);
            ArgumentNullException.ThrowIfNull(favoriteRepository);
            ArgumentNullException.ThrowIfNull(sessionService);
            ArgumentNullException.ThrowIfNull(preferenceService);

            PreferenceService = preferenceService;
            AssessmentService = assessmentService;
            FavoriteRepository = favoriteRepository;
            SessionService = sessionService;
        }

        public string FavoriteIcon
        {
            get
            {
                return KnownIcons.Favorites;
            }
        }

        private IPreferenceService PreferenceService { get; }

        private IAssessmentService AssessmentService { get; }

        private IFavoriteRepository FavoriteRepository { get; }

        private ISessionService SessionService { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sections = new List<ObservableObject>();
                    var assessmentTiles = await AssessmentService.GetAssessmentTilesAsync(worker.Token).ConfigureAwait(false);
                    var favorites = await FavoriteRepository.GetItemsByTypeAsync(nameof(ModuleTile), worker.Token).ConfigureAwait(false) ?? new List<Favorite>();
                    var tiles = assessmentTiles.GroupBy(x => x.Grouping).OrderBy(x => x.Key);
                    foreach (var tile in tiles)
                    {
                        if (string.IsNullOrWhiteSpace(tile.Key))
                        {
                            sections.Add(HeadlineTextEntry.Import(Resources.Strings.Resources.AssessmentFeed_Title));
                            sections.Add(SublineTextEntry.Import(Resources.Strings.Resources.AssessmentFeed_Subline));
                        }
                        else
                        {
                            sections.Add(TextEntry.Import(tile.Key));
                        }

                        foreach (var item in tile.ToList())
                        {
                            string? route = null;
                            NavigationParameters? parameters = null;
                            bool isFavorite = item.ModuleIds.Count == 1 && favorites.Any(f => item.ModuleIds.Contains(f.Id));
                            if (item.ModuleIds.Count > 1)
                            {
                                route = Routes.ModuleOverView;
                                parameters = new NavigationParameters()
                                    {
                                        { NavigationParameter.Data, string.Join(";", item.ModuleIds) },
                                        { NavigationParameter.Type, item.Type.ToString() },
                                    };
                            }
                            else
                            {
                                route = Routes.ModuleDetailView;
                                parameters = new NavigationParameters()
                                    {
                                        { NavigationParameter.Id, item.ModuleIds.First() },
                                        { NavigationParameter.Type, item.Type.ToString() },
                                    };
                            }

                            sections.Add(AssessmentTileEntry.Import(item, route, parameters, isFavorite, Interact, CanInteract, ToggleFavorite, CanToggleFavorite));
                        }
                    }

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(sections), worker.Token);
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

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            var interactiveSections = Sections.OfType<AssessmentTileEntry>().ToList();
            foreach (var interactiveSection in interactiveSections)
            {
                interactiveSection.RefreshCommands();
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

        private bool CanInteract(AssessmentTileEntry tile)
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(tile?.Route);
        }

        private async Task Interact(AssessmentTileEntry tile, CancellationToken token)
        {
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

        private void LoadonUIThread(List<ObservableObject> sections)
        {
            Sections = new ObservableCollection<ObservableObject>(sections);
        }

        private bool CanOpenFavorite()
        {
            return !IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenFavorite))]
        private async Task OpenFavorite(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.NavigateAsync(Routes.AssessmentFavoriteView, worker.Token);
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
    }
}
