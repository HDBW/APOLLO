// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public abstract partial class BaseViewModel : ObservableObject, IQueryAttributable
    {
        public BaseViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger logger)
            : base()
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(navigationService);
            ArgumentNullException.ThrowIfNull(dialogService);
            ArgumentNullException.ThrowIfNull(dispatcherService);
            Logger = logger;
            NavigationService = navigationService;
            DialogService = dialogService;
            DispatcherService = dispatcherService;
        }

        public bool IsBusy
        {
            get
            {
                return Workers.Any();
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

        private Dictionary<string, CancellationTokenSource> Workers { get; } = new Dictionary<string, CancellationTokenSource>();

        public virtual Task OnNavigatedToAsync() => Task.CompletedTask;

        public virtual Task OnNavigatingFromAsync(bool isForwardNavigation) => Task.CompletedTask;

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

        protected CancellationTokenSource ScheduleWork(CancellationToken? token = null, [CallerMemberName] string? workerName = null)
        {
            if (string.IsNullOrWhiteSpace(workerName))
            {
                workerName = GetType().Name;
            }

            CancellationTokenSource scope;
            if (Workers.ContainsKey(workerName))
            {
                scope = Workers[workerName];
                Workers.Remove(workerName);
                scope.Cancel();
                scope.Dispose();
            }

            if (token == null)
            {
                scope = new CancellationTokenSource();
            }
            else
            {
                scope = CancellationTokenSource.CreateLinkedTokenSource(token.Value);
            }

            Workers.Add(workerName, scope);
            DispatcherService.BeginInvokeOnMainThread(SignalWorkerChanged);
            return scope;
        }

        protected void UnscheduleWork(CancellationTokenSource worker)
        {
            var kv = Workers.FirstOrDefault(w => w.Value == worker);
            if (string.IsNullOrWhiteSpace(kv.Key))
            {
                return;
            }

            Workers.Remove(kv.Key);
            worker.Dispose();
            DispatcherService.BeginInvokeOnMainThread(SignalWorkerChanged);
        }

        protected Task ExecuteOnUIThreadAsync(Action methodeToExecute, CancellationToken token)
        {
            return DispatcherService.SafeExecuteOnMainThreadAsync(methodeToExecute, Logger, token);
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanNavigateToRoute), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private async Task NavigateToRoute(string route, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    switch (route)
                    {
                        case Routes.FeedbackView:
                            await Browser.Default.OpenAsync(Resources.Strings.Resource.FeedbackUrl, BrowserLaunchMode.SystemPreferred);
                            break;
                        default:
                            await NavigationService.NavigateAsnc(route, worker.Token);
                            break;
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NavigateToRoute)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NavigateToRoute)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(NavigateToRoute)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanNavigateToRoute(string route)
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(route);
        }

        private void SignalWorkerChanged()
        {
            OnPropertyChanged(nameof(IsBusy));
            RefreshCommands();
        }
    }
}
