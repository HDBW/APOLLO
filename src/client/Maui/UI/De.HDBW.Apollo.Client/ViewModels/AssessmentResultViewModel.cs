// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Microsoft.Extensions.Logging;

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
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            _assessmentItemId = navigationParameters.GetValue<long?>(NavigationParameter.Id);
        }
    }
}
