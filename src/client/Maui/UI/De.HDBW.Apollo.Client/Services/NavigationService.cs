// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Messages;
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

        public async Task<bool> PushToRootAsync(string route, CancellationToken token, NavigationParameters? parameters = null)
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
                Logger?.LogDebug($"Canceled {nameof(PushToRootAsync)} in {GetType().Name}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled {nameof(PushToRootAsync)} in {GetType().Name}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(PushToRootAsync)} in {GetType().Name}.");
            }

            return result;
        }

        public async Task<bool> PushToRootAsync(CancellationToken token)
        {
            Logger?.LogDebug($"PushToRoot.");
            token.ThrowIfCancellationRequested();
            var result = false;
            try
            {
                await DispatcherService.ExecuteOnMainThreadAsync(() => PushToRootOnUIThreadAsnc(token), token);
                result = true;
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled {nameof(PushToRootAsync)} in {GetType().Name}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled {nameof(PushToRootAsync)} in {GetType().Name}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(PushToRootAsync)} in {GetType().Name}.");
            }

            return result;
        }

        public async Task<bool> PopAsync(CancellationToken token, NavigationParameters? parameters)
        {
            Logger?.LogDebug($"PopAsync.");
            token.ThrowIfCancellationRequested();
            var result = false;
            try
            {
                await DispatcherService.ExecuteOnMainThreadAsync(() => PopOnUIThreadAsnc(token, parameters), token);
                result = true;
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled {nameof(PopAsync)} in {GetType().Name}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled {nameof(PopAsync)} in {GetType().Name}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(PopAsync)} in {GetType().Name}.");
            }

            return result;
        }

        public async Task<bool> NavigateAsync(string route, CancellationToken token, NavigationParameters? parameters = null)
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
                Logger?.LogDebug($"Canceled {nameof(NavigateAsync)} in {GetType().Name}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled {nameof(NavigateAsync)} in {GetType().Name}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown Error while {nameof(NavigateAsync)} in {GetType().Name}.");
            }

            return result;
        }

        public async Task RestartAsync(bool confirmedDataUsage, CancellationToken token)
        {
            await DispatcherService.ExecuteOnMainThreadAsync(
                () =>
                {
                    try
                    {
                        if (Application.Current == null)
                        {
                            Logger.LogWarning($"Application is null during {nameof(RestartAsync)} in {GetType().Name}.");
                            return Task.CompletedTask;
                        }

                        var current = Application.Current.MainPage;

                        var route = confirmedDataUsage ? Routes.RegistrationView : Routes.ExtendedSplashScreenView;
                        Application.Current.MainPage = new NavigationPage(Routing.GetOrCreateContent(route, ServiceProvider) as Page);
                        if (current != null)
                        {
                            var stack = current.Navigation.NavigationStack.ToArray();
                            for (int i = stack.Length - 1; i > 0; i--)
                            {
                                current.Navigation.RemovePage(stack[i]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger?.LogError(ex, $"Unknown error while {nameof(RestartAsync)} in {GetType().Name}.");
                    }

                    return Task.CompletedTask;
                }, token);
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

                page.NavigatedTo -= NavigatedToPage;
                page.NavigatedTo += NavigatedToPage;
                await navigationPage.PushAsync(page, false);
                page.NavigatedFrom -= NavigatedFromPage;
                page.NavigatedFrom += NavigatedFromPage;
                return;
            }

            Shell.Current.FlyoutIsPresented = false;
            if (parameters == null)
            {
                await Shell.Current.GoToAsync(route, true);
            }
            else
            {
                await Shell.Current.GoToAsync(route, true, parameters.ToQueryDictionary());
            }
        }

        private async Task PushToRootOnUIThreadAsnc(string route, CancellationToken token, NavigationParameters? parameters)
        {
            Logger?.LogDebug($"PushToRootOnUI to {route} with parameters: {parameters?.ToString() ?? "null"}.");
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

            SubscribePageEvents(page);

            var navigationPage = Application.Current.MainPage as NavigationPage;
            if (navigationPage != null && !(page is Shell))
            {
                await navigationPage.Navigation.PushAsync(page, true);
                var existingPages = navigationPage.Navigation.NavigationStack.ToList();
                foreach (var existingPage in existingPages)
                {
                    if (existingPage == page)
                    {
                        continue;
                    }

                    UnsubscribePageEvents(existingPage);

                    if (existingPage != navigationPage.CurrentPage)
                    {
                        navigationPage.Navigation.RemovePage(existingPage);
                    }

                    NavigatedFromPage(existingPage, null);
                }

                return;
            }

            var shell = Application.Current.MainPage as Shell;
            if (shell != null)
            {
                Shell.Current.FlyoutIsPresented = false;
                await PopShellAsync(Shell.Current);
                if (parameters == null)
                {
                    await Shell.Current.GoToAsync(route, true);
                }
                else
                {
                    await Shell.Current.GoToAsync(route, true, parameters.ToQueryDictionary());
                }
            }

            if (page is Shell)
            {
                Application.Current.MainPage = page;
                NavigatedToPage(page, null);

                var existingPages = navigationPage?.Navigation?.NavigationStack?.ToList() ?? new List<Page>();
                foreach (var existingPage in existingPages)
                {
                    UnsubscribePageEvents(existingPage);
                    if (navigationPage?.CurrentPage != existingPage)
                    {
                        navigationPage?.Navigation?.RemovePage(existingPage);
                    }

                    NavigatedFromPage(existingPage, null);
                }

                shell = Application.Current.MainPage as Shell;
                if (shell != null)
                {
                    shell.Navigating -= NavigatedFromPageInShell;
                    shell.Navigated -= NavigatedToPageInShell;
                    shell.Navigating += NavigatedFromPageInShell;
                    shell.Navigated += NavigatedToPageInShell;
                    NavigatedToPageInShell(shell, null);
                }
            }
        }

        private async Task PopOnUIThreadAsnc(CancellationToken token, NavigationParameters? parameters)
        {
            Logger?.LogDebug($"PopOnUIThreadAsnc.");
            token.ThrowIfCancellationRequested();
            if (Application.Current == null)
            {
                return;
            }

            var navigationPage = Application.Current.MainPage as NavigationPage;
            if (navigationPage != null)
            {
                await navigationPage.Navigation.PopAsync(true);
                return;
            }

            var shell = Application.Current.MainPage as Shell;
            if (shell != null)
            {
                Shell.Current.FlyoutIsPresented = false;
                if (parameters != null)
                {
                    await Shell.Current.GoToAsync("..", parameters.ToQueryDictionary());
                }
                else
                {
                    await Shell.Current.GoToAsync("..");
                }
            }
        }

        private async Task PopShellAsync(Shell shell)
        {
            foreach (var page in shell.Navigation.NavigationStack)
            {
                if (page == null)
                {
                    continue;
                }

                UnsubscribePageEvents(page);
                await (GetViewModel(page)?.OnNavigatingFromAsync() ?? Task.CompletedTask);
            }

            await Shell.Current.Navigation.PopToRootAsync(false);
        }

        private void SubscribePageEvents(Page page)
        {
            UnsubscribePageEvents(page);
            if (page == null)
            {
                return;
            }

            page.NavigatedTo += NavigatedToPage;
            page.NavigatedFrom += NavigatedFromPage;
        }

        private void UnsubscribePageEvents(Page page)
        {
            if (page == null)
            {
                return;
            }

            page.NavigatedTo -= NavigatedToPage;
            page.NavigatedFrom -= NavigatedFromPage;
        }

        private async Task PushToRootOnUIThreadAsnc(CancellationToken token)
        {
            Logger?.LogDebug($"PushToRootOnUI.");
            token.ThrowIfCancellationRequested();
            if (Application.Current == null)
            {
                return;
            }

            var navigationPage = Application.Current.MainPage as NavigationPage;
            if (navigationPage != null)
            {
                await navigationPage.Navigation.PopToRootAsync(true);
                return;
            }

            var shell = Application.Current.MainPage as Shell;
            if (shell != null)
            {
                await PopShellAsync(shell);
                NavigatedToPageInShell(shell.CurrentPage, null);
                NavigatedToPage(shell, null);
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

            if (e?.Source == ShellNavigationSource.ShellSectionChanged)
            {
                var vm = GetViewModel(page);
                WeakReferenceMessenger.Default.Send(new ShellContentChangedMessage(vm?.GetType()));
            }

            NavigatedToPage(page, null);
        }

        private async void NavigatedFromPage(object? sender, NavigatedFromEventArgs? e)
        {
            try
            {
                var page = sender as Page;
                if (page == null)
                {
                    Logger?.LogWarning($"NavigatedFromPage sender was not a Page.");
                    return;
                }

                UnsubscribePageEvents(page);
                await (GetViewModel(page)?.OnNavigatingFromAsync() ?? Task.CompletedTask);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(NavigatedFromPage)} in {GetType().Name}.");
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
                Logger?.LogError(ex, $"Unknown error while {nameof(NavigatedToPage)} in {GetType().Name}.");
            }
        }

        private BaseViewModel? GetViewModel(Page page)
        {
            return page.BindingContext as BaseViewModel;
        }
    }
}
