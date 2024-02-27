// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using TrainingModel = Invite.Apollo.App.Graph.Common.Models.Trainings.Training;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class SearchViewModel : BaseViewModel, ILoadSuggestionsProvider
    {
        private readonly int _maxHistoryItemsCount = 10;
        private readonly int _maxSugestionItemsCount = 10;
        private readonly ConcurrentDictionary<Uri, List<IProvideImageData>> _loadingCache = new ConcurrentDictionary<Uri, List<IProvideImageData>>();

        [ObservableProperty]
        private ObservableCollection<SearchSuggestionEntry> _suggestions = new ObservableCollection<SearchSuggestionEntry>();

        [ObservableProperty]
        private ObservableCollection<HistoricalSuggestionEntry> _recents = new ObservableCollection<HistoricalSuggestionEntry>();

        [ObservableProperty]
        private ObservableCollection<SearchInteractionEntry> _searchResults = new ObservableCollection<SearchInteractionEntry>();

        private CancellationTokenSource? _loadingCts;
        private List<Task>? _loadingTask;

        private Filter? _customFilter;
        private string? _query;

        private List<TrainingModel> _trainings = new List<TrainingModel>();

        private decimal _maxPrice;

        public SearchViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ISessionService sessionService,
            ISheetService sheetService,
            ITrainingService trainingService,
            ISearchHistoryRepository searchHistoryRepository,
            IImageCacheService imageCacheService,
            ILogger<RegistrationViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(sessionService);
            ArgumentNullException.ThrowIfNull(sheetService);
            ArgumentNullException.ThrowIfNull(trainingService);
            ArgumentNullException.ThrowIfNull(searchHistoryRepository);
            ArgumentNullException.ThrowIfNull(imageCacheService);
            SessionService = sessionService;
            SheetService = sheetService;
            TrainingService = trainingService;
            SearchHistoryRepository = searchHistoryRepository;
            ImageCacheService = imageCacheService;
            Filter = CreateDefaultTrainingsFilter(string.Empty);
            WeakReferenceMessenger.Default.Register<FilterChangedMessage>(this, OnFilterChangedMessage);
            WeakReferenceMessenger.Default.Register<SheetDismissedMessage>(this, OnSheetDismissedMessage);
        }

        public bool HasFilter
        {
            get
            {
                return _customFilter?.Fields?.Any() ?? false;
            }
        }

        public string FilterIcon
        {
            get
            {
                return HasFilter ? "filteractive.png" : "filter.png";
            }
        }

        private IImageCacheService ImageCacheService { get; }

        private ISessionService SessionService { get; }

        private ISheetService SheetService { get; }

        private ITrainingService TrainingService { get; }

        private ISearchHistoryRepository SearchHistoryRepository { get; }

        private Filter Filter { get; set; }

        public async void StartLoadSuggestions(string inputValue)
        {
            using (var worker = ScheduleWork())
            {
                var token = worker.Token;
                try
                {
                    token.ThrowIfCancellationRequested();
                    Suggestions.Clear();
                    await Task.Run(() => LoadTrainingsSuggestionsAsync(inputValue, worker.Token), worker.Token);
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(LoadTrainingsSuggestionsAsync)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var history = await SearchHistoryRepository.GetMaxItemsAsync(_maxHistoryItemsCount, null, worker.Token).ConfigureAwait(false);
                    if (!(history?.Any() ?? false))
                    {
                        return;
                    }

                    var recents = history.Select(h => HistoricalSuggestionEntry.Import(h));
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(recents), worker.Token);
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
            SearchCommand?.NotifyCanExecuteChanged();
            OpenFilterSheetCommand?.NotifyCanExecuteChanged();
            foreach (var result in SearchResults)
            {
                result.NavigateCommand?.NotifyCanExecuteChanged();
            }
        }

        private bool CanSearch(object queryElement)
        {
            return !IsBusy && queryElement != null;
        }

        private bool CanOpenFilterSheet()
        {
            return !IsBusy && ((SearchResults?.Any() ?? false) || _customFilter != null) && !SheetService.IsShowingSheet;
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

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenFilterSheet))]
        private async Task OpenFilterSheet(CancellationToken token)
        {
            Logger.LogDebug("OpenFilterSheet START");
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var parameters = new NavigationParameters
                    {
                        { NavigationParameter.Data, JsonConvert.SerializeObject(_customFilter) },
                    };
                    parameters.AddValue<decimal>(NavigationParameter.SavedState, _maxPrice);
                    await SheetService.OpenAsync(Routes.SearchFilterSheet, worker.Token, parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenFilterSheet)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenFilterSheet)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenFilterSheet)} in {GetType().Name}.");
                }
                finally
                {
                    Logger.LogDebug("OpenFilterSheet UnscheduleWork start");
                    UnscheduleWork(worker);
                    Logger.LogDebug("OpenFilterSheet UnscheduleWork end");
                }
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearch))]
        private async Task Search(object queryElement, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    string? query = null;
                    HistoricalSuggestionEntry? historyEntry = null;
                    SearchSuggestionEntry? suggestionEntry = null;
                    switch (queryElement)
                    {
                        case string input:
                            query = input;
                            break;
                        case HistoricalSuggestionEntry entry:
                            historyEntry = entry;
                            query = entry.Name;
                            break;
                        case SearchSuggestionEntry entry:
                            suggestionEntry = entry;
                            query = entry.Name;
                            break;
                    }

                    token.ThrowIfCancellationRequested();

                    query = query ?? string.Empty;
                    UpdateTrainingsFilter(query);
                    var interactions = await SearchTrainingsAsync(Filter, worker.Token).ConfigureAwait(false);
                    if (!interactions.Any() || string.IsNullOrWhiteSpace(query))
                    {
                        await ExecuteOnUIThreadAsync(() => LoadonUIThread(interactions), worker.Token).ConfigureAwait(false);
                        return;
                    }

                    await SaveSearchHistoryAsync(query, historyEntry, suggestionEntry, worker.Token).ConfigureAwait(false);

                    await ExecuteOnUIThreadAsync(
                        () =>
                        {
                            LoadonUIThread(interactions);
                            ClearSuggesionsAndHistory();
                        }, worker.Token);

                    _loadingCts?.Cancel();
                    _loadingCts = new CancellationTokenSource();
                    _loadingTask = new List<Task>();
                    var loadingToken = _loadingCts.Token;
                    foreach (var url in _loadingCache.Keys.ToList())
                    {
                        _loadingTask.Add(ImageCacheService.DownloadAsync(url, loadingToken).ContinueWith(OnApplyImageData));
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Search)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Search)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Search)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
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

        private async Task SaveSearchHistoryAsync(string query, HistoricalSuggestionEntry? historyEntry, SearchSuggestionEntry? suggestionEntry, CancellationToken token)
        {
            SearchHistory? history = null;
            long time = DateTime.UtcNow.Ticks;
            if (historyEntry != null)
            {
                history = historyEntry.Export();
                history.Ticks = time;
            }
            else if (suggestionEntry != null)
            {
                history = new SearchHistory()
                {
                    Query = query,
                    Ticks = time,
                };
            }
            else
            {
                history = await SearchHistoryRepository.GetItemsByQueryAsync(query, CancellationToken.None).ConfigureAwait(false);
                history = history ?? new SearchHistory()
                {
                    Query = query,
                };

                history.Ticks = DateTime.UtcNow.Ticks;
            }

            if (history.Id == 0)
            {
                await SearchHistoryRepository.AddItemAsync(history, CancellationToken.None).ConfigureAwait(false);
            }
            else
            {
                await SearchHistoryRepository.UpdateItemAsync(history, CancellationToken.None).ConfigureAwait(false);
            }
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
                    };

            var items = await TrainingService.SearchTrainingsAsync(filter, visibleFields, null, null, token);
            items = items ?? new List<TrainingModel>();
            _trainings = items.ToList();
            _customFilter = null;
            var result = new List<SearchInteractionEntry>();
            result.AddRange(CreateTrainingResults(_trainings, true));
            return result;
        }

        private IEnumerable<TrainingModel> TryFilter(IEnumerable<TrainingModel> items, Filter? customFilter)
        {
            if (items == null)
            {
                return new List<TrainingModel>();
            }

            if (customFilter == null)
            {
                return items;
            }

            bool? loansArg = customFilter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.LoansFieldName)?.Argument?.OfType<bool>().FirstOrDefault();
            bool? individualStartDateArg = customFilter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.IndividualStartDateFieldName)?.Argument?.OfType<bool>().FirstOrDefault();
            bool? accessibilityAvailableArg = customFilter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.AccessibilityAvailableFieldName)?.Argument?.OfType<bool>().FirstOrDefault();
            decimal? minPrice = customFilter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.PriceFieldName)?.Argument?.OfType<decimal>().Min();
            decimal? maxPrice = customFilter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.PriceFieldName)?.Argument?.OfType<decimal>().Max();
            List<TrainingMode>? trainingModes = customFilter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.TrainingsModeFieldName)?.Argument?.OfType<TrainingMode>().ToList();
            List<TrainingTimeModel>? trainingTimes = customFilter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.AppointmenTrainingsTimeModelFieldName)?.Argument?.OfType<TrainingTimeModel>().ToList();
            var validItems = items.Where(x =>
                FilterByLoans(x, loansArg)
                && FilterByIndividualStartDate(x, individualStartDateArg)
                && FilterByAccessibilityAvailable(x, accessibilityAvailableArg)
                && FilterByPrice(x, (double?)minPrice, (double?)maxPrice)
                && FilterByTrainingsMode(x, trainingModes)
                && FilterByTrainingsTimeModel(x, trainingTimes));

            return validItems.Distinct();
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

                var interaction = SearchInteractionEntry.Import(text, subline, sublineImagePath, decoratorText, decoratorImagePath, info, data, HandleToggleIsFavorite, CanHandleToggleIsFavorite, HandleInteract, CanHandleInteract);
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

                // interaction.IsFavorite = SessionService.GetFavorites().Any(f => f.Id == trainingItem?.Id && f.Type == typeof(Training));
                trainings.Add(interaction);
            }

            return trainings;
        }

        private bool CanHandleToggleIsFavorite(SearchInteractionEntry entry)
        {
            return entry != null;
        }

        private Task HandleToggleIsFavorite(SearchInteractionEntry entry)
        {
            entry.IsFavorite = !entry.IsFavorite;
            if (entry.IsFavorite)
            {
                // SessionService.AddFavorite(entry.EntityId.ToString(), entry.EntityType);
            }
            else
            {
                // SessionService.RemoveFavorite(entry.EntityId.ToString(), entry.EntityType);
            }

            return Task.CompletedTask;
        }

        private void ClearSuggesionsAndHistory()
        {
            Recents = new ObservableCollection<HistoricalSuggestionEntry>();
            Suggestions = new ObservableCollection<SearchSuggestionEntry>();
        }

        private void LoadonUIThread(IEnumerable<SearchInteractionEntry> interactionEntries)
        {
            SearchResults = new ObservableCollection<SearchInteractionEntry>(interactionEntries);
            OnPropertyChanged(nameof(HasFilter));
            OnPropertyChanged(nameof(FilterIcon));
        }

        private async Task LoadTrainingsSuggestionsAsync(string inputValue, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            UpdateTrainingsFilter(inputValue);
            var suggestions = inputValue?.Length >= 2 ? await TrainingService.SearchSuggesionsAsync(Filter, null, _maxSugestionItemsCount, token).ConfigureAwait(false) : Array.Empty<TrainingModel>();
            var recents = await SearchHistoryRepository.GetMaxItemsAsync(_maxHistoryItemsCount, inputValue, token).ConfigureAwait(false);
            if (!(recents?.Any() ?? false))
            {
                recents = await SearchHistoryRepository.GetMaxItemsAsync(_maxHistoryItemsCount, null, token).ConfigureAwait(false);
            }

            recents = recents ?? Array.Empty<SearchHistory>();
            suggestions = suggestions.Take(_maxSugestionItemsCount) ?? Array.Empty<TrainingModel>();
            var courses = suggestions.Where(x => !string.IsNullOrWhiteSpace(x.TrainingName)).Select(x => SearchSuggestionEntry.Import(x.TrainingName)).ToList();
            var history = recents.Take(Math.Max(_maxHistoryItemsCount - courses.Count, 0)).Select(x => HistoricalSuggestionEntry.Import(x)).ToList();
            await ExecuteOnUIThreadAsync(
                () =>
                {
                    Recents = new ObservableCollection<HistoricalSuggestionEntry>(history);
                    Suggestions = new ObservableCollection<SearchSuggestionEntry>(courses);
                }, token);
        }

        private void LoadonUIThread(IEnumerable<HistoricalSuggestionEntry> recents)
        {
            Recents = new ObservableCollection<HistoricalSuggestionEntry>(recents);
        }

        private Filter CreateDefaultTrainingsFilter(string query)
        {
            var filter = Filter.CreateQuery(nameof(TrainingModel.TrainingName), new List<object>() { query }, QueryOperator.Contains);
            filter.AddExpression(nameof(TrainingModel.Description), new List<object>() { query }, QueryOperator.Contains);
            filter.AddExpression(nameof(TrainingModel.ShortDescription), new List<object>() { query }, QueryOperator.Contains);
            filter.AddExpression($"{nameof(TrainingModel.TrainingProvider)}.{nameof(EduProvider.Name)}", new List<object>() { query }, QueryOperator.Contains);
            filter.AddExpression($"{nameof(TrainingModel.CourseProvider)}.{nameof(EduProvider.Name)}", new List<object>() { query }, QueryOperator.Contains);
            filter.IsOrOperator = true;
            return filter;
        }

        private void UpdateTrainingsFilter(string query)
        {
            _query = query;
            if (Filter == null)
            {
                Filter = CreateDefaultTrainingsFilter(query);
                return;
            }

            var defaultFilter = CreateDefaultTrainingsFilter(query);
            var defaultFieldNames = defaultFilter.Fields.Select(x => x.FieldName);
            foreach (var fieldExpression in Filter.Fields)
            {
                if (defaultFieldNames.Contains(fieldExpression.FieldName))
                {
                    fieldExpression.Argument = new List<object> { query };
                }
            }
        }

        private void OnFilterChangedMessage(object recipient, FilterChangedMessage message)
        {
            _customFilter = message.Filter;
            var filteredTrainings = TryFilter(_trainings, _customFilter);
            var items = CreateTrainingResults(filteredTrainings, false);
            if (MainThread.IsMainThread)
            {
                LoadonUIThread(items);
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() => LoadonUIThread(items));
            }
        }

        private void OnSheetDismissedMessage(object recipient, SheetDismissedMessage message)
        {
            if (MainThread.IsMainThread)
            {
                RefreshCommands();
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(RefreshCommands);
            }
        }

        private bool FilterByLoans(TrainingModel model, bool? argument)
        {
            if (argument == null)
            {
                return true;
            }

            return (model.Loans?.Any() ?? false) == argument!;
        }

        private bool FilterByAccessibilityAvailable(TrainingModel model, bool? argument)
        {
            if (argument == null)
            {
                return true;
            }

            return (model.AccessibilityAvailable ?? false) == argument!;
        }

        private bool FilterByIndividualStartDate(TrainingModel model, bool? argument)
        {
            if (argument == null)
            {
                return true;
            }

            return !string.IsNullOrWhiteSpace(model.IndividualStartDate) == argument!;
        }

        private bool FilterByPrice(TrainingModel model, double? start, double? end)
        {
            if (!start.HasValue || !end.HasValue)
            {
                return true;
            }

            if (!model.Price.HasValue)
            {
                return false;
            }

            return model.Price.Value >= start.Value && model.Price.Value <= end.Value;
        }

        private bool FilterByTrainingsMode(TrainingModel model, List<TrainingMode>? flags)
        {
            if (flags == null || !flags.Any())
            {
                return true;
            }

            if (!model.TrainingMode.HasValue)
            {
                return false;
            }

            return flags.Any(x => (model.TrainingMode & x) != 0);
        }

        private bool FilterByTrainingsTimeModel(TrainingModel model, List<TrainingTimeModel>? values)
        {
            if (values == null || !values.Any())
            {
                return true;
            }

            if (model.Appointment == null || !model.Appointment.Any(x => x.TrainingMode.HasValue))
            {
                return false;
            }

            var supportedModels = model.Appointment.Where(x => x.TimeModel.HasValue).Select(x => x.TimeModel).ToList();
            return values.Any(x => supportedModels.Contains(x));
        }
    }
}
