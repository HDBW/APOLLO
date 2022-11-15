﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
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

        private void LoadonUIThread(AssessmentScore score)
        {
            Score = (double)score.PercentageScore;
        }
    }
}
