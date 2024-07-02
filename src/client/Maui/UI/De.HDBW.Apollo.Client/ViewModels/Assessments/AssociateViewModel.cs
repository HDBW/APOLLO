// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Questions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class AssociateViewModel : AbstractQuestionViewModel<Associate, AssociateEntry>
    {
        public AssociateViewModel(
            IAssessmentService service,
            ILocalAssessmentSessionRepository sessionRepository,
            IRawDataCacheRepository repository,
            IUserSecretsService userSecretsService,
            IAudioPlayerService audioPlayerService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<AssociateViewModel> logger)
            : base(service, sessionRepository, repository, userSecretsService, audioPlayerService, dispatcherService, navigationService, dialogService, logger)
        {
        }

        protected override AssociateEntry CreateEntry(Associate data)
        {
            return AssociateEntry.Import(data, MediaBasePath, Density, ImageSizeConfig[typeof(AssociateEntry)]);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanReset))]
        private Task Reset(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (Question == null)
                {
                    return Task.CompletedTask;
                }

                foreach (var item in Question.TargetImages)
                {
                    item.AssociatedIndex = null;
                }
            }
            catch (OperationCanceledException ex)
            {
                Logger?.LogInformation(ex, $"Canceled {nameof(Reset)} from {GetType().Name}.");
                throw;
            }
            catch (ObjectDisposedException ex)
            {
                Logger?.LogInformation(ex, $"Canceled {nameof(Reset)} from {GetType().Name}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error in {nameof(Reset)} from {GetType().Name}.");
            }

            return Task.CompletedTask;
        }

        private bool CanReset()
        {
            return true;
        }
    }
}
