// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public abstract partial class BaseViewModel : ObservableObject, IQueryAttributable
    {
        private bool _isBusy;

        public BaseViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger logger)
            : base()
        {
            Logger = logger;
            NavigationService = navigationService;
            DialogService = dialogService;
            DispatcherService = dispatcherService;
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    RefreshCommands();
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
            OnPrepare(NavigationParameters.FromQueryDictionary(query));
        }

        protected virtual void OnPrepare(NavigationParameters navigationParameters)
        {
        }

        protected virtual void RefreshCommands()
        {
            NavigateToRouteCommand?.NotifyCanExecuteChanged();
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanNavigateToRoute), FlowExceptionsToTaskScheduler =false, IncludeCancelCommand =false)]
        private async Task NavigateToRoute(string route, CancellationToken token)
        {
            IsBusy = true;
            try
            {
               await NavigationService.NavigateAsnc(route, token);
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled NavigateToRoute in {GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled NavigateToRoute in {GetType()}.");
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknoen Error in NavigateToRoute in {GetType()}.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanNavigateToRoute(string route)
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(route);
        }
    }
}
