﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Questions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class SortViewModel : AbstractQuestionViewModel<Sort, SortEntry>
    {
        public SortViewModel(
            IAssessmentService service,
            IRawDataCacheRepository repository,
            IUserSecretsService userSecretsService,
            IAudioPlayerService audioPlayerService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<SortViewModel> logger)
            : base(service, repository, userSecretsService, audioPlayerService, dispatcherService, navigationService, dialogService, logger)
        {
        }

        protected override SortEntry CreateEntry(Sort data)
        {
            return SortEntry.Import(data);
        }
    }
}
