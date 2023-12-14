// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
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
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;
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
        private ObservableCollection<StartViewInteractionEntry> _searchResults = new ObservableCollection<StartViewInteractionEntry>();

        public SearchViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ISessionService sessionService,
            ICourseItemRepository courseItemRepository,
            IEduProviderItemRepository eduProviderItemRepository,
            ITrainingService trainingService,
            ISearchHistoryRepository searchHistoryRepository,
            ILogger<RegistrationViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(sessionService);
            ArgumentNullException.ThrowIfNull(courseItemRepository);
            ArgumentNullException.ThrowIfNull(eduProviderItemRepository);
            ArgumentNullException.ThrowIfNull(trainingService);
            ArgumentNullException.ThrowIfNull(searchHistoryRepository);
            SessionService = sessionService;
            CourseItemRepository = courseItemRepository;
            EduProviderItemRepository = eduProviderItemRepository;
            TrainingService = trainingService;
            SearchHistoryRepository = searchHistoryRepository;
        }

        private ISessionService SessionService { get; }

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
                    await NavigationService.NavigateAsnc(navigationData.Route, CancellationToken.None, navigationData.Parameters);
                    break;
                default:
                    Logger.LogWarning($"Unknown interaction data {interaction?.Data ?? "null"} while {nameof(HandleInteract)} in {GetType().Name}.");
                    break;
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

                    var converter = new CourseTagTypeToStringConverter();
                    var courseItems = await TrainingService.SearchTrainingsAsync(new Filter() { Fields = new List<FieldExpression>() { new FieldExpression() { FieldName = nameof(Training.TrainingName), Argument = new List<object>() { query ?? string.Empty } } } }, worker.Token);
                    courseItems = courseItems ?? Array.Empty<CourseItem>();
                    var interactions = new List<StartViewInteractionEntry>();
                    foreach (var course in courseItems)
                    {
                        var decoratorText = converter.Convert(course, typeof(string), null, CultureInfo.CurrentUICulture)?.ToString() ?? string.Empty;
                        var courseData = new NavigationParameters();
                        courseData.AddValue<long?>(NavigationParameter.Id, course.Id);
                        var data = new NavigationData(Routes.CourseView, courseData);

                        EduProviderItem eduProvider = null; //eduProviderItems?.FirstOrDefault(p => p.Id == course.CourseProviderId);

                        var duration = course.Duration ?? string.Empty;
                        var provider = !string.IsNullOrWhiteSpace(eduProvider?.Name) ? eduProvider.Name : Resources.Strings.Resources.StartViewModel_UnknownProvider;
                        var image = "placeholdercontinuingeducation.png";
                        switch (course.CourseTagType)
                        {
                            case CourseTagType.InfoEvent:
                                image = "placeholderinfoevent.png";
                                break;
                        }

                        var interaction = StartViewInteractionEntry.Import<CourseItem>(course.Title, provider, decoratorText, duration, image, Status.Unknown, course.Id, data, null, null, HandleInteract, CanHandleInteract);
                        interaction.IsFavorite = SessionService.GetFavorites().Any(f => f.Id == course.Id && f.Type == typeof(CourseItem));
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

                    await SearchHistoryRepository.AddOrUpdateItemAsync(history, CancellationToken.None).ConfigureAwait(false);
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

        private void LoadonUIThread(IEnumerable<StartViewInteractionEntry> interactionEntries)
        {
            SearchResults = new ObservableCollection<StartViewInteractionEntry>(interactionEntries);
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
