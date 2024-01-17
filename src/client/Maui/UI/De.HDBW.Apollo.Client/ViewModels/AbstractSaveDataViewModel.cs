// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public abstract partial class AbstractSaveDataViewModel : BaseViewModel, IBackNavigationInterceptor
    {
        private bool _isDirty;

        protected AbstractSaveDataViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public bool IsDirty
        {
            get
            {
                return _isDirty;
            }

            set
            {
                if (SetProperty(ref _isDirty, value))
                {
                    RefreshCommands();
                }
            }
        }

        protected override void RefreshCommands()
        {
            SaveCommand?.NotifyCanExecuteChanged();
            base.RefreshCommands();
        }

        protected abstract Task SaveAsync(CancellationToken token);

        protected virtual bool CanSave()
        {
            return !IsBusy && IsDirty;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSave))]
        private async Task Save(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await SaveAsync(worker.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Save)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Save)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Save)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }
    }
}
