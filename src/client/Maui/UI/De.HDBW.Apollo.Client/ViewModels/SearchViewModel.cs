// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using Apollo.Common.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Course;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class SearchViewModel : BaseViewModel, ILoadSuggestionsProvider
    {
        [ObservableProperty]
        private ObservableCollection<SearchSuggestion> _suggestions = new ObservableCollection<SearchSuggestion>();

        [ObservableProperty]
        private ObservableCollection<SearchSuggestion> _recents = new ObservableCollection<SearchSuggestion>();

        [ObservableProperty]
        private ObservableCollection<CourseItemEntry> _searchResults = new ObservableCollection<CourseItemEntry>();

        public SearchViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ICourseItemRepository courseItemRepository,
            IEduProviderItemRepository eduProviderItemRepository,
            ITrainingService trainingService,
            ILogger<RegistrationViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(courseItemRepository);
            CourseItemRepository = courseItemRepository;
            EduProviderItemRepository = eduProviderItemRepository;
            TrainingService = trainingService;
        }

        private ICourseItemRepository CourseItemRepository { get; }

        private IEduProviderItemRepository EduProviderItemRepository { get; }

        private ITrainingService TrainingService { get; }

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

        private bool CanSearch(string query)
        {
            return true;
        }

        private bool CanOpenCourseItem(CourseItemEntry entry)
        {
            return !IsBusy;
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
            await Task.Delay(700).ConfigureAwait(false);

            var trainings = await TrainingService.SearchTrainingsAsync(Filter.CreateQuery(nameof(Training.TrainingName), new List<object>() { inputValue }, QueryOperator.Contains), token);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                var res = new List<SearchSuggestion>();
                foreach (var item in trainings)
                {
                    res.Add(new SearchSuggestion() { Name = $"{item?.Title}" });
                }

                Suggestions = new ObservableCollection<SearchSuggestion>(res);
            });
        }
    }
}
