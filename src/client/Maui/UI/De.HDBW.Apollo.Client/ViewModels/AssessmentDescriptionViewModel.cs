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
        private string? _descriptionText;

        [ObservableProperty]
        private string? _descriptionDetails;

        [ObservableProperty]
        private string? _startText;

        [ObservableProperty]
        private string? _duration;

        [ObservableProperty]
        private string? _decoratorText;

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
            ImagePath = "placeholdertest.png";
            DecoratorText = Resources.Strings.Resources.AssessmentItem_DecoratorText;
            OnPropertyChanged(nameof(HasImage));
            DescriptionTitle = assessmentItem.Title;
            DescriptionText = assessmentItem.Description;
            DescriptionDetails = assessmentItem.Disclaimer;
            Duration = string.Format(Resources.Strings.Resources.Global_DurationFormat, !string.IsNullOrWhiteSpace(assessmentItem.Duration) ? assessmentItem.Duration : 0);
            switch (assessmentItem.AssessmentType)
            {
                case AssessmentType.SoftSkillAssessment:
                    StartText = Resources.Strings.Resources.AssessmentDescriptionViewModel_SoftSkill_Start;
                    break;
                case AssessmentType.SkillAssessment:
                    StartText = Resources.Strings.Resources.AssessmentDescriptionViewModel_Skill_Start;
                    break;
                case AssessmentType.Survey:
                    StartText = Resources.Strings.Resources.AssessmentDescriptionViewModel_Survey_Start;
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
                    await NavigationService.NavigateAsync(Routes.AssessmentView, worker.Token, parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Start)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Start)} in {GetType().Name}.");
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
