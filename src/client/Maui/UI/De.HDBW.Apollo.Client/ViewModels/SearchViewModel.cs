// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;
using ContactModel = Invite.Apollo.App.Graph.Common.Models.Contact;
using TrainingModel = Invite.Apollo.App.Graph.Common.Models.Trainings.Training;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class SearchViewModel : BaseViewModel, ILoadSuggestionsProvider
    {
        private readonly int _maxHistoryItemsCount = 10;
        private readonly int _maxSugestionItemsCount = 10;

        [ObservableProperty]
        private ObservableCollection<SearchSuggestionEntry> _suggestions = new ObservableCollection<SearchSuggestionEntry>();

        [ObservableProperty]
        private ObservableCollection<HistoricalSuggestionEntry> _recents = new ObservableCollection<HistoricalSuggestionEntry>();

        [ObservableProperty]
        private ObservableCollection<SearchInteractionEntry> _searchResults = new ObservableCollection<SearchInteractionEntry>();

        private Filter? _customFilter;
        private string? _query;
        private List<TrainingModel> _trainings = new List<TrainingModel>();
        private decimal _maxPrice;
        private ICollection<Favorite> _favorites = new List<Favorite>();

        public SearchViewModel(
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
            SheetService = sheetService;
            TrainingService = trainingService;
            SearchHistoryRepository = searchHistoryRepository;
            ImageCacheService = imageCacheService;
            FavoriteRepository = favoriteRepository;
            Filter = CreateDefaultTrainingsFilter(string.Empty);
            WeakReferenceMessenger.Default.Register<FilterChangedMessage>(this, OnFilterChangedMessage);
            WeakReferenceMessenger.Default.Register<SheetDismissedMessage>(this, OnSheetDismissedMessage);
            WeakReferenceMessenger.Default.Register<ShellContentChangedMessage>(this, OnShellContentChangedMessage);
            WeakReferenceMessenger.Default.Register<FlyoutStateChangedMessage>(this, OnFlyoutStateChangedMessage);
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
                return HasFilter ? KnownIcons.FilterActive : KnownIcons.Filter;
            }
        }

        public bool IsRegistered
        {
            get
            {
                return SessionService?.HasRegisteredUser ?? false;
            }
        }

        public string FavoriteIcon
        {
            get
            {
                return KnownIcons.Favorites;
            }
        }

        private IImageCacheService ImageCacheService { get; }

        private IFavoriteRepository FavoriteRepository { get; }

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

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    _favorites = (await FavoriteRepository.GetItemsByTypeAsync(nameof(Training), worker.Token))?.ToList() ?? new List<Favorite>();
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
            Logger.LogInformation($"Invoked {nameof(OpenFilterSheetCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var filter = SerializationHelper.Serialize(_customFilter);
                    var parameters = new NavigationParameters
                    {
                        { NavigationParameter.Data, filter ?? string.Empty },
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
                    UnscheduleWork(worker);
                }
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearch))]
        private async Task Search(object queryElement, CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(SearchCommand)} in {GetType().Name}.");
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
                        nameof(TrainingModel.ProviderId),
                        $"{nameof(TrainingModel.Appointment)}.{nameof(Appointment.AppointmentLocation)}.{nameof(ContactModel.City)}",
                        $"{nameof(TrainingModel.Appointment)}.{nameof(Appointment.Occurences)}.{nameof(Occurence.Location)}.{nameof(ContactModel.City)}",
                        KnownFilters.PriceFieldName,
                        $"{nameof(TrainingModel.TrainingProvider)}.{nameof(EduProvider.Name)}",
                        $"{nameof(TrainingModel.CourseProvider)}.{nameof(EduProvider.Name)}",
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

            var items = await TrainingService.SearchTrainingsAsync(filter, visibleFields, null, null, true, token);
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
            DateTime? startDate = customFilter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.AppointmenStartDateFieldName)?.Argument?.OfType<DateTime>().FirstOrDefault();
            DateTime? endDate = customFilter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.AppointmenEndDateFieldName)?.Argument?.OfType<DateTime>().FirstOrDefault();

            var validItems = items.Where(x =>
                FilterByLoans(x, loansArg)
                && FilterByIndividualStartDate(x, individualStartDateArg)
                && FilterByAccessibilityAvailable(x, accessibilityAvailableArg)
                && FilterByPrice(x, (double?)minPrice, (double?)maxPrice)
                && FilterByTrainingsMode(x, trainingModes)
                && FilterByTrainingsTimeModel(x, trainingTimes)
                && FilterByDate(x, startDate, endDate));

            return validItems.Distinct();
        }

        private IEnumerable<SearchInteractionEntry> CreateTrainingResults(IEnumerable<TrainingModel> items, bool setMaxPrice)
        {
            var trainings = new List<SearchInteractionEntry>();
            if (setMaxPrice)
            {
                _maxPrice = 0;
            }

            foreach (var item in items)
            {
                var text = item.TrainingName;
                if (setMaxPrice)
                {
                    _maxPrice = Math.Max(_maxPrice, Convert.ToDecimal(item.Price ?? 0));
                }

                var decoratorText = string.IsNullOrWhiteSpace(item.TrainingType) ? Resources.Strings.Resources.Global_Training : item.TrainingType;
                var decoratorImagePath = KnownIcons.Training;

                EduProvider? eduProvider = null;
                if (!string.IsNullOrWhiteSpace(item.TrainingProvider?.Name))
                {
                    eduProvider = item.TrainingProvider;
                }
                else if (!string.IsNullOrWhiteSpace(item.CourseProvider?.Name))
                {
                    eduProvider = item.CourseProvider;
                }

                var subline = string.IsNullOrWhiteSpace(item.ShortDescription) || text == item.ShortDescription ? null : item.ShortDescription;
                var price = item.Price ?? -1d;
                var infoParts = new List<string>();
                infoParts.Add(price == -1d ? Resources.Strings.Resources.Global_PriceOnRequest : price == 0 ? Resources.Strings.Resources.Global_PriceFreeOfCharge : $"{price:0.##} €");
                infoParts.Add(Environment.NewLine);
                var needsSpace = true;
                if (item.Loans?.Any() ?? false)
                {
                    infoParts.Add(Resources.Strings.Resources.HasLoans);
                    infoParts.Add(Environment.NewLine);
                }

                var startDates = new List<DateTime>();
                var locations = new List<string>();
                foreach (var appointment in item.Appointment ?? new List<Appointment>())
                {
                    if (!string.IsNullOrWhiteSpace(appointment.AppointmentLocation?.City))
                    {
                        locations.Add(appointment.AppointmentLocation.City);
                    }

                    startDates.Add(appointment.StartDate);
                    foreach (var occurence in appointment.Occurences)
                    {
                        if (!string.IsNullOrWhiteSpace(occurence.Location?.City))
                        {
                            locations.Add(occurence.Location.City);
                        }

                        startDates.Add(occurence.StartDate);
                    }
                }

                startDates = startDates.Distinct().ToList();
                locations = locations.Distinct().ToList();
                if (needsSpace)
                {
                    infoParts.Add(Environment.NewLine);
                    needsSpace = false;
                }

                if (startDates.Any())
                {
                    infoParts.Add(startDates.Count == 1 ? startDates.First().ToLongDateFormatStringWithoutDay() : Resources.Strings.Resources.MultipleAppointments);
                    infoParts.Add(Environment.NewLine);
                }
                else
                {
                    infoParts.Add(Resources.Strings.Resources.AppointmentOnRequest);
                    infoParts.Add(Environment.NewLine);
                }

                if (locations.Any())
                {
                    infoParts.Add(locations.Count == 1 ? locations.First() : Resources.Strings.Resources.MultipleLocations);
                    infoParts.Add(Environment.NewLine);
                }

                needsSpace = true;
                if (!string.IsNullOrWhiteSpace(eduProvider?.Name))
                {
                    if (needsSpace)
                    {
                        infoParts.Add(Environment.NewLine);
                        needsSpace = false;
                    }

                    infoParts.Add(eduProvider.Name);
                }

                var info = string.Join(string.Empty, infoParts);

                var parameters = new NavigationParameters();
                parameters.AddValue(NavigationParameter.Id, item.Id);
                var data = new NavigationData(Routes.TrainingView, parameters);

                var isFavorite = _favorites.Any(f => f?.Id == item.Id);
                var canBeMadeFavorite = SessionService.HasRegisteredUser && !KnownEduProviders.FavoriteDisabledProviders.Any(id => item.ProviderId == id);

                var interaction = SearchInteractionEntry.Import(text, subline, null, decoratorText, decoratorImagePath, info, data, canBeMadeFavorite, isFavorite, HandleToggleIsFavorite, CanHandleToggleIsFavorite, HandleInteract, CanHandleInteract);
                trainings.Add(interaction);
            }

            return trainings;
        }

        private bool CanHandleToggleIsFavorite(SearchInteractionEntry entry)
        {
            return entry != null && entry.CanBeMadeFavorite;
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

                    if (result)
                    {
                        _favorites = (await FavoriteRepository.GetItemsByTypeAsync(nameof(Training), CancellationToken.None).ConfigureAwait(false))?.ToList() ?? new List<Favorite>();
                    }
                    else
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
            var favoriteIds = _favorites?.Select(x => x.Id) ?? new List<string>();
            foreach (var searchResult in SearchResults)
            {
                if (!searchResult.CanBeMadeFavorite)
                {
                    continue;
                }

                var navigationData = searchResult.Data as NavigationData;
                if (navigationData == null)
                {
                    continue;
                }

                var id = navigationData.Parameters?.GetValue<string>(NavigationParameter.Id);
                if (string.IsNullOrWhiteSpace(id))
                {
                    continue;
                }

                searchResult.IsFavorite = favoriteIds.Contains(id);
            }

            OnPropertyChanged(nameof(IsRegistered));
            WeakReferenceMessenger.Default.Send(new UpdateToolbarMessage());
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

        private void OnFlyoutStateChangedMessage(object recipient, FlyoutStateChangedMessage message)
        {
            if (MainThread.IsMainThread)
            {
                UnscheduleAllWork();
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() => UnscheduleAllWork());
            }
        }

        private void OnShellContentChangedMessage(object recipient, ShellContentChangedMessage message)
        {
            if (message.NewViewModelType != typeof(SearchViewModel))
            {
                if (MainThread.IsMainThread)
                {
                    CleanUp();
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() => SearchResults = new ObservableCollection<SearchInteractionEntry>());
                }
            }
        }

        private void CleanUp()
        {
            UnscheduleAllWork();
            SearchResults = new ObservableCollection<SearchInteractionEntry>();
            _customFilter = null;
            _query = null;
            _trainings = new List<TrainingModel>();
            OnPropertyChanged(nameof(HasFilter));
            OnPropertyChanged(nameof(FilterIcon));
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

        private bool FilterByDate(TrainingModel model, DateTime? start, DateTime? end)
        {
            if (start == null && end == null)
            {
                return true;
            }

            if (model.Appointment == null || model.Appointment.Any())
            {
                return false;
            }

            var appointmentDates = model.Appointment.Select(x => (x.StartDate, x.EndDate)).ToList();
            if (appointmentDates.Any(x => x.StartDate.Date <= (start?.Date ?? x.StartDate.Date) && x.EndDate.Date >= (end?.Date ?? x.EndDate.Date)))
            {
                return true;
            }

            var occurencesDates = model.Appointment.Where(x => (x.Occurences?.Any() ?? false)).SelectMany(x => x.Occurences).Select(x => (x.StartDate, x.EndDate)).ToList();
            return occurencesDates.Any(x => x.StartDate.Date <= (start?.Date ?? x.StartDate.Date) && x.EndDate.Date >= (end?.Date ?? x.EndDate.Date));
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenFavorites))]
        private async Task OpenFavorites(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.NavigateAsync(Routes.TrainingsFavoritesView!, worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenFavorites)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenFavorites)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(OpenFavorites)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanOpenFavorites()
        {
            return !IsBusy && IsRegistered;
        }
    }
}
