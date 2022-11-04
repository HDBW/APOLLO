// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.ViewModels;
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

        private INavigation Navigation
        {
            get
            {
                var navigation = Application.Current?.MainPage?.Navigation;
                if (navigation == null)
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }

                    throw new NotSupportedException("Navigation was not found");
                }

                return navigation;
            }
        }

        public async Task<bool> PushToRootAsnc(string route, CancellationToken token, NavigationParameters? parameters = null)
        {
            Logger?.LogDebug($"PushToRoot to {route} with parameters: {parameters?.ToString() ?? "null"}.");
            token.ThrowIfCancellationRequested();
            var result = false;
            try
            {
                await DispatcherService.ExecuteOnMainThreadAsync(() => PushToRootOnUIThreadAsnc(route, token, parameters), token);
                result = true;
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled {nameof(PushToRootAsnc)} in {GetType()}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled {nameof(PushToRootAsnc)} in {GetType()}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(PushToRootAsnc)} in {GetType()}.");
            }

            return result;
        }

        public async Task<bool> NavigateAsnc(string route, CancellationToken token, NavigationParameters? parameters = null)
        {
            Logger?.LogDebug($"Navigate to {route} with parameters: {parameters?.ToString() ?? "null"}.");
            token.ThrowIfCancellationRequested();
            var result = false;
            try
            {
                await DispatcherService.ExecuteOnMainThreadAsync(() => NavigateOnUIThreadAsnc(route, token, parameters), token);
                result = true;
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled {nameof(NavigateAsnc)} in {GetType()}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled {nameof(NavigateAsnc)} in {GetType()}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error while {nameof(NavigateAsnc)} in {GetType()}.");
            }

            return result;
        }

        public async Task<bool> ResetNavigationAsnc(string route, CancellationToken token, NavigationParameters? parameters = null)
        {
            Logger?.LogDebug($"ResetNavigationAsnc to {route} with parameters: {parameters?.ToString() ?? "null"}.");
            token.ThrowIfCancellationRequested();
            var result = false;
            try
            {
                await DispatcherService.ExecuteOnMainThreadAsync(() => ResetNavigationOnUIThreadAsnc(route, token, parameters), token);
                result = true;
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled {nameof(ResetNavigationAsnc)} in {GetType()}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled {nameof(ResetNavigationAsnc)} in {GetType()}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(ResetNavigationAsnc)} in {GetType()}.");
            }

            return result;
        }

        private async Task NavigateOnUIThreadAsnc(string route, CancellationToken token, NavigationParameters? parameters)
        {
            Logger?.LogDebug($"NavigateOnUI to {route} with parameters: {parameters?.ToString() ?? "null"}.");
            token.ThrowIfCancellationRequested();
            if (Application.Current == null && Shell.Current == null)
            {
                return;
            }

            var navigationPage = Application.Current?.MainPage as NavigationPage;
            if (navigationPage != null && Shell.Current == null)
            {
                var page = Routing.GetOrCreateContent(route, ServiceProvider) as Page;
                if (page == null)
                {
                    return;
                }

                var queryAble = page as IQueryAttributable ?? page.BindingContext as IQueryAttributable;
                if (queryAble != null && parameters != null)
                {
                    queryAble.ApplyQueryAttributes(parameters.ToQueryDictionary());
                }

                page.NavigatedTo += NavigatedToPage;
                await navigationPage.PushAsync(page, false);
                page.NavigatedFrom += NavigatedFromPage;
                return;
            }

            if (parameters == null)
            {
                await Shell.Current.GoToAsync(route, true);
            }
            else
            {
                await Shell.Current.GoToAsync(route, true, parameters.ToQueryDictionary());
            }
        }

        private Task PushToRootOnUIThreadAsnc(string route, CancellationToken token, NavigationParameters? parameters)
        {
            Logger?.LogDebug($"PushToRootOnUI to {route} with parameters: {parameters?.ToString() ?? "null"}.");
            token.ThrowIfCancellationRequested();
            if (Application.Current == null)
            {
                return Task.CompletedTask;
            }

            var page = Routing.GetOrCreateContent(route, ServiceProvider) as Page;
            if (page == null)
            {
                return Task.CompletedTask;
            }

            var queryAble = page as IQueryAttributable ?? page.BindingContext as IQueryAttributable;
            if (queryAble != null && parameters != null)
            {
                queryAble.ApplyQueryAttributes(parameters.ToQueryDictionary());
            }

            page.NavigatedTo += NavigatedToPage;
            page.NavigatedFrom += NavigatedFromPage;

            var navigationPage = Application.Current.MainPage as NavigationPage;
            if (navigationPage == null || page is Shell)
            {
                var shell = Application.Current.MainPage as Shell;
                var currentPage = Application.Current.MainPage;
                Application.Current.MainPage = page;
                if (shell != null)
                {
                    shell.Navigating -= NavigatedFromPageInShell;
                    shell.Navigated -= NavigatedToPageInShell;
                    NavigatedFromPageInShell(shell, null);
                }

                if (currentPage != null && shell == null)
                {
                    currentPage.NavigatedFrom -= NavigatedFromPage;
                    currentPage.NavigatedTo -= NavigatedToPage;
                    NavigatedFromPage(currentPage, null);
                }

                shell = Application.Current.MainPage as Shell;
                if (shell != null)
                {
                    shell.Navigating += NavigatedFromPageInShell;
                    shell.Navigated += NavigatedToPageInShell;

                    if (!(currentPage is Shell))
                    {
                        NavigatedToPage(shell, null);
                    }

                    NavigatedToPageInShell(shell, null);
                }
                else if (page != null)
                {
                    NavigatedToPage(page, null);
                }
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

                    existingPage.NavigatedTo -= NavigatedToPage;
                    existingPage.NavigatedFrom -= NavigatedFromPage;
                    navigationPage.Navigation.RemovePage(existingPage);
                    NavigatedFromPage(existingPage, null);
                }
            }

            return Task.CompletedTask;
        }

        private async Task ResetNavigationOnUIThreadAsnc(string route, CancellationToken token, NavigationParameters? parameters)
        {
            Logger?.LogDebug($"ResetNavigationOnUI to {route} with parameters: {parameters?.ToString() ?? "null"}.");
            token.ThrowIfCancellationRequested();
            if (Application.Current == null)
            {
                return;
            }

            var page = Routing.GetOrCreateContent(route, ServiceProvider) as Page;
            if (page == null)
            {
                return;
            }

            var queryAble = page as IQueryAttributable ?? page.BindingContext as IQueryAttributable;
            if (queryAble != null && parameters != null)
            {
                queryAble.ApplyQueryAttributes(parameters.ToQueryDictionary());
            }

            page.NavigatedTo += NavigatedToPage;
            page.NavigatedFrom += NavigatedFromPage;

            var navigationPage = Application.Current.MainPage as NavigationPage;
            var shell = Application.Current.MainPage as Shell;

            Application.Current.MainPage = page;

            if (navigationPage != null)
            {
                var existingPages = navigationPage.Navigation.NavigationStack.ToList();
                foreach (var existingPage in existingPages)
                {
                    if (existingPage == page)
                    {
                        continue;
                    }

                    existingPage.NavigatedTo -= NavigatedToPage;
                    existingPage.NavigatedFrom -= NavigatedFromPage;
                    navigationPage.Navigation.RemovePage(existingPage);
                    NavigatedFromPage(existingPage, null);
                }
            }

            if (shell != null)
            {
                shell.Navigating -= NavigatedFromPageInShell;
                shell.Navigated -= NavigatedToPageInShell;
                var isNavigatingBack = true;
                while (isNavigatingBack)
                {
                    var stackedPage = await shell.Navigation.PopAsync(false);
                    if (stackedPage == null)
                    {
                        isNavigatingBack = false;
                        continue;
                    }

                    NavigatedFromPageInShell(stackedPage, null);
                    shell.Navigation.RemovePage(shell.CurrentPage);
                }
            }
        }

        private void NavigatedFromPageInShell(object? sender, ShellNavigatingEventArgs? e)
        {
            var shell = sender as Shell;
            var page = shell?.CurrentPage;
            if (page == null)
            {
                return;
            }

            NavigatedFromPage(page, null);
        }

        private void NavigatedToPageInShell(object? sender, ShellNavigatedEventArgs? e)
        {
            var shell = sender as Shell;
            var page = shell?.CurrentPage;
            if (page == null)
            {
                return;
            }

            NavigatedToPage(page, null);
        }

        private async void NavigatedFromPage(object? sender, NavigatedFromEventArgs? e)
        {
            try
            {
                bool isForwardNavigation = Navigation.NavigationStack.Count > 1
                && Navigation.NavigationStack[^2] == sender;
                var page = sender as Page;
                if (page == null)
                {
                    Logger?.LogWarning($"NavigatedFromPage sender was not a Page.");
                    return;
                }

                if (!isForwardNavigation)
                {
                    page.NavigatedTo -= NavigatedToPage;
                    page.NavigatedFrom -= NavigatedFromPage;
                }

                await (GetViewModel(page)?.OnNavigatingFromAsync(isForwardNavigation) ?? Task.CompletedTask);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {NavigatedFromPage} in {GetType()}.");
            }
        }

        private async void NavigatedToPage(object? sender, NavigatedToEventArgs? e)
        {
            try
            {
                var page = sender as Page;
                if (page == null)
                {
                    Logger?.LogWarning($"{nameof(NavigatedToPage)} sender was not a Page.");
                    return;
                }

                await (GetViewModel(page)?.OnNavigatedToAsync() ?? Task.CompletedTask);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(NavigatedToPage)} in {GetType()}.");
            }
        }

        private BaseViewModel? GetViewModel(Page page)
        {
            return page.BindingContext as BaseViewModel;
        }
    }
}
