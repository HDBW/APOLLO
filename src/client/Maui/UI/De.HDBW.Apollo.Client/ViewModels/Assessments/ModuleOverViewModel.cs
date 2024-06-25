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
    public partial class ModuleOverViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ModuleTileEntry> _modules = new ObservableCollection<ModuleTileEntry>();

        [ObservableProperty]
        private string? _title;

        private IEnumerable<string>? _moduleIds;

        private AssessmentType _assessmentType;

        public ModuleOverViewModel(
            IAssessmentService assessmentService,
            IFavoriteRepository favoriteRepository,
            IPreferenceService preferenceService,
            ISessionService sessionService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ModuleOverViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(assessmentService);
            ArgumentNullException.ThrowIfNull(favoriteRepository);
            ArgumentNullException.ThrowIfNull(preferenceService);
            ArgumentNullException.ThrowIfNull(sessionService);

            AssessmentService = assessmentService;
            FavoriteRepository = favoriteRepository;
            PreferenceService = preferenceService;
            SessionService = sessionService;
        }

        private IAssessmentService AssessmentService { get; }

        private IFavoriteRepository FavoriteRepository { get; }

        private IPreferenceService PreferenceService { get; }

        private ISessionService SessionService { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sections = new List<ObservableObject>();
                    var moduleTiles = await AssessmentService.GetModuleTilesAsync(_moduleIds ?? new List<string>(), worker.Token).ConfigureAwait(false);
                    var favorites = await FavoriteRepository.GetItemsByTypeAsync(nameof(ModuleTile), worker.Token).ConfigureAwait(false) ?? new List<Favorite>();
                    var modules = new List<ModuleTileEntry>();
                    foreach (var moduleTile in moduleTiles)
                    {
                        var route = Routes.ModuleDetailView;
                        var parameters = new NavigationParameters()
                                    {
                                        { NavigationParameter.Id, moduleTile.ModuleId },
                                        { NavigationParameter.Type, moduleTile.Type.ToString() },
                                    };

                        modules.Add(ModuleTileEntry.Import(moduleTile, route, parameters, favorites.Any(f => f.Id == moduleTile.ModuleId), Interact, CanInteract, ToggleFavorite, CanToggleFavorite));
                    }

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(modules), worker.Token);
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
            if (navigationParameters.TryGetValue(NavigationParameter.Data, out object? moduleIds))
            {
                var idString = moduleIds.ToString() ?? string.Empty;
                _moduleIds = idString.Split(';');
            }

            if (navigationParameters.TryGetValue(NavigationParameter.Type, out object? type) && Enum.TryParse(typeof(AssessmentType), type?.ToString(), true, out object? enumValue))
            {
                _assessmentType = (AssessmentType)enumValue;
            }

            switch (_assessmentType)
            {
                case AssessmentType.Sk:
                    Title = Resources.Strings.Resources.AssessmentTypeSk;
                    break;
                case AssessmentType.Ea:
                    Title = Resources.Strings.Resources.AssessmentTypeEa;
                    break;
                case AssessmentType.So:
                    Title = Resources.Strings.Resources.AssessmentTypeSo;
                    break;
                case AssessmentType.Gl:
                    Title = Resources.Strings.Resources.AssessmentTypeGl;
                    break;
                case AssessmentType.Be:
                    Title = Resources.Strings.Resources.AssessmentTypeBe;
                    break;
            }
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            var items = Modules.ToList();
            foreach (var item in items)
            {
                item.RefreshCommands();
            }
        }

        private void LoadonUIThread(IEnumerable<ModuleTileEntry> modules)
        {
            Modules = new ObservableCollection<ModuleTileEntry>(modules);
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
                    if (!result)
                    {
                        await ExecuteOnUIThreadAsync(() => { entry.IsFavorite = !entry.IsFavorite; }, CancellationToken.None).ConfigureAwait(false);
                    }

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
