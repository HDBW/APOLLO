// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using Apollo.Common.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Course;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

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
        private ObservableCollection<CourseItemEntry> _searchResults = new ObservableCollection<CourseItemEntry>();

        public SearchViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ICourseItemRepository courseItemRepository,
            IEduProviderItemRepository eduProviderItemRepository,
            ITrainingService trainingService,
            ISearchHistoryRepository searchHistoryRepository,
            ILogger<RegistrationViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(courseItemRepository);
            ArgumentNullException.ThrowIfNull(eduProviderItemRepository);
            ArgumentNullException.ThrowIfNull(trainingService);
            ArgumentNullException.ThrowIfNull(searchHistoryRepository);
            CourseItemRepository = courseItemRepository;
            EduProviderItemRepository = eduProviderItemRepository;
            TrainingService = trainingService;
            SearchHistoryRepository = searchHistoryRepository;
        }

        private ICourseItemRepository CourseItemRepository { get; }

        private IEduProviderItemRepository EduProviderItemRepository { get; }

        private ITrainingService TrainingService { get; }

        private ISearchHistoryRepository SearchHistoryRepository { get; }

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
            foreach (var result in SearchResults)
            {
                result.OpenCourseItemCommand?.NotifyCanExecuteChanged();
            }
        }

        private bool CanSearch(object queryElement)
        {
            return !IsBusy && queryElement != null;
        }

        private bool CanOpenCourseItem(CourseItemEntry entry)
        {
            return !IsBusy && entry != null;
        }

        private async Task OpenCourseItem(CourseItemEntry entry)
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var courseData = new NavigationParameters();
                    courseData.AddValue<long?>(NavigationParameter.Id, entry.Export().Id);
                    await NavigationService.NavigateAsnc(Routes.CourseView, worker.Token, courseData);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Search)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Search)} in {GetType().Name}.");
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
                    var courseItems = await TrainingService.SearchTrainingsAsync(new Filter() { Fields = new List<FieldExpression>() { new FieldExpression() { FieldName = nameof(Training.TrainingName), Argument = new List<object>() { query ?? string.Empty } } } }, worker.Token);
                    courseItems = courseItems ?? Array.Empty<CourseItem>();
                    var courses = courseItems.Select(item => CourseItemEntry.Import(item, OpenCourseItem, CanOpenCourseItem)).ToList();
                    if (!courses.Any() || string.IsNullOrWhiteSpace(query))
                    {
                        await ExecuteOnUIThreadAsync(() => LoadonUIThread(courses), worker.Token);
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

                    await SearchHistoryRepository.AddOrUpdateItemAsync(history, CancellationToken.None).ConfigureAwait(false);
                    await ExecuteOnUIThreadAsync(
                        () =>
                        {
                            LoadonUIThread(courses);
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

        private void LoadonUIThread(HistoricalSuggestionEntry? historyEntry, SearchSuggestionEntry? suggestionEntry, SearchHistory history)
        {
            var recent = Recents.ToList();
            historyEntry?.Update(history);
            if (historyEntry == null)
            {
                recent.Add(HistoricalSuggestionEntry.Import(history));
            }

            if (suggestionEntry != null)
            {
                Suggestions.Remove(suggestionEntry);
            }

            recent.OrderBy(r => r.Ticks).Take(int.Max(_maxHistoryItemsCount - Suggestions.Count, 0)).ToList();
            Recents = new ObservableCollection<HistoricalSuggestionEntry>(recent);
        }

        private void LoadonUIThread(IEnumerable<CourseItemEntry> courses)
        {
            SearchResults = new ObservableCollection<CourseItemEntry>(courses);
        }

        private async Task LoadSuggestionsAsync(string inputValue, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var trainings = await TrainingService.SearchTrainingsAsync(Filter.CreateQuery(nameof(Training.TrainingName), new List<object>() { inputValue }, QueryOperator.Contains), token).ConfigureAwait(false);
            var recents = await SearchHistoryRepository.GetMaxItemsAsync(_maxHistoryItemsCount, inputValue, token).ConfigureAwait(false);
            if (!(recents?.Any() ?? false))
            {
                recents = await SearchHistoryRepository.GetMaxItemsAsync(_maxHistoryItemsCount, null, token).ConfigureAwait(false);
            }

            recents = recents ?? Array.Empty<SearchHistory>();
            trainings = trainings.Take(_maxSugestionItemsCount) ?? Array.Empty<CourseItem>();
            var courses = trainings.Where(x => !string.IsNullOrWhiteSpace(x.Title)).Select(x => SearchSuggestionEntry.Import(x.Title)).ToList();
            var history = recents.Take(Math.Max(_maxHistoryItemsCount - courses.Count, 0)).Select(x => HistoricalSuggestionEntry.Import(x)).ToList();
            await ExecuteOnUIThreadAsync(
                () =>
                {
                    Suggestions = new ObservableCollection<SearchSuggestionEntry>(courses);
                    Recents = new ObservableCollection<HistoricalSuggestionEntry>(history);
                }, token);
        }

        private void LoadonUIThread(IEnumerable<HistoricalSuggestionEntry> recents)
        {
            Recents = new ObservableCollection<HistoricalSuggestionEntry>(recents);
        }
    }
}
