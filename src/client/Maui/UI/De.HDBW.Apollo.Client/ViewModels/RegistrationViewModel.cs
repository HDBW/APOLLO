namespace De.HDBW.Apollo.Client.ViewModels
{
    using CommunityToolkit.Mvvm.Input;
    using De.HDBW.Apollo.Client.Contracts;
    using De.HDBW.Apollo.Client.Models;
    using Microsoft.Extensions.Logging;
    using Microsoft.Identity.Client;

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
            this.AuthService = authService;
        }

        private IAuthService AuthService { get; set; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            this.SkipCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSkip))]
        private async Task Skip(CancellationToken token)
        {
            this.IsBusy = true;
            try
            {
                await this.NavigationService.PushToRootAsnc(Routes.UseCaseTutorialView, token);
            }
            catch (OperationCanceledException)
            {
                this.Logger?.LogDebug($"Canceled Skip in {this.GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                this.Logger?.LogDebug($"Canceled Skip in {this.GetType()}.");
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error in Skip in {this.GetType()}.");
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    this.IsBusy = false;
                }
            }
        }

        private bool CanSkip()
        {
            return !this.IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRegister))]
        private async Task Register(CancellationToken token)
        {
            this.IsBusy = true;
            try
            {
                var x = await this.AuthService.AcquireTokenSilent(token).ConfigureAwait(false);
                var result = await this.AuthService.SignInInteractively(token).ConfigureAwait(false);

                await this.NavigationService.PushToRootAsnc(Routes.UseCaseTutorialView, token);
            }
            catch (OperationCanceledException)
            {
                this.Logger?.LogDebug($"Canceled Register in {this.GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                this.Logger?.LogDebug($"Canceled Register in {this.GetType()}.");
            }
            catch (MsalException ex)
            {
                this.Logger?.LogWarning(ex, $"Error while registering user in {this.GetType()}.");
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error in Register in {this.GetType()}.");
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    this.IsBusy = false;
                }
            }
        }

        private bool CanRegister()
        {
            return !this.IsBusy;
        }
    }
}
