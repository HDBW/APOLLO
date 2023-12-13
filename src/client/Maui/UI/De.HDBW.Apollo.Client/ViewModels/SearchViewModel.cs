// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using Apollo.Common.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Course;
using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class SearchViewModel : BaseViewModel, ILoadSuggestionsProvider
    {
        [ObservableProperty]
        private ObservableCollection<SearchSuggestionEntry> _suggestions = new ObservableCollection<SearchSuggestionEntry>();

        [ObservableProperty]
        private ObservableCollection<SearchSuggestionEntry> _recents = new ObservableCollection<SearchSuggestionEntry>();

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

        public async void LoadSuggestionsAsync(string inputValue)
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
                    var history = await SearchHistoryRepository.GetItemsAsync(worker.Token).ConfigureAwait(false);
                    if (!(history?.Any() ?? false))
                    {
                        return;
                    }

                    var recents = history.Where(h => !string.IsNullOrWhiteSpace(h.Query)).Select(h => SearchSuggestionEntry.Import(h.Query!, true));
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

        private bool CanSearch(string query)
        {
            return true;
        }

        private bool CanOpenCourseItem(CourseItemEntry entry)
        {
            return !IsBusy && entry != null;
        }

        private async Task OpenCourseItem(CourseItemEntry entry)
        {
            using (var worker = ScheduleWork())
            {
                var courseData = new NavigationParameters();
                courseData.AddValue<long?>(NavigationParameter.Id, entry.Export().Id);
                await NavigationService.NavigateAsnc(Routes.CourseView, worker.Token, courseData);
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearch))]
        private async Task Search(string query, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    var courseItems = await TrainingService.SearchTrainingsAsync(new Filter() { Fields = new List<FieldExpression>() { new FieldExpression() { FieldName = nameof(Training.TrainingName), Argument = new List<object>() { query } } } }, token);
                    SearchResults = new ObservableCollection<CourseItemEntry>(courseItems.Select(item => CourseItemEntry.Import(item, OpenCourseItem, CanOpenCourseItem)));
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

        private async Task LoadSuggestionsAsync(string inputValue, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var trainings = await TrainingService.SearchTrainingsAsync(Filter.CreateQuery(nameof(Training.TrainingName), new List<object>() { inputValue }, QueryOperator.Contains), token).ConfigureAwait(false);
            trainings = trainings ?? Array.Empty<CourseItem>();
            trainings = trainings.Where(x => !string.IsNullOrWhiteSpace(x.Title)).ToList();
            await DispatcherService.BeginInvokeOnMainThreadAsync(
                () =>
                {
                    var res = new List<SearchSuggestionEntry>();
                    foreach (var item in trainings)
                    {
                        res.Add(SearchSuggestionEntry.Import(item.Title));
                    }

                    Suggestions = new ObservableCollection<SearchSuggestionEntry>(res);
                }, token);
        }

        private void LoadonUIThread(IEnumerable<SearchSuggestionEntry> recents)
        {
            Recents = new ObservableCollection<SearchSuggestionEntry>(recents);
        }
    }
}
