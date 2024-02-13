// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.ViewModels;
using Microsoft.Extensions.Logging;
using The49.Maui.BottomSheet;

namespace De.HDBW.Apollo.Client.Services
{
    public class SheetService : ISheetService
    {
        private readonly Dictionary<WeakReference<BottomSheet>, BaseViewModel> _sheetLookup = new Dictionary<WeakReference<BottomSheet>, BaseViewModel>();

        public SheetService(IDispatcherService dispatcherService, ILogger<SheetService> logger, IServiceProvider serviceProvider)
        {
            Logger = logger;
            DispatcherService = dispatcherService;
            ServiceProvider = serviceProvider;
        }

        private ILogger Logger { get; }

        private IDispatcherService DispatcherService { get; }

        private IServiceProvider ServiceProvider { get; }

        public async Task<bool> OpenAsync(string route, CancellationToken token, NavigationParameters? parameters = null)
        {
            Logger?.LogDebug($"Open Sheet {route} with parameters: {parameters?.ToString() ?? "null"}.");
            token.ThrowIfCancellationRequested();
            var result = false;
            try
            {
                await DispatcherService.ExecuteOnMainThreadAsync(() => OpenOnUIThreadAsnc(route, token, parameters), token);
                result = true;
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled {nameof(OpenAsync)} in {GetType().Name}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled {nameof(OpenAsync)} in {GetType().Name}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error while {nameof(OpenAsync)} in {GetType().Name}.");
            }

            return result;
        }

        public async Task<bool> CloseAsync(BaseViewModel viewModel)
        {
            try
            {
                var itemToClose = _sheetLookup.FirstOrDefault(k => k.Value == viewModel);
                if (!itemToClose.Key.TryGetTarget(out BottomSheet? sheet))
                {
                    return false;
                }

                await sheet.DismissAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(CloseAsync)} in {GetType().Name}.");
            }

            return false;
        }

        private async Task OpenOnUIThreadAsnc(string route, CancellationToken token, NavigationParameters? parameters)
        {
            token.ThrowIfCancellationRequested();

            var sheet = Routing.GetOrCreateContent(route, ServiceProvider) as BottomSheet;
            if (sheet == null)
            {
                return;
            }

            RegisterSheet(sheet);
            var window = Application.Current?.Windows?.FirstOrDefault();
            if (window != null)
            {
                await sheet.ShowAsync(window, true);
                return;
            }

            await sheet.ShowAsync(true);
            UnregisterSheet(sheet);
        }

        private bool UnregisterSheet(BottomSheet sheet)
        {
            if (sheet == null)
            {
                return false;
            }

            CleanupLookups();
            var exitingItem = _sheetLookup.FirstOrDefault(k => k.Key.TryGetTarget(out BottomSheet? s) && s == sheet);
            if (exitingItem.Key == null)
            {
                return false;
            }

            _sheetLookup.Remove(exitingItem.Key);
            return true;
        }

        private void CleanupLookups()
        {
            var disposedItems = _sheetLookup.Where(k => !k.Key.TryGetTarget(out _)).ToList();
            foreach (var disposedItem in disposedItems)
            {
                _sheetLookup.Remove(disposedItem.Key);
            }
        }

        private bool RegisterSheet(BottomSheet sheet)
        {
            if (sheet == null)
            {
                return false;
            }

            CleanupLookups();
            var viewModel = sheet.BindingContext as BaseViewModel;
            if (viewModel == null)
            {
                return false;
            }

            _sheetLookup.Add(new WeakReference<BottomSheet>(sheet), viewModel);
            return true;
        }
    }
}
