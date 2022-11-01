// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Enums;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class StartViewModel : BaseViewModel
    {
        private readonly ObservableCollection<InteractionCategoryEntry> _interactionsCategories = new ObservableCollection<InteractionCategoryEntry>();

        [ObservableProperty]
        private UserProfileEntry? _userProfile = UserProfileEntry.Import(new UserProfile());

        public StartViewModel(
            IPreferenceService preferenceService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ICourseItemRepository courseItemRepository,
            IAssessmentItemRepository assessmentItemRepository,
            IUserProfileRepository userProfileRepository,
            IEduProviderItemRepository eduProviderItemRepository,
            ILogger<StartViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            PreferenceService = preferenceService;
            AssessmentItemRepository = assessmentItemRepository;
            CourseItemRepository = courseItemRepository;
            UserProfileRepository = userProfileRepository;
            EduProviderItemRepository = eduProviderItemRepository;
        }

        public ObservableCollection<InteractionCategoryEntry> InteractionCategories
        {
            get
            {
                return _interactionsCategories;
            }
        }

        private IPreferenceService PreferenceService { get; }

        private IAssessmentItemRepository AssessmentItemRepository { get; }

        private ICourseItemRepository CourseItemRepository { get; }

        private IUserProfileRepository UserProfileRepository { get; set; }

        private IEduProviderItemRepository EduProviderItemRepository { get; set; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            OpenSettingsCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenSettings), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private async Task OpenSettings(CancellationToken token)
        {
            using (var work = ScheduleWork(token))
            {
                try
                {
                    var parameters = new NavigationParameters();
                    parameters.AddValue(NavigationParameter.Id, 0);
                    parameters.AddValue(NavigationParameter.Unknown, "Test");
                    await NavigationService.NavigateAsnc(Routes.EmptyView, token, parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenSettings)} in {GetType()}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenSettings)} in {GetType()}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenSettings)} in {GetType()}.");
                }
                finally
                {
                    UnscheduleWork(work);
                }
            }
        }

        private bool CanOpenSettings()
        {
            return !IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanChangeUseCase), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private async Task ChangeUseCase(CancellationToken token)
        {
            using (var work = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.ResetNavigationAsnc(Routes.UseCaseSelectionView, token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ChangeUseCase)} in {GetType()}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ChangeUseCase)} in {GetType()}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(ChangeUseCase)} in {GetType()}.");
                }
                finally
                {
                    UnscheduleWork(work);
                }
            }
        }

        private bool CanChangeUseCase()
        {
            return !IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = true)]
        private async Task LoadData(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var assesments = await AssessmentItemRepository.GetItemsAsync(worker.Token).ConfigureAwait(false);
                    var courses = await CourseItemRepository.GetItemsAsync(worker.Token).ConfigureAwait(false);
                    var userProfile = await UserProfileRepository.GetItemByIdAsync(1, worker.Token).ConfigureAwait(false);
                    var eduProviders = await EduProviderItemRepository.GetItemsAsync(worker.Token).ConfigureAwait(false);

                    await ExecuteOnUIThreadAsync(
                           () => LoadonUIThread(
                           assesments,
                           courses,
                           userProfile,
                           eduProviders), worker.Token);

                    var taskList = new List<Task>();
                    Task<NavigationParameters?>? dialogTask = null;
                    var isFirstTime = PreferenceService.GetValue(Preference.IsFirstTime, true);
                    if (isFirstTime)
                    {
                        PreferenceService.SetValue(Preference.IsFirstTime, false);
                        dialogTask = DialogService.ShowPopupAsync<FirstTimeDialog, NavigationParameters>(token);
                        taskList.Add(dialogTask);
                    }

                    if (taskList.Any())
                    {
                        await Task.WhenAll(taskList).ConfigureAwait(false);
                    }

                    var selection = dialogTask?.Result?.GetValue<bool>(NavigationParameter.Result) ?? false;
                    if (selection)
                    {
                        await NavigationService.NavigateAsnc(Routes.TutorialView, token);
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(LoadData)} in {GetType()}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(LoadData)} in {GetType()}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(LoadData)} in {GetType()}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private void LoadonUIThread(
           IEnumerable<AssessmentItem> assessmentItems,
           IEnumerable<CourseItem> courseItems,
           UserProfile? userProfile,
           IEnumerable<EduProviderItem> eduProviderItems)
        {
            UserProfile = userProfile != null ? UserProfileEntry.Import(userProfile) : null;
            var interactions = new List<InteractionEntry>();
            foreach (var assesment in assessmentItems)
            {
                var assemsmentData = new NavigationParameters();
                assemsmentData.AddValue<long?>(NavigationParameter.Id, assesment.Id);
                var data = new NavigationData(Routes.AssessmentView, assemsmentData);
                var duration = assesment.Duration !=  TimeSpan.Zero ? string.Format("g", assesment.Duration) : string.Empty;
                var provider = !string.IsNullOrWhiteSpace(assesment.Publisher) ? assesment.Publisher : Resources.Strings.Resource.StartViewModel_UnknownProvider;
                interactions.Add(StartViewInteractionEntry.Import<AssessmentItem>(assesment.Title, provider, Resources.Strings.Resource.AssessmentItem_DecoratorText, duration, "fallback.png", Status.Unknown, data, HandleInteract, CanHandleInteract));
            }

            InteractionCategories.Add(InteractionCategoryEntry.Import(Resources.Strings.Resource.StartViewModel_TestHeadline, Resources.Strings.Resource.StartViewModel_TestSubline, interactions, null, HandleShowMore, CanHandleShowMore));

            interactions = new List<InteractionEntry>();
            foreach (var course in courseItems)
            {
                var assemsmentData = new NavigationParameters();
                assemsmentData.AddValue<long?>(NavigationParameter.Id, course.Id);
                var data = new NavigationData(Routes.CourseView, assemsmentData);

                var eduProvider = eduProviderItems?.FirstOrDefault(p => p.Id == course.TrainingProviderId);

                var duration = course.Duration != TimeSpan.Zero ? string.Format("g", course.Duration) : string.Empty;
                var provider = !string.IsNullOrWhiteSpace(eduProvider?.Name) ? eduProvider.Name : Resources.Strings.Resource.StartViewModel_UnknownProvider;
                var decoratorText = string.Empty;
                switch (course.CourseTagType)
                {
                    case CourseTagType.AttendeeCertificate:
                        decoratorText = Resources.Strings.Resource.CourseTagType_AttendeeCertificate;
                        break;
                    case CourseTagType.Admission:
                        decoratorText = Resources.Strings.Resource.CourseTagType_Admission;
                        break;
                    case CourseTagType.Certificate:
                        decoratorText = Resources.Strings.Resource.CourseTagType_Certificate;
                        break;
                    case CourseTagType.Course:
                        decoratorText = Resources.Strings.Resource.CourseTagType_Course;
                        break;
                    case CourseTagType.InfoEvent:
                        decoratorText = Resources.Strings.Resource.CourseTagType_InfoEvent;
                        break;
                    case CourseTagType.PartialQualification:
                        decoratorText = Resources.Strings.Resource.CourseTagType_PartialQualification;
                        break;
                    case CourseTagType.Qualification:
                        decoratorText = Resources.Strings.Resource.CourseTagType_Qualification;
                        break;
                }

                interactions.Add(StartViewInteractionEntry.Import<CourseItem>(course.Title, provider, decoratorText, duration, "fallback.png", Status.Unknown, data, HandleInteract, CanHandleInteract));
            }

            InteractionCategories.Add(InteractionCategoryEntry.Import(Resources.Strings.Resource.StartViewModel_LearningHeadline, Resources.Strings.Resource.StartViewModel_LearningSubline, interactions, null, HandleShowMore, CanHandleShowMore));
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
                    Logger.LogWarning($"Unknown interaction data {interaction?.Data ?? "null"} while {nameof(HandleInteract)} in {GetType()}.");
                    break;
            }
        }

        private Task HandleShowMore(InteractionCategoryEntry arg)
        {
            return Task.CompletedTask;
        }

        private bool CanHandleShowMore(InteractionCategoryEntry arg)
        {
            return true;
        }
    }
}
