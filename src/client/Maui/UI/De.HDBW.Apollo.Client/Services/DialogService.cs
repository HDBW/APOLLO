namespace De.HDBW.Apollo.Client.Services
{
    using CommunityToolkit.Maui.Views;
    using De.HDBW.Apollo.Client.Contracts;
    using De.HDBW.Apollo.Client.Models;
    using De.HDBW.Apollo.Client.ViewModels;
    using Microsoft.Extensions.Logging;

    public class DialogService : IDialogService
    {
        private readonly Dictionary<WeakReference<Popup>, BaseViewModel> dialogLookup = new Dictionary<WeakReference<Popup>, BaseViewModel>();

        public DialogService(IDispatcherService dispatcherService, ILogger<DialogService> logger, IServiceProvider serviceProvider)
        {
            this.Logger = logger;
            this.ServiceProvider = serviceProvider;
            this.DispatcherService = dispatcherService;
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
                popup = this.ServiceProvider.GetService<TU>();
                if (popup == null || Shell.Current?.CurrentPage == null)
                {
                    throw new NotSupportedException();
                }

                this.RegisterDialog(popup);
                var result = await this.DispatcherService.ExecuteOnMainThreadAsync(() => Shell.Current.CurrentPage.ShowPopupAsync(popup), token);
                return result as TV;
            }
            catch (OperationCanceledException)
            {
                this.Logger?.LogDebug($"Canceled ShowPopupAsync in {this.GetType()}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                this.Logger?.LogDebug($"Canceled ShowPopupAsync in {this.GetType()}.");
                throw;
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error while ShowPopupAsync in {this.GetType()}.");
            }
            finally
            {
                if (popup != null)
                {
                    this.UnregisterDialog(popup);
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
                var popup = this.ServiceProvider.GetService<TU>();
                if (Shell.Current?.CurrentPage == null || popup == null)
                {
                    throw new NotSupportedException();
                }

                return this.DispatcherService.ExecuteOnMainThreadAsync(() => Shell.Current.CurrentPage.ShowPopupAsync(popup), token);
            }
            catch (OperationCanceledException)
            {
                this.Logger?.LogDebug($"Canceled ShowPopupAsync in {this.GetType()}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                this.Logger?.LogDebug($"Canceled ShowPopupAsync in {this.GetType()}.");
                throw;
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error while ShowPopupAsync in {this.GetType()}.");
            }

            return Task.CompletedTask;
        }

        public bool ClosePopup<TV>(BaseViewModel viewModel, TV result)
            where TV : NavigationParameters
        {
            try
            {
                var itemToClose = this.dialogLookup.FirstOrDefault(k => k.Value == viewModel);
                if (!itemToClose.Key.TryGetTarget(out Popup? popup))
                {
                    return false;
                }

                popup.Close(result);
                return true;
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error while ClosePopup in {this.GetType()}.");
            }

            return false;
        }

        public bool ClosePopup(BaseViewModel viewModel)
        {
            try
            {
                var itemToClose = this.dialogLookup.FirstOrDefault(k => k.Value == viewModel);
                if (!itemToClose.Key.TryGetTarget(out Popup? popup))
                {
                    return false;
                }

                popup.Close();
                return true;
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error while ClosePopup in {this.GetType()}.");
            }

            return false;
        }

        private bool UnregisterDialog(Popup popup)
        {
            if (popup == null)
            {
                return false;
            }

            this.CleanupLookups();
            var exitingItem = this.dialogLookup.FirstOrDefault(k => k.Key.TryGetTarget(out Popup? p) && p == popup);
            if (exitingItem.Key == null)
            {
                return false;
            }

            this.dialogLookup.Remove(exitingItem.Key);
            return true;
        }

        private void CleanupLookups()
        {
            var disposedItems = this.dialogLookup.Where(k => !k.Key.TryGetTarget(out _)).ToList();
            foreach (var disposedItem in disposedItems)
            {
                this.dialogLookup.Remove(disposedItem.Key);
            }
        }

        private bool RegisterDialog(Popup popup)
        {
            if (popup == null)
            {
                return false;
            }

            this.CleanupLookups();
            var dialog = popup;
            var viewModel = dialog.BindingContext as BaseViewModel;
            if (viewModel == null)
            {
                return false;
            }

            this.dialogLookup.Add(new WeakReference<Popup>(dialog), viewModel);
            return true;
        }
    }
}
