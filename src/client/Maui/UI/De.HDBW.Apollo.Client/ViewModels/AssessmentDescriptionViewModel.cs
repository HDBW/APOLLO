// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class AssessmentDescriptionViewModel : BaseViewModel
    {
        private long? _assessmentItemId;

        [ObservableProperty]
        private string? _imagePath;

        [ObservableProperty]
        private string? _descriptionTitle;

        [ObservableProperty]
        private string? _descriptionSubline;

        [ObservableProperty]
        private string? _descriptionText;

        [ObservableProperty]
        private string? _descriptionDetailsTitle;

        [ObservableProperty]
        private string? _descriptionDetails;

        public AssessmentDescriptionViewModel(
            IAssessmentItemRepository assessmentItemRepository,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<AssessmentDescriptionViewModel> logger)
            : base(
                dispatcherService,
                navigationService,
                dialogService,
                logger)
        {
            ArgumentNullException.ThrowIfNull(assessmentItemRepository);
            AssessmentItemRepository = assessmentItemRepository;
        }

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath);
            }
        }

        private IAssessmentItemRepository AssessmentItemRepository { get; }

        public async override Task OnNavigatedToAsync()
        {
            if (!_assessmentItemId.HasValue)
            {
                return;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    var assessmentItem = await AssessmentItemRepository.GetItemByIdAsync(_assessmentItemId.Value, worker.Token).ConfigureAwait(false);
                    assessmentItem = assessmentItem ?? new AssessmentItem();
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(assessmentItem), worker.Token);
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
            base.RefreshCommands();
            StartCommand?.NotifyCanExecuteChanged();
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            _assessmentItemId = navigationParameters.GetValue<long?>(NavigationParameter.Id);
        }

        private void LoadonUIThread(AssessmentItem assessmentItem)
        {
            ImagePath = "fallback.png";
            OnPropertyChanged(nameof(HasImage));
            switch (assessmentItem.AssessmentType)
            {
                case AssessmentType.SoftSkillAssessment:
                    DescriptionTitle = Resources.Strings.Resource.AssessmentDescriptionViewModel_SoftSkill_DescriptionTitle;
                    DescriptionSubline = Resources.Strings.Resource.AssessmentDescriptionViewModel_SoftSkill_DescriptionSubline;
                    DescriptionText = Resources.Strings.Resource.AssessmentDescriptionViewModel_SoftSkill_DescriptionText;
                    DescriptionDetailsTitle = Resources.Strings.Resource.AssessmentDescriptionViewModel_SoftSkill_DescriptionDetailsTitle;
                    DescriptionDetails = Resources.Strings.Resource.AssessmentDescriptionViewModel_SoftSkill_DescriptionDetails;
                    break;
                case AssessmentType.SkillAssessment:
                    DescriptionTitle = Resources.Strings.Resource.AssessmentDescriptionViewModel_Skill_DescriptionTitle;
                    DescriptionSubline = Resources.Strings.Resource.AssessmentDescriptionViewModel_Skill_DescriptionSubline;
                    DescriptionText = Resources.Strings.Resource.AssessmentDescriptionViewModel_Skill_DescriptionText;
                    DescriptionDetailsTitle = Resources.Strings.Resource.AssessmentDescriptionViewModel_Skill_DescriptionDetailsTitle;
                    DescriptionDetails = Resources.Strings.Resource.AssessmentDescriptionViewModel_Skill_DescriptionDetails;
                    break;
                case AssessmentType.Survey:
                    DescriptionTitle = Resources.Strings.Resource.AssessmentDescriptionViewModel_Survey_DescriptionTitle;
                    DescriptionSubline = Resources.Strings.Resource.AssessmentDescriptionViewModel_Survey_DescriptionSubline;
                    DescriptionText = Resources.Strings.Resource.AssessmentDescriptionViewModel_Survey_DescriptionText;
                    DescriptionDetailsTitle = Resources.Strings.Resource.AssessmentDescriptionViewModel_Survey_DescriptionDetailsTitle;
                    DescriptionDetails = Resources.Strings.Resource.AssessmentDescriptionViewModel_Survey_DescriptionDetails;
                    break;
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanStart))]
        private async Task Start(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var parameters = new NavigationParameters();
                    parameters.AddValue<long?>(NavigationParameter.Id, _assessmentItemId);
                    await NavigationService.NavigateAsnc(Routes.AssessmentView, worker.Token, parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Start)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Start)} in {GetType().Name}.");
                }
                catch (MsalException ex)
                {
                    Logger?.LogWarning(ex, $"Error while starting assessment in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Start)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanStart()
        {
            return !IsBusy && _assessmentItemId.HasValue;
        }
    }
}
