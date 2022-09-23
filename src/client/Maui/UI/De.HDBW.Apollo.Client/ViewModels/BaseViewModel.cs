namespace De.HDBW.Apollo.Client.ViewModels
{
    using System.Collections.Generic;
    using CommunityToolkit.Mvvm.ComponentModel;
    using De.HDBW.Apollo.Client.Contracts;
    using De.HDBW.Apollo.Client.Models;
    using Microsoft.Extensions.Logging;

    public abstract partial class BaseViewModel : ObservableObject, IQueryAttributable
    {
        private bool isBusy;

        public BaseViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger logger)
            : base()
        {
            this.Logger = logger;
            this.NavigationService = navigationService;
            this.DialogService = dialogService;
            this.DispatcherService = dispatcherService;
        }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                if (this.SetProperty(ref this.isBusy, value))
                {
                    this.RefreshCommands();
                }
            }
        }

        protected ILogger Logger { get; set; }

        protected INavigationService NavigationService { get; set; }

        protected IDialogService DialogService { get; set; }

        protected IDispatcherService DispatcherService { get; }

        private Shell Shell
        {
            get { return Shell.Current; }
        }

        public virtual Task OnNavigatedTo() => Task.CompletedTask;

        public virtual Task OnNavigatingFrom() => Task.CompletedTask;

        public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            this.OnPrepare(NavigationParameters.FromQueryDictionary(query));
        }

        protected virtual void OnPrepare(NavigationParameters navigationParameters)
        {
        }

        protected virtual void RefreshCommands()
        {
            this.NavigateToRouteCommand?.NotifyCanExecuteChanged();
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanNavigateToRoute), FlowExceptionsToTaskScheduler =false, IncludeCancelCommand =false)]
        private async Task NavigateToRoute(string route, CancellationToken token)
        {
            this.IsBusy = true;
            try
            {
               await this.NavigationService.NavigateAsnc(route, token);
            }
            catch (OperationCanceledException)
            {
                this.Logger?.LogDebug($"Canceled NavigateToRoute in {this.GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                this.Logger?.LogDebug($"Canceled NavigateToRoute in {this.GetType()}.");
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknoen Error in NavigateToRoute in {this.GetType()}.");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private bool CanNavigateToRoute(string route)
        {
            return !this.IsBusy && !string.IsNullOrWhiteSpace(route);
        }
    }
}
