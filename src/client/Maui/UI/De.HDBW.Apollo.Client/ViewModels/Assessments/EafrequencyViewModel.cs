// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Questions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class EafrequencyViewModel : AbstractQuestionViewModel<Eafrequency, EafrequencyEntry>
    {
        public EafrequencyViewModel(
            IAssessmentService service,
            ILocalAssessmentSessionRepository sessionRepository,
            IRawDataCacheRepository repository,
            IUserSecretsService userSecretsService,
            IAudioPlayerService audioPlayerService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<EafrequencyViewModel> logger)
            : base(service, sessionRepository, repository, userSecretsService, audioPlayerService, dispatcherService, navigationService, dialogService, logger)
        {
        }

        protected override EafrequencyEntry CreateEntry(Eafrequency data)
        {
            return EafrequencyEntry.Import(data, MediaBasePath, Density, ImageSizeConfig[typeof(EafrequencyEntry)]);
        }
    }
}
