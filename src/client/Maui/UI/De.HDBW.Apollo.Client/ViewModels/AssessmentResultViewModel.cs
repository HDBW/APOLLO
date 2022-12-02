// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Converter;
using De.HDBW.Apollo.Client.Enums;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class AssessmentResultViewModel : BaseViewModel
    {
        [ObservableProperty]
        private double _score;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _interactions = new ObservableCollection<InteractionEntry>();

        private long? _assessmentItemId;

        public AssessmentResultViewModel(
            IAnswerItemResultRepository answerItemResultRepository,
            IAssessmentScoreRepository assessmentScoreRepository,
            IAssessmentScoreService assessmentResultService,
            IAssessmentCategoryResultRepository assessmentCategoryResultRepository,
            ICourseItemRepository courseItemRepository,
            IEduProviderItemRepository eduProviderItemRepository,
            IDispatcherService dispatcherService,
            ISessionService sessionService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<AssessmentResultViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            SessionService = sessionService;
            AnswerItemResultRepository = answerItemResultRepository;
            AssessmentScoreRepository = assessmentScoreRepository;
            AssessmentResultService = assessmentResultService;
            AssessmentCategoryResultRepository = assessmentCategoryResultRepository;
            CourseItemRepository = courseItemRepository;
            EduProviderItemRepository = eduProviderItemRepository;
        }

        public bool HasInteractions
        {
            get
            {
                return Interactions.Any();
            }
        }

        private IAnswerItemResultRepository AnswerItemResultRepository { get; }

        private IAssessmentScoreRepository AssessmentScoreRepository { get; }

        private IAssessmentScoreService AssessmentResultService { get; }

        private IAssessmentCategoryResultRepository AssessmentCategoryResultRepository { get; }

        private ICourseItemRepository CourseItemRepository { get; }

        private IEduProviderItemRepository EduProviderItemRepository { get; }

        private ISessionService SessionService { get; }

        public override async Task OnNavigatedToAsync()
        {
            if (!_assessmentItemId.HasValue)
            {
                return;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    var results = await AnswerItemResultRepository.GetItemsByForeignKeyAsync(_assessmentItemId.Value, worker.Token).ConfigureAwait(false);
                    var score = await AssessmentScoreRepository.GetItemByForeignKeyAsync(_assessmentItemId.Value, worker.Token).ConfigureAwait(false);
                    IEnumerable<AssessmentCategoryResult> categoryResults = new List<AssessmentCategoryResult>();
                    if (score == null)
                    {
                        score = await AssessmentResultService.GetAssessmentScoreAsync(results, worker.Token).ConfigureAwait(false);
                        await AssessmentScoreRepository.AddOrUpdateItemAsync(score, worker.Token).ConfigureAwait(false);
                        categoryResults = await AssessmentCategoryResultRepository.GetItemsByForeignKeyAsync(score.Id, worker.Token).ConfigureAwait(false);
                    }

                    var courseIds = categoryResults.Select(r => r.CategoryId).Distinct().ToList();
                    IEnumerable<CourseItem> courseItems = new List<CourseItem>();
                    var eduProviders = await EduProviderItemRepository.GetItemsAsync(worker.Token).ConfigureAwait(false);
                    if (courseIds.Any())
                    {
                        courseItems = await CourseItemRepository.GetItemsByIdsAsync(courseIds, worker.Token).ConfigureAwait(false);
                    }

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(score, courseItems, eduProviders), worker.Token);
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
            _assessmentItemId = navigationParameters.GetValue<long?>(NavigationParameter.Id);
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            ConfirmCommand?.NotifyCanExecuteChanged();
            foreach (var interaction in Interactions.OfType<StartViewInteractionEntry>())
            {
                interaction.NavigateCommand?.NotifyCanExecuteChanged();
                interaction.ToggleIsFavoriteCommand?.NotifyCanExecuteChanged();
            }
        }

        private void LoadonUIThread(AssessmentScore score, IEnumerable<CourseItem> courseItems, IEnumerable<EduProviderItem> eduProviderItems)
        {
            Score = (double)score.PercentageScore / 100d;
            var converter = new CourseTagTypeToStringConverter();
            var interactions = new List<InteractionEntry>();
            foreach (var course in courseItems)
            {
                course.UnPublishingDate = null;
                var decoratorText = converter.Convert(course, typeof(string), null, CultureInfo.CurrentUICulture)?.ToString() ?? string.Empty;

                var courseData = new NavigationParameters();
                courseData.AddValue<long?>(NavigationParameter.Id, course.Id);
                var data = new NavigationData(Routes.CourseView, courseData);

                var eduProvider = eduProviderItems?.FirstOrDefault(p => p.Id == course.TrainingProviderId);

                var duration = course.Duration ?? string.Empty;
                var provider = !string.IsNullOrWhiteSpace(eduProvider?.Name) ? eduProvider.Name : Resources.Strings.Resource.StartViewModel_UnknownProvider;
                var image = "placeholdercontinuingeducation.png";
                switch (course.CourseTagType)
                {
                    case CourseTagType.InfoEvent:
                        image = "placeholderinfoevent.png";
                        break;
                }

                var interaction = StartViewInteractionEntry.Import<CourseItem>(course.Title, provider, decoratorText, duration, image, Status.Unknown, course.Id, data, HandleToggleIsFavorite, CanHandleToggleIsFavorite, HandleInteract, CanHandleInteract);
                interactions.Add(interaction);
                Interactions = new ObservableCollection<InteractionEntry>(interactions);
                OnPropertyChanged(nameof(HasInteractions));
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanConfirm))]
        private async Task Confirm(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.PushToRootAsnc(worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Confirm)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Confirm)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Confirm)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanConfirm()
        {
            return !IsBusy;
        }

        private bool CanHandleInteract(InteractionEntry arg)
        {
            return false;
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

        private bool CanHandleToggleIsFavorite(StartViewInteractionEntry entry)
        {
            return !IsBusy;
        }

        private Task HandleToggleIsFavorite(StartViewInteractionEntry entry)
        {
            if (entry.IsFavorite)
            {
                SessionService.RemoveFavorite(entry.EntityId, entry.EntityType);
            }
            else
            {
                SessionService.AddFavorite(entry.EntityId, entry.EntityType);
            }
            return Task.CompletedTask;
        }
    }
}
