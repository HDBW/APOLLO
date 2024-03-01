// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.ViewModels;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Services
{
    public class DialogService : IDialogService
    {
        private readonly Dictionary<object, BaseViewModel> _completionLookup = new Dictionary<object, BaseViewModel>();

        public DialogService(IDispatcherService dispatcherService, ILogger<DialogService> logger, IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(dispatcherService);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(serviceProvider);
            DispatcherService = dispatcherService;
            Logger = logger;
            ServiceProvider = serviceProvider;
        }

        private ILogger Logger { get; }

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

        private IDispatcherService DispatcherService { get; }

        public async Task<TV?> ShowPopupAsync<TU, TV, TW>(TW parameters, CancellationToken token)
            where TU : Dialog
            where TV : NavigationParameters
            where TW : NavigationParameters?
        {
            token.ThrowIfCancellationRequested();
            Dialog? popup = null;
            TV? result = default;
            try
            {
                popup = await DispatcherService.ExecuteOnMainThreadAsync(() => { return Task.FromResult(ServiceProvider.GetService<TU>()); }, token);
                var viewModel = popup?.BindingContext as BaseViewModel;
                if (popup == null || viewModel == null)
                {
                    throw new NotSupportedException();
                }

                result = await DispatcherService.ExecuteOnMainThreadAsync(
                async () =>
                {
                    var queryAble = popup as IModalQueryAttributable ?? viewModel as IModalQueryAttributable;
                    if (queryAble != null && parameters != null)
                    {
                        queryAble.ApplyModalQueryAttributes(parameters.ToQueryDictionary());
                    }

                    var cts = new TaskCompletionSource<TV>(token);
                    _completionLookup.Add(cts, viewModel);
                    await (Navigation?.PushModalAsync(popup, false) ?? Task.CompletedTask);
                    var result = await cts.Task;
                    return result;
                }, token);
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
                await DispatcherService.ExecuteOnMainThreadAsync(() => (popup?.CloseAsync() ?? Task.CompletedTask), CancellationToken.None);
            }

            return result;
        }

        public Task<TV?> ShowPopupAsync<TU, TV>(CancellationToken token)
            where TU : Dialog
            where TV : NavigationParameters
        {
            return ShowPopupAsync<TU, TV, NavigationParameters?>(null, token);
        }

        public bool ClosePopup<TV>(BaseViewModel viewModel, TV result)
            where TV : NavigationParameters
        {
            try
            {
                var itemToClose = _completionLookup.FirstOrDefault(k => k.Value == viewModel);
                var completionHandler = itemToClose.Key as TaskCompletionSource<TV>;
                if (completionHandler == null)
                {
                    return false;
                }

                if (!completionHandler.TrySetResult(result))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(ClosePopup)}<{typeof(TV).Name}> in {GetType().Name}.");
            }

            return false;
        }
    }
}
