﻿namespace De.HDBW.Apollo.Client.Services
{
    using De.HDBW.Apollo.Client.Contracts;
    using De.HDBW.Apollo.Client.Models;
    using De.HDBW.Apollo.Client.ViewModels;
    using Microsoft.Extensions.Logging;

    public class NavigationService : INavigationService
    {
        private readonly WeakEventManager weakEventManager = new WeakEventManager();

        public NavigationService(IDispatcherService dispatcherService, ILogger<NavigationService> logger, IServiceProvider serviceProvider)
        {
            this.Logger = logger;
            this.DispatcherService = dispatcherService;
            this.ServiceProvider = serviceProvider;
        }

        private ILogger Logger { get; }

        private IDispatcherService DispatcherService { get; }

        private IServiceProvider ServiceProvider { get; }

        public async Task<bool> PushToRootAsnc(string route, CancellationToken token, NavigationParameters parameters = null)
        {
            token.ThrowIfCancellationRequested();
            var result = false;
            try
            {
                await this.DispatcherService.ExecuteOnMainThreadAsync(() => this.PushToRootOnUIThreadAsnc(route, token, parameters), token);
                result = true;
            }
            catch (OperationCanceledException)
            {
                this.Logger?.LogDebug($"Canceled PushToRootAsnc in {this.GetType()}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                this.Logger?.LogDebug($"Canceled PushToRootAsnc in {this.GetType()}.");
                throw;
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error while PushToRootAsnc in {this.GetType()}.");
            }

            return result;
        }

        public async Task<bool> NavigateAsnc(string route, CancellationToken token, NavigationParameters parameters = null)
        {
            token.ThrowIfCancellationRequested();
            var result = false;
            try
            {
                await this.DispatcherService.ExecuteOnMainThreadAsync(() => this.NavigateOnUIThreadAsnc(route, token, parameters), token);
                result = true;
            }
            catch (OperationCanceledException)
            {
                this.Logger?.LogDebug($"Canceled NavigateAsync in {this.GetType()}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                this.Logger?.LogDebug($"Canceled NavigateAsync in {this.GetType()}.");
                throw;
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error while NavigateAsync in {this.GetType()}.");
            }

            return result;
        }

        private Task NavigateOnUIThreadAsnc(string route, CancellationToken token, NavigationParameters parameters)
        {
            var navigationPage = Application.Current.MainPage as NavigationPage;
            if (navigationPage != null && Shell.Current == null)
            {
                navigationPage.PushAsync(Routing.GetOrCreateContent(route, this.ServiceProvider) as Page, false);
            }

            if (parameters == null)
            {
                return Shell.Current?.GoToAsync(route, true);
            }
            else
            {
                return Shell.Current?.GoToAsync(route, true, parameters.ToQueryDictionary());
            }
        }

        private Task PushToRootOnUIThreadAsnc(string route, CancellationToken token, NavigationParameters parameters)
        {
            Application.Current.MainPage = Routing.GetOrCreateContent(route, this.ServiceProvider) as Page;
            return Task.CompletedTask;
        }
    }
}
