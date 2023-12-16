// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using Microsoft.Extensions.Logging;
using The49.Maui.BottomSheet;

namespace De.HDBW.Apollo.Client.Services
{
    public class SheetService : ISheetService
    {
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

        private async Task OpenOnUIThreadAsnc(string route, CancellationToken token, NavigationParameters? parameters)
        {
            token.ThrowIfCancellationRequested();

            var sheet = Routing.GetOrCreateContent(route, ServiceProvider) as BottomSheet;
            if (sheet == null)
            {
                return;
            }

            var window = Application.Current?.Windows?.FirstOrDefault();
            if (window != null)
            {
                await sheet.ShowAsync(window, true);
                return;
            }

            await sheet.ShowAsync(true);
        }
    }
}
