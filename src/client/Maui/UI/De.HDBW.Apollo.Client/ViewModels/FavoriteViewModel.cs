// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;
using TrainingModel = Invite.Apollo.App.Graph.Common.Models.Trainings.Training;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class FavoriteViewModel : BaseViewModel
    {
        private readonly ConcurrentDictionary<Uri, List<IProvideImageData>> _loadingCache = new ConcurrentDictionary<Uri, List<IProvideImageData>>();

        [ObservableProperty]
        private ObservableCollection<SearchSuggestionEntry> _suggestions = new ObservableCollection<SearchSuggestionEntry>();

        [ObservableProperty]
        private ObservableCollection<HistoricalSuggestionEntry> _recents = new ObservableCollection<HistoricalSuggestionEntry>();

        [ObservableProperty]
        private ObservableCollection<SearchInteractionEntry> _favoriteItems = new ObservableCollection<SearchInteractionEntry>();

        private CancellationTokenSource? _loadingCts;
        private List<Task>? _loadingTask;

        private Filter? _customFilter;
        private string? _query;

        private List<TrainingModel> _trainings = new List<TrainingModel>();

        private decimal _maxPrice;

        public FavoriteViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ISessionService sessionService,
            ISheetService sheetService,
            ITrainingService trainingService,
            ISearchHistoryRepository searchHistoryRepository,
            IImageCacheService imageCacheService,
            IFavoriteRepository favoriteRepository,
            ILogger<RegistrationViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(sessionService);
            ArgumentNullException.ThrowIfNull(sheetService);
            ArgumentNullException.ThrowIfNull(trainingService);
            ArgumentNullException.ThrowIfNull(searchHistoryRepository);
            ArgumentNullException.ThrowIfNull(imageCacheService);
            SessionService = sessionService;
            TrainingService = trainingService;
            ImageCacheService = imageCacheService;
            FavoriteRepository = favoriteRepository;
        }

        private IImageCacheService ImageCacheService { get; }

        private IFavoriteRepository FavoriteRepository { get; }

        private ISessionService SessionService { get; }

        private ITrainingService TrainingService { get; }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var favorites = (await FavoriteRepository.GetItemsByTypeAsync(nameof(Training), worker.Token).ConfigureAwait(false)) ?? new List<Favorite>();

                    var filter = Filter.CreateQuery(nameof(TrainingModel.Id), favorites.Where(x => !string.IsNullOrWhiteSpace(x.Id)).Select(x => x.Id).Cast<object>().ToList(), QueryOperator.Contains);
                    filter.IsOrOperator = true;
                    var interactions = await SearchTrainingsAsync(filter, worker.Token).ConfigureAwait(false);
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(interactions), worker.Token).ConfigureAwait(false);
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
            foreach (var result in FavoriteItems)
            {
                result.NavigateCommand?.NotifyCanExecuteChanged();
            }
        }

        private bool CanSearch(object queryElement)
        {
            return !IsBusy && queryElement != null;
        }

        private bool CanHandleInteract(InteractionEntry interaction)
        {
            return !IsBusy;
        }

        private async Task HandleInteract(InteractionEntry interaction)
        {
            switch (interaction.Data)
            {
                case NavigationData navigationData:
                    await NavigationService.NavigateAsync(navigationData.Route, CancellationToken.None, navigationData.Parameters);
                    break;
                default:
                    Logger.LogWarning($"Unknown interaction data {interaction?.Data ?? "null"} while {nameof(HandleInteract)} in {GetType().Name}.");
                    break;
            }
        }

        private async void OnApplyImageData(Task<(Uri Uri, string? Data)> task)
        {
            var result = task.Result;
            _loadingTask?.Remove(task);
            if (!(_loadingTask?.Any() ?? false))
            {
                _loadingCache.Clear();
                _loadingCts?.Dispose();
                _loadingCts = null;
            }

            if (string.IsNullOrWhiteSpace(result.Data))
            {
                return;
            }

            if (!_loadingCache.TryGetValue(result.Uri, out List<IProvideImageData>? entries))
            {
                return;
            }

            await ExecuteOnUIThreadAsync(
                () =>
                {
                    foreach (var entry in entries)
                    {
                        entry.ImageData = result.Data;
                    }
                }, CancellationToken.None);
        }

        private async Task<IEnumerable<SearchInteractionEntry>> SearchTrainingsAsync(Filter filter, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var visibleFields = new List<string>()
                    {
                        nameof(TrainingModel.Id),
                        nameof(TrainingModel.TrainingName),
                        nameof(TrainingModel.TrainingType),
                        nameof(TrainingModel.ShortDescription),
                        KnownFilters.PriceFieldName,
                        $"{nameof(TrainingModel.TrainingProvider)}.{nameof(EduProvider.Name)}",
                        $"{nameof(TrainingModel.TrainingProvider)}.{nameof(EduProvider.Image)}",
                        $"{nameof(TrainingModel.CourseProvider)}.{nameof(EduProvider.Name)}",
                        $"{nameof(TrainingModel.CourseProvider)}.{nameof(EduProvider.Image)}",
                        KnownFilters.LoansFieldName,
                        KnownFilters.IndividualStartDateFieldName,
                        KnownFilters.AccessibilityAvailableFieldName,
                        KnownFilters.TrainingsModeFieldName,
                        KnownFilters.AppointmenTrainingsModeFieldName,
                        KnownFilters.AppointmenTrainingsTimeModelFieldName,
                        KnownFilters.AppointmenStartDateFieldName,
                        KnownFilters.AppointmenEndDateFieldName,
                        KnownFilters.OccurenceStartDateFieldName,
                        KnownFilters.OccurenceEndDateFieldName,
                    };

            var items = await TrainingService.SearchTrainingsAsync(filter, visibleFields, null, null, token);
            items = items ?? new List<TrainingModel>();
            _trainings = items.ToList();
            _customFilter = null;
            var result = new List<SearchInteractionEntry>();
            result.AddRange(CreateTrainingResults(_trainings, true));
            return result;
        }

        private IEnumerable<SearchInteractionEntry> CreateTrainingResults(IEnumerable<TrainingModel> items, bool setMaxPrice)
        {
            _loadingCache.Clear();
            _loadingCts?.Cancel();
            _loadingCts = null;
            _loadingTask = null;
            var trainings = new List<SearchInteractionEntry>();
            if (setMaxPrice)
            {
                _maxPrice = 0;
            }

            foreach (var item in items)
            {
                var parts = new List<string>();
                {
                    parts.Add(item.TrainingName);
                    parts.Add(item.ShortDescription);
                }

                var text = string.Join(" - ", parts.Where(x => !string.IsNullOrWhiteSpace(x)));
                if (setMaxPrice)
                {
                    _maxPrice = Math.Max(_maxPrice, Convert.ToDecimal(item.Price ?? 0));
                }

                var decoratorText = string.IsNullOrWhiteSpace(item.TrainingType) ? Resources.Strings.Resources.Global_Training : item.TrainingType;
                var decoratorImagePath = KnonwIcons.Training;

                EduProvider? eduProvider = null;
                if (!string.IsNullOrWhiteSpace(item.TrainingProvider?.Name))
                {
                    eduProvider = item.TrainingProvider;
                }
                else if (!string.IsNullOrWhiteSpace(item.CourseProvider?.Name))
                {
                    eduProvider = item.CourseProvider;
                }

                var subline = eduProvider?.Name ?? Resources.Strings.Resources.Global_UnknownProvider;
                var sublineImagePath = eduProvider?.Image?.OriginalString;
                var info = $"{item.Price ?? 0:0.##} €";

                var parameters = new NavigationParameters();
                parameters.AddValue(NavigationParameter.Id, item.Id);
                var data = new NavigationData(Routes.TrainingView, parameters);

                var interaction = SearchInteractionEntry.Import(text, subline, sublineImagePath, decoratorText, decoratorImagePath, info, data, true, true, HandleToggleIsFavorite, CanHandleToggleIsFavorite, HandleInteract, CanHandleInteract);
                if (eduProvider?.Image != null && eduProvider.Image.IsWellFormedOriginalString())
                {
                    if (!_loadingCache.Keys.Any(x => x.OriginalString == eduProvider.Image.OriginalString))
                    {
                        if (!_loadingCache.TryAdd(eduProvider.Image, new List<IProvideImageData>() { interaction }))
                        {
                            _loadingCache[eduProvider.Image].Add(interaction);
                        }
                    }
                    else
                    {
                        _loadingCache[eduProvider.Image].Add(interaction);
                    }
                }

                trainings.Add(interaction);
            }

            return trainings;
        }

        private bool CanHandleToggleIsFavorite(SearchInteractionEntry entry)
        {
            return entry != null;
        }

        private async Task HandleToggleIsFavorite(SearchInteractionEntry entry)
        {
            using (var worker = ScheduleWork())
            {
                var token = worker.Token;
                try
                {
                    token.ThrowIfCancellationRequested();
                    entry.IsFavorite = !entry.IsFavorite;

                    var data = entry.Data as NavigationData;
                    var entryId = data?.parameters?.GetValue<string?>(NavigationParameter.Id);
                    if (string.IsNullOrWhiteSpace(entryId))
                    {
                        return;
                    }

                    var result = false;
                    if (entry.IsFavorite)
                    {
                        result = await FavoriteRepository.SaveAsync(new Favorite(entryId, nameof(Training)), token).ConfigureAwait(false);
                    }
                    else
                    {
                        result = await FavoriteRepository.DeleteFavoriteAsync(entryId, nameof(Training), token).ConfigureAwait(false);
                    }

                    if (!result)
                    {
                        await ExecuteOnUIThreadAsync(() => { entry.IsFavorite = !entry.IsFavorite; }, CancellationToken.None).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(HandleToggleIsFavorite)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private void ClearSuggesionsAndHistory()
        {
            Recents = new ObservableCollection<HistoricalSuggestionEntry>();
            Suggestions = new ObservableCollection<SearchSuggestionEntry>();
        }

        private void LoadonUIThread(IEnumerable<SearchInteractionEntry> interactionEntries)
        {
            FavoriteItems = new ObservableCollection<SearchInteractionEntry>(interactionEntries);
        }
    }
}
