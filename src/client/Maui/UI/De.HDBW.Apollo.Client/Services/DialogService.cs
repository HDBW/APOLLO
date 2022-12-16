﻿// (c) Licensed to the HDBW under one or more agreements.
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

        public async Task<TV?> ShowPopupAsync<TU, TV, TW>(CancellationToken token, TW parameters)
            where TU : Popup
            where TV : NavigationParameters
            where TW : NavigationParameters?
        {
            token.ThrowIfCancellationRequested();
            Popup? popup = null;
            try
            {
                popup = ServiceProvider.GetService<TU>();

                var rootPage = Shell.Current?.CurrentPage ?? Application.Current?.MainPage;
                if (popup == null || rootPage == null)
                {
                    throw new NotSupportedException();
                }

                RegisterDialog(popup);
                var result = await DispatcherService.ExecuteOnMainThreadAsync(
                    () =>
                {
                    var queryAble = popup as IQueryAttributable ?? popup.BindingContext as IQueryAttributable;
                    if (queryAble != null && parameters != null)
                    {
                        queryAble.ApplyQueryAttributes(parameters.ToQueryDictionary());
                    }

                    return rootPage.ShowPopupAsync(popup);
                }, token);

                UnregisterDialog(popup);
                popup.Parent = null;
                return result as TV;
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled {nameof(ShowPopupAsync)}<{typeof(TU).Name}, {typeof(TV).Name}> in {GetType().Name}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled {nameof(ShowPopupAsync)}<{typeof(TU).Name}, {typeof(TV).Name}> in {GetType().Name}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(ShowPopupAsync)}<{typeof(TU).Name}, {typeof(TV).Name}> in {GetType().Name}.");
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

        public Task<TV?> ShowPopupAsync<TU, TV>(CancellationToken token)
            where TU : Popup
            where TV : NavigationParameters
        {
            return ShowPopupAsync<TU, TV, NavigationParameters?>(token, null);
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
                Logger?.LogDebug($"Canceled {nameof(ShowPopupAsync)}<{typeof(TU).Name}> in {GetType().Name}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled {nameof(ShowPopupAsync)}<{typeof(TU).Name}> in {GetType().Name}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(ShowPopupAsync)}<{typeof(TU).Name}> in {GetType().Name}.");
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
                UnregisterDialog(popup);
                popup.Parent = null;
                return true;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(ClosePopup)}<{typeof(TV).Name}> in {GetType().Name}.");
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
                Logger?.LogError(ex, $"Unknown error while {nameof(ClosePopup)} in {GetType().Name}.");
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
