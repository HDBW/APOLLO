// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class FavoriteViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ModuleTileEntry> _favoriteItems = new ObservableCollection<ModuleTileEntry>();

        public FavoriteViewModel(
            IFavoriteRepository favoriteRepository,
            IAssessmentService assessmentService,
            IPreferenceService preferenceService,
            ISessionService sessionService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<FavoriteViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(favoriteRepository);
            ArgumentNullException.ThrowIfNull(assessmentService);
            ArgumentNullException.ThrowIfNull(preferenceService);
            ArgumentNullException.ThrowIfNull(sessionService);
            FavoriteRepository = favoriteRepository;
            AssessmentService = assessmentService;
            PreferenceService = preferenceService;
            SessionService = sessionService;
        }

        private IAssessmentService AssessmentService { get; }

        private IPreferenceService PreferenceService { get; }

        private ISessionService SessionService { get; }

        private IFavoriteRepository FavoriteRepository { get; }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var favorites = (await FavoriteRepository.GetItemsByTypeAsync(nameof(ModuleTile), worker.Token).ConfigureAwait(false)) ?? new List<Favorite>();
                    var moduleIds = favorites.Select(f => f.Id).ToList();
                    IEnumerable<ModuleTile> moduleTiles = new List<ModuleTile>();
                    if (moduleIds.Any())
                    {
                        moduleTiles = await AssessmentService.GetModuleTilesAsync(moduleIds, worker.Token).ConfigureAwait(false);
                    }

                    var items = new List<ModuleTileEntry>();
                    foreach (var moduleTile in moduleTiles)
                    {
                        var route = Routes.ModuleDetailView;
                        var parameters = new NavigationParameters()
                                    {
                                        { NavigationParameter.Id, moduleTile.ModuleId },
                                        { NavigationParameter.Type, moduleTile.Type.ToString() },
                                    };

                        items.Add(ModuleTileEntry.Import(moduleTile, route, parameters, true, SessionService.HasRegisteredUser, Interact, CanInteract, ToggleFavorite, CanToggleFavorite));
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(items), worker.Token).ConfigureAwait(false);
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
            var items = FavoriteItems.ToList();
            foreach (var item in items)
            {
                item.RefreshCommands();
            }
        }

        private void LoadonUIThread(IEnumerable<ModuleTileEntry> items)
        {
            FavoriteItems = new ObservableCollection<ModuleTileEntry>(items);
        }

        private bool CanToggleFavorite(ModuleTileEntry entry)
        {
            return !IsBusy;
        }

        private async Task ToggleFavorite(ModuleTileEntry entry, CancellationToken token)
        {
            bool result = false;
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var id = entry.ModuleId;
                    var type = nameof(ModuleTile);

                    if (entry.IsFavorite)
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
                    await ExecuteOnUIThreadAsync(
                        () =>
                        {
                            if (!result)
                            {
                                entry.IsFavorite = !entry.IsFavorite;
                            }
                            else
                            {
                                FavoriteItems.Remove(entry);
                            }
                        }, CancellationToken.None).ConfigureAwait(false);
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanInteract(ModuleTileEntry entry)
        {
            return !IsBusy;
        }

        private async Task Interact(ModuleTileEntry tile, CancellationToken token)
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
    }
}
