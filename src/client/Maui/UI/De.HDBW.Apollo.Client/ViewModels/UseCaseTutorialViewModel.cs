// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Helper;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class UseCaseTutorialViewModel : BaseViewModel
    {
        public UseCaseTutorialViewModel(
           IDispatcherService dispatcherService,
           INavigationService navigationService,
           IDialogService dialogService,
           IUseCaseBuilder builder,
           ILogger<UseCaseTutorialViewModel> logger)
           : base(dispatcherService, navigationService, dialogService, logger)
        {
            UseCaseBuilder = builder;
        }

        private UseCase UseCase { get; set; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            CreateUseCaseCommand?.NotifyCanExecuteChanged();
        }

        private IUseCaseBuilder UseCaseBuilder { get; }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanCreateUseCase))]
        public async Task CreateUseCase(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    if (!await UseCaseBuilder.BuildAsync(UseCase, worker.Token))
                    {
                        return;
                    }

                    await NavigationService.PushToRootAsnc(Routes.Shell, token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(CreateUseCase)} in {GetType()}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(CreateUseCase)} in {GetType()}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(CreateUseCase)} in {GetType()}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        public bool CanCreateUseCase()
        {
            return !IsBusy && UseCase != UseCase.Unknown;
        }
    }
}
