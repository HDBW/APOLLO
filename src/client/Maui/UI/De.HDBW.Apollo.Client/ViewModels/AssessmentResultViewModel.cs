// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class AssessmentResultViewModel : BaseViewModel
    {
        [ObservableProperty]
        private double _score;

        private long? _assessmentItemId;

        public AssessmentResultViewModel(
            IAnswerItemResultRepository answerItemResultRepository,
            IAssessmentScoreRepository assessmentScoreRepository,
            IAssessmentScoreService assessmentResultService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<AssessmentResultViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            AnswerItemResultRepository = answerItemResultRepository;
            AssessmentScoreRepository = assessmentScoreRepository;
            AssessmentResultService = assessmentResultService;
        }

        private IAnswerItemResultRepository AnswerItemResultRepository { get; }

        private IAssessmentScoreRepository AssessmentScoreRepository { get; }

        private IAssessmentScoreService AssessmentResultService { get; }

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
                    if (score == null)
                    {
                        score = await AssessmentResultService.GetAssessmentScoreAsync(results, worker.Token).ConfigureAwait(false);
                        await AssessmentScoreRepository.AddOrUpdateItemAsync(score, worker.Token).ConfigureAwait(false);
                    }

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(score), worker.Token);
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
        }

        private void LoadonUIThread(AssessmentScore score)
        {
            Score = (double)score.PercentageScore;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSkip))]
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

        private bool CanSkip()
        {
            return !IsBusy;
        }
    }
}
