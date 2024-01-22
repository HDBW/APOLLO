// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
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

        public bool NeedsToCancel
        {
            get
            {
                return IsDirty;
            }
        }

        public async void CanceledNavigation()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    if (HasErrors)
                    {
                        var parameters = new NavigationParameters();
                        parameters.AddValue(NavigationParameter.Data, Resources.Strings.Resources.GlobalError_InvalidData);
                        var result = await DialogService.ShowPopupAsync<RetryDialog, NavigationParameters, NavigationParameters>(parameters, worker.Token).ConfigureAwait(false);
                        if (result?.GetValue<bool?>(NavigationParameter.Result) ?? false)
                        {
                            return;
                        }

                        _isDirty = false;
                        await NavigationService.PopAsync(worker.Token).ConfigureAwait(false);
                        return;
                    }

                    var savedData = false;
                    while (!savedData)
                    {
                        savedData = await SaveAsync(worker.Token).ConfigureAwait(false);
                        if (!savedData)
                        {
                            var parameters = new NavigationParameters();
                            parameters.AddValue(NavigationParameter.Data, Resources.Strings.Resources.GlobalError_RetryUnableToSaveData);
                            var result = await DialogService.ShowPopupAsync<RetryDialog, NavigationParameters, NavigationParameters>(parameters, worker.Token).ConfigureAwait(false);
                            savedData = !(result?.GetValue<bool?>(NavigationParameter.Result) ?? false);
                        }
                    }

                    _isDirty = false;
                    await NavigationService.PopAsync(worker.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(CanceledNavigation)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(CanceledNavigation)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(CanceledNavigation)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        protected override void RefreshCommands()
        {
            SaveCommand?.NotifyCanExecuteChanged();
            base.RefreshCommands();
        }

        protected abstract Task<bool> SaveAsync(CancellationToken token);

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
                    if (HasErrors)
                    {
                        return;
                    }

                    if (!await SaveAsync(worker.Token).ConfigureAwait(false))
                    {
                        var parameters = new NavigationParameters();
                        parameters.AddValue(NavigationParameter.Data, Resources.Strings.Resources.GlobalError_UnableToSaveData);
                        await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, token).ConfigureAwait(false);
                    }
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
