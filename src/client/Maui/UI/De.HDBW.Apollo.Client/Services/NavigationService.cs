using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Services
{
    public class NavigationService : INavigationService
    {
        public NavigationService(IDispatcherService dispatcherService, ILogger<NavigationService> logger, IServiceProvider serviceProvider)
        {
            Logger = logger;
            DispatcherService = dispatcherService;
            ServiceProvider = serviceProvider;
        }

        private ILogger Logger { get; }

        private IDispatcherService DispatcherService { get; }

        private IServiceProvider ServiceProvider { get; }

        public async Task<bool> PushToRootAsnc(string route, CancellationToken token, NavigationParameters? parameters = null)
        {
            Logger?.LogDebug($"PushToRoot to {route} with parameters: {parameters?.ToString()}.");
            token.ThrowIfCancellationRequested();
            var result = false;
            try
            {
                await DispatcherService.ExecuteOnMainThreadAsync(() => PushToRootOnUIThreadAsnc(route, token, parameters), token);
                result = true;
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled PushToRootAsnc in {GetType()}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled PushToRootAsnc in {GetType()}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error while PushToRootAsnc in {GetType()}.");
            }

            return result;
        }

        public async Task<bool> NavigateAsnc(string route, CancellationToken token, NavigationParameters? parameters = null)
        {
            Logger?.LogDebug($"Navigate to {route} with parameters: {parameters?.ToString()}.");
            token.ThrowIfCancellationRequested();
            var result = false;
            try
            {
                await DispatcherService.ExecuteOnMainThreadAsync(() => NavigateOnUIThreadAsnc(route, token, parameters), token);
                result = true;
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled NavigateAsync in {GetType()}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled NavigateAsync in {GetType()}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error while NavigateAsync in {GetType()}.");
            }

            return result;
        }

        private Task NavigateOnUIThreadAsnc(string route, CancellationToken token, NavigationParameters? parameters)
        {
            Logger?.LogDebug($"NavigateOnUI to {route} with parameters: {parameters?.ToString()}.");
            token.ThrowIfCancellationRequested();
            if (Application.Current == null || Shell.Current == null)
            {
                return Task.CompletedTask;
            }

            var navigationPage = Application.Current.MainPage as NavigationPage;
            if (navigationPage != null && Shell.Current == null)
            {
                return navigationPage.PushAsync(Routing.GetOrCreateContent(route, ServiceProvider) as Page, false);
            }

            if (parameters == null)
            {
                return Shell.Current.GoToAsync(route, true);
            }
            else
            {
                return Shell.Current.GoToAsync(route, true, parameters.ToQueryDictionary());
            }
        }

        private Task PushToRootOnUIThreadAsnc(string route, CancellationToken token, NavigationParameters? parameters)
        {
            Logger?.LogDebug($"PushToRootOnUI to {route} with parameters: {parameters?.ToString()}.");
            token.ThrowIfCancellationRequested();
            if (Application.Current == null)
            {
                return Task.CompletedTask;
            }

            var page = Routing.GetOrCreateContent(route, ServiceProvider) as Page;
            var navigationPage = Application.Current.MainPage as NavigationPage;
            if (navigationPage == null || page is Shell)
            {
                Application.Current.MainPage = page;
            }
            else
            {
                navigationPage.Navigation.PushAsync(page, true);
                var existingPages = navigationPage.Navigation.NavigationStack.ToList();
                foreach (var existingPage in existingPages)
                {
                    if (existingPage == page)
                    {
                        continue;
                    }

                    navigationPage.Navigation.RemovePage(existingPage);
                }
            }

            return Task.CompletedTask;
        }
    }
}