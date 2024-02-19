// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        public SearchViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ISessionService sessionService,
            ISheetService sheetService,
            ITrainingService trainingService,
            ISearchHistoryRepository searchHistoryRepository,
            ILogger<RegistrationViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(sessionService);
            ArgumentNullException.ThrowIfNull(sheetService);
            ArgumentNullException.ThrowIfNull(trainingService);
            ArgumentNullException.ThrowIfNull(searchHistoryRepository);
            SessionService = sessionService;
            SheetService = sheetService;
            TrainingService = trainingService;
            SearchHistoryRepository = searchHistoryRepository;
            Filter = CreateDefaultTrainingsFilter(string.Empty);
        }

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
            return !IsBusy && (SearchResults?.Any() ?? false);
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
        private async Task OpenFilterSheet()
        {
            var parameters = new NavigationParameters();
            parameters.Add(NavigationParameter.Data, JsonConvert.SerializeObject(Filter));
            await SheetService.OpenAsync(Routes.SearchFilterSheet, CancellationToken.None, parameters);
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
                        nameof(TrainingModel.Price),
                        $"{nameof(TrainingModel.TrainingProvider)}.{nameof(EduProvider.Name)}",
                        $"{nameof(TrainingModel.TrainingProvider)}.{nameof(EduProvider.Image)}",
                        $"{nameof(TrainingModel.CourseProvider)}.{nameof(EduProvider.Name)}",
                        $"{nameof(TrainingModel.CourseProvider)}.{nameof(EduProvider.Image)}",
                    };

            var items = await TrainingService.SearchTrainingsAsync(filter, visibleFields, null, null, token);
            items = items ?? new List<TrainingModel>();
            var result = new List<SearchInteractionEntry>();
            result.AddRange(CreateTrainingResults(items));

            return result;
        }

        private IEnumerable<SearchInteractionEntry> CreateTrainingResults(IEnumerable<TrainingModel> items)
        {
            var trainings = new List<SearchInteractionEntry>();
            foreach (var item in items)
            {
                var parts = new List<string>();
                {
                    parts.Add(item.TrainingName);
                    parts.Add(item.ShortDescription);
                }

                var text = string.Join(" - ", parts.Where(x => !string.IsNullOrWhiteSpace(x)));

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

                var subline = eduProvider?.Name ?? Resources.Strings.Resources.StartViewModel_UnknownProvider;
                var sublineImagePath = eduProvider?.Image?.OriginalString;
                var info = $"{item.Price ?? 0:0.##} €";

                var parameters = new NavigationParameters();
                parameters.AddValue(NavigationParameter.Id, item.Id);
                var data = new NavigationData(Routes.TrainingView, parameters);

                var interaction = SearchInteractionEntry.Import(text, subline, sublineImagePath, decoratorText, decoratorImagePath, info, data, HandleToggleIsFavorite, CanHandleToggleIsFavorite, HandleInteract, CanHandleInteract);

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
    }
}
