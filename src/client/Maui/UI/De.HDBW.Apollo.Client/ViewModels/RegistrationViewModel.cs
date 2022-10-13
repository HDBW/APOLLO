using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        public RegistrationViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            IAuthService authService,
            ILogger<RegistrationViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            AuthService = authService;
        }

        private IAuthService AuthService { get; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SkipCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSkip))]
        private async Task Skip(CancellationToken token)
        {
            IsBusy = true;
            try
            {
                await NavigationService.PushToRootAsnc(Routes.UseCaseTutorialView, token);
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled Skip in {GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled Skip in {GetType()}.");
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error in Skip in {GetType()}.");
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    IsBusy = false;
                }
            }
        }

        private bool CanSkip()
        {
            return !IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRegister))]
        private async Task Register(CancellationToken token)
        {
            IsBusy = true;
            try
            {
                var x = await AuthService.AcquireTokenSilent(token);
                if (x != null)
                {
                    await AuthService.LogoutAsync(token);
                }

                var result = await AuthService.SignInInteractively(token);

                await NavigationService.PushToRootAsnc(Routes.UseCaseTutorialView, token);
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled Register in {GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled Register in {GetType()}.");
            }
            catch (MsalException ex)
            {
                Logger?.LogWarning(ex, $"Error while registering user in {GetType()}.");
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error in Register in {GetType()}.");
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    IsBusy = false;
                }
            }
        }

        private bool CanRegister()
        {
            return !IsBusy;
        }
    }
}
