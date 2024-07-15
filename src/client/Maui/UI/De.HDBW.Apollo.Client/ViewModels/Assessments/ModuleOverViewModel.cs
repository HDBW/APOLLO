// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
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
    public partial class ModuleOverViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ModuleTileEntry> _modules = new ObservableCollection<ModuleTileEntry>();

        [ObservableProperty]
        private List<ModuleScoreEntry> _segments = new List<ModuleScoreEntry>();

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

        public string? CurrentSegment
        {
            get
            {
                return Segments.FirstOrDefault()?.Segment;
            }
        }

        public string? CurrentQuantity
        {
            get
            {
                return Segments.FirstOrDefault()?.DisplayQuantity;
            }
        }

        private IAssessmentService AssessmentService { get; }

        private IFavoriteRepository FavoriteRepository { get; }

        private IPreferenceService PreferenceService { get; }

        private ISessionService SessionService { get; }

        [IndexerName("Item")]
        public new string this[string key]
        {
            get
            {
                return Resources.Strings.Resources.ResourceManager.GetString(key) ?? string.Empty;
            }
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sections = new List<ObservableObject>();
                    var segments = new List<ModuleScoreEntry>();
                    var moduleTiles = await AssessmentService.GetModuleTilesAsync(_moduleIds ?? new List<string>(), worker.Token).ConfigureAwait(false);
                    var favorites = await FavoriteRepository.GetItemsByTypeAsync(nameof(ModuleTile), worker.Token).ConfigureAwait(false) ?? new List<Favorite>();
                    var modules = new List<ModuleTileEntry>();
                    var quantity_Patter = "Quantity_{0}";
                    foreach (var moduleTile in moduleTiles)
                    {
                        var route = Routes.ModuleDetailView;
                        var parameters = new NavigationParameters()
                                    {
                                        { NavigationParameter.Id, moduleTile.ModuleId },
                                        { NavigationParameter.Type, moduleTile.Type.ToString() },
                                    };

                        modules.Add(ModuleTileEntry.Import(moduleTile, route, parameters, favorites.Any(f => f.Id == moduleTile.ModuleId), Interact, CanInteract, ToggleFavorite, CanToggleFavorite));
                        segments.Add(ModuleScoreEntry.Import(moduleTile.ModuleScore, this[string.Format(quantity_Patter, moduleTile.ModuleScore.Quantity)], moduleTile.Type));
                    }

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(modules, segments), worker.Token);
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
            _moduleIds = navigationParameters.GetValue<string?>(NavigationParameter.Data)?.Split(';') ?? Array.Empty<string>();

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
            ToggleSegmentSelectionCommand?.NotifyCanExecuteChanged();
            var items = Modules.ToList();
            foreach (var item in items)
            {
                item.RefreshCommands();
            }
        }

        private void LoadonUIThread(IEnumerable<ModuleTileEntry> modules, IEnumerable<ModuleScoreEntry> segments)
        {
            Modules = new ObservableCollection<ModuleTileEntry>(modules);
            Segments = new List<ModuleScoreEntry>(segments);
            OnPropertyChanged(nameof(CurrentSegment));
            OnPropertyChanged(nameof(CurrentQuantity));
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

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanToggleSegmentSelection))]
        private Task ToggleSegmentSelection(string direction, CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(ToggleSegmentSelectionCommand)} in {GetType().Name}.");
            var dir = Convert.ToInt32(direction);
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var items = Segments.ToList();
                    if (dir < 0)
                    {
                        var item = items.Last();
                        items.Remove(item);
                        items.Insert(0, item);
                    }
                    else
                    {
                        var item = items.First();
                        items.Remove(item);
                        items.Add(item);
                    }

                    Segments = new List<ModuleScoreEntry>(items);
                    OnPropertyChanged(nameof(CurrentSegment));
                    OnPropertyChanged(nameof(CurrentQuantity));
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ToggleSegmentSelection)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ToggleSegmentSelection)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(ToggleSegmentSelection)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }

                return Task.CompletedTask;
            }
        }

        private bool CanToggleSegmentSelection(string direction)
        {
            return !IsBusy && Segments.Any();
        }
    }
}
