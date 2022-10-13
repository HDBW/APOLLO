// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Maui.Views;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.ViewModels;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Services
{
    public class DialogService : IDialogService
    {
        private readonly Dictionary<WeakReference<Popup>, BaseViewModel> _dialogLookup = new Dictionary<WeakReference<Popup>, BaseViewModel>();

        public DialogService(IDispatcherService dispatcherService, ILogger<DialogService> logger, IServiceProvider serviceProvider)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
            DispatcherService = dispatcherService;
        }

        private ILogger Logger { get; }

        private IServiceProvider ServiceProvider { get; }

        private IDispatcherService DispatcherService { get; }

        public async Task<TV?> ShowPopupAsync<TU, TV>(CancellationToken token)
            where TU : Popup
            where TV : NavigationParameters
        {
            token.ThrowIfCancellationRequested();
            Popup? popup = null;
            try
            {
                popup = ServiceProvider.GetService<TU>();
                if (popup == null || Shell.Current?.CurrentPage == null)
                {
                    throw new NotSupportedException();
                }

                RegisterDialog(popup);
                var result = await DispatcherService.ExecuteOnMainThreadAsync(() => Shell.Current.CurrentPage.ShowPopupAsync(popup), token);
                return result as TV;
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled ShowPopupAsync in {GetType()}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled ShowPopupAsync in {GetType()}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error while ShowPopupAsync in {GetType()}.");
            }
            finally
            {
                if (popup != null)
                {
                    UnregisterDialog(popup);
                }
            }

            return null;
        }

        public Task ShowPopupAsync<TU>(CancellationToken token)
            where TU : Popup
        {
            token.ThrowIfCancellationRequested();
            try
            {
                var popup = ServiceProvider.GetService<TU>();
                if (Shell.Current?.CurrentPage == null || popup == null)
                {
                    throw new NotSupportedException();
                }

                return DispatcherService.ExecuteOnMainThreadAsync(() => Shell.Current.CurrentPage.ShowPopupAsync(popup), token);
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled ShowPopupAsync in {GetType()}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled ShowPopupAsync in {GetType()}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error while ShowPopupAsync in {GetType()}.");
            }

            return Task.CompletedTask;
        }

        public bool ClosePopup<TV>(BaseViewModel viewModel, TV result)
            where TV : NavigationParameters
        {
            try
            {
                var itemToClose = _dialogLookup.FirstOrDefault(k => k.Value == viewModel);
                if (!itemToClose.Key.TryGetTarget(out Popup? popup))
                {
                    return false;
                }

                popup.Close(result);
                return true;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error while ClosePopup in {GetType()}.");
            }

            return false;
        }

        public bool ClosePopup(BaseViewModel viewModel)
        {
            try
            {
                var itemToClose = _dialogLookup.FirstOrDefault(k => k.Value == viewModel);
                if (!itemToClose.Key.TryGetTarget(out Popup? popup))
                {
                    return false;
                }

                popup.Close();
                return true;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error while ClosePopup in {GetType()}.");
            }

            return false;
        }

        private bool UnregisterDialog(Popup popup)
        {
            if (popup == null)
            {
                return false;
            }

            CleanupLookups();
            var exitingItem = _dialogLookup.FirstOrDefault(k => k.Key.TryGetTarget(out Popup? p) && p == popup);
            if (exitingItem.Key == null)
            {
                return false;
            }

            _dialogLookup.Remove(exitingItem.Key);
            return true;
        }

        private void CleanupLookups()
        {
            var disposedItems = _dialogLookup.Where(k => !k.Key.TryGetTarget(out _)).ToList();
            foreach (var disposedItem in disposedItems)
            {
                _dialogLookup.Remove(disposedItem.Key);
            }
        }

        private bool RegisterDialog(Popup popup)
        {
            if (popup == null)
            {
                return false;
            }

            CleanupLookups();
            var dialog = popup;
            var viewModel = dialog.BindingContext as BaseViewModel;
            if (viewModel == null)
            {
                return false;
            }

            _dialogLookup.Add(new WeakReference<Popup>(dialog), viewModel);
            return true;
        }
    }
}
