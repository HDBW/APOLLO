// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using Apollo.Common.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Converter;
using De.HDBW.Apollo.Client.Enums;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

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
        private ObservableCollection<BasicViewInteractionEntry> _searchResults = new ObservableCollection<BasicViewInteractionEntry>();

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
            Filter = CreateDefaultFilter(string.Empty);
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
                    await Task.Run(() => LoadSuggestionsAsync(inputValue, worker.Token), worker.Token);
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(LoadSuggestionsAsync)} in {GetType().Name}.");
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
                    await NavigationService.NavigateAsnc(navigationData.Route, CancellationToken.None, navigationData.Parameters);
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
                    var converter = new CourseTagTypeToStringConverter();
                    UpdateFilter(query);

                    var visibleFields = new List<string>()
                    {
                        nameof(Training.Id),
                        nameof(Training.ProviderId),
                        nameof(Training.TrainingName),
                        nameof(Training.Description),
                        nameof(Training.ShortDescription),
                        nameof(Training.TrainingProvider),
                        nameof(Training.Content),
                        nameof(Training.BenefitList),
                        nameof(Training.CourseProvider),
                        nameof(Training.TargetAudience),
                        nameof(Training.ProductUrl),
                        nameof(Training.Price),
                        nameof(Training.Tags),
                        nameof(Training.PublishingDate),
                        nameof(Training.UnpublishingDate),
                        nameof(Training.Appointment),
                    };

                    var trainingItems = await TrainingService.SearchTrainingsAsync(Filter, visibleFields, null, null, worker.Token);
                    trainingItems = trainingItems ?? Array.Empty<Training>();
                    var interactions = new List<BasicViewInteractionEntry>();
                    foreach (var trainingItem in trainingItems)
                    {
                        var decoratorText = string.Join(", ", trainingItem.Tags ?? new List<string>());

                        var courseData = new NavigationParameters();
                        courseData.AddValue<string?>(NavigationParameter.Id, trainingItem.Id);
                        var data = new NavigationData(Routes.CourseView, courseData);

                        //ToDo
                        var duration = trainingItem.Appointment?.FirstOrDefault()?.Duration.ToString() ?? string.Empty;

                        var provider = !string.IsNullOrWhiteSpace(trainingItem.CourseProvider?.Name) ? trainingItem.CourseProvider.Name : Resources.Strings.Resources.StartViewModel_UnknownProvider;
                        var image = "placeholdercontinuingeducation.png";

                        //ToDo

                        /*
                        switch (course.)
                        {
                            case CourseTagType.InfoEvent:
                                image = "placeholderinfoevent.png";
                                break;
                        }
                        */

                        var interaction = BasicViewInteractionEntry.Import<Training>(trainingItem?.TrainingName ?? string.Empty, provider, decoratorText, duration, image, Status.Unknown, trainingItem?.Id ?? string.Empty, data, HandleToggleIsFavorite, CanHandleToggleIsFavorite, HandleInteract, CanHandleInteract);
                        interaction.IsFavorite = SessionService.GetFavorites().Any(f => f.Id == trainingItem?.Id && f.Type == typeof(Training));
                        interactions.Add(interaction);
                    }

                    if (!interactions.Any() || string.IsNullOrWhiteSpace(query))
                    {
                        await ExecuteOnUIThreadAsync(() => LoadonUIThread(interactions), worker.Token).ConfigureAwait(false);
                        return;
                    }

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

                    await ExecuteOnUIThreadAsync(
                        () =>
                        {
                            LoadonUIThread(interactions);
                            LoadonUIThread(historyEntry, suggestionEntry, history);
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
                catch (MsalException ex)
                {
                    Logger?.LogWarning(ex, $"Error while searching course-item in {GetType().Name}.");
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

        private bool CanHandleToggleIsFavorite(BasicViewInteractionEntry entry)
        {
            return entry != null;
        }

        private Task HandleToggleIsFavorite(BasicViewInteractionEntry entry)
        {
            entry.IsFavorite = !entry.IsFavorite;
            if (entry.IsFavorite)
            {
                SessionService.AddFavorite(entry.EntityId.ToString(), entry.EntityType);
            }
            else
            {
                SessionService.RemoveFavorite(entry.EntityId.ToString(), entry.EntityType);
            }

            return Task.CompletedTask;
        }

        private void LoadonUIThread(HistoricalSuggestionEntry? historyEntry, SearchSuggestionEntry? suggestionEntry, SearchHistory history)
        {
            Recents = new ObservableCollection<HistoricalSuggestionEntry>();
            Suggestions = new ObservableCollection<SearchSuggestionEntry>();
        }

        private void LoadonUIThread(IEnumerable<BasicViewInteractionEntry> interactionEntries)
        {
            SearchResults = new ObservableCollection<BasicViewInteractionEntry>(interactionEntries);
        }

        private async Task LoadSuggestionsAsync(string inputValue, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            UpdateFilter(inputValue);
            var suggestions = inputValue?.Length > 3 ? await TrainingService.SearchSuggesionsAsync(Filter, 0, _maxSugestionItemsCount, token).ConfigureAwait(false) : Array.Empty<Training>();
            var recents = await SearchHistoryRepository.GetMaxItemsAsync(_maxHistoryItemsCount, inputValue, token).ConfigureAwait(false);
            if (!(recents?.Any() ?? false))
            {
                recents = await SearchHistoryRepository.GetMaxItemsAsync(_maxHistoryItemsCount, null, token).ConfigureAwait(false);
            }

            recents = recents ?? Array.Empty<SearchHistory>();
            suggestions = suggestions.Take(_maxSugestionItemsCount) ?? Array.Empty<Training>();
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

        private Filter CreateDefaultFilter(string query)
        {
            var filter = Filter.CreateQuery(nameof(Training.TrainingName), new List<object>() { query }, QueryOperator.Contains);
            filter.AddExpression(nameof(Training.Description), new List<object>() { query }, QueryOperator.Contains);
            filter.AddExpression(nameof(Training.ShortDescription), new List<object>() { query }, QueryOperator.Contains);
            filter.AddExpression(nameof(Training.TrainingProvider), new List<object>() { query }, QueryOperator.Contains);
            filter.AddExpression(nameof(Training.CourseProvider), new List<object>() { query }, QueryOperator.Contains);
            filter.IsOrOperator = true;
            return filter;
        }

        private void UpdateFilter(string query)
        {
            if (Filter == null)
            {
                Filter = CreateDefaultFilter(query);
                return;
            }

            var defaultFilter = CreateDefaultFilter(query);
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
