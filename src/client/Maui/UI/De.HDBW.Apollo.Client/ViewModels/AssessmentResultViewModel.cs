// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class AssessmentResultViewModel : BaseViewModel
    {
        private long? _assessmentItemId;

        public AssessmentResultViewModel(
            IAnswerItemResultRepository answerItemResultRepository,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<AssessmentResultViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            AnswerItemResultRepository = answerItemResultRepository;
        }

        private IAnswerItemResultRepository AnswerItemResultRepository { get; }

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
                    var results = AnswerItemResultRepository.GetItemsByForeignKeyAsync(_assessmentItemId.Value, worker.Token).ConfigureAwait(false);
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
    }
}
