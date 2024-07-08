// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.ViewModels.Assessments;

namespace De.HDBW.Apollo.Client.Helper
{
    public sealed class JSBridge
    {
        private readonly WeakReference _reference;

        private readonly WeakReference _dispatcher;

        private readonly WeakReference _sheetService;

        private string? _lastId;

        public JSBridge(HybridWebView.HybridWebView webView, ISheetService sheetService)
        {
            ArgumentNullException.ThrowIfNull(webView);
            ArgumentNullException.ThrowIfNull(sheetService);
            _reference = new WeakReference(webView);
            _dispatcher = new WeakReference(webView.Dispatcher);
            _sheetService = new WeakReference(sheetService);
            WeakReferenceMessenger.Default.Register<SelectionMessage>(this, OnSelectionMessage);
        }

        /// <summary>
        /// Called from JS.
        /// </summary>
        /// <param name="id">The id of the input.</param>
        /// <param name="value">The current value of the input.</param>
        public async void SetValue(string id, string value)
        {
            if (!(_dispatcher?.IsAlive ?? false))
            {
                return;
            }

            var dispatcher = _dispatcher?.Target as Dispatcher;
            if (dispatcher == null)
            {
                return;
            }

            await dispatcher.DispatchAsync(() =>
            {
                WeakReferenceMessenger.Default.Send(new SetValueMessage(id, value));
            });
        }

        /// <summary>
        /// Called from JS.
        /// </summary>
        /// <param name="id">The id of the input.</param>
        public async void RemovedFocused(string id)
        {
            var sheetService = _sheetService?.Target as ISheetService;
            if (!(sheetService?.IsShowingSheet ?? false))
            {
                return;
            }

            var dispatcher = _dispatcher?.Target as Dispatcher;
            if (dispatcher == null)
            {
                return;
            }

            await dispatcher.DispatchAsync(() =>
            {
                sheetService.CloseAsync<SelectionSheetViewModel>();
            });
        }

        public async void SetFocused(string? id, bool readOnly)
        {
            if (!(_dispatcher?.IsAlive ?? false))
            {
                return;
            }

            var dispatcher = _dispatcher?.Target as Dispatcher;
            if (dispatcher == null)
            {
                return;
            }

            await dispatcher.DispatchAsync(() => SetFocusAsync(id, readOnly));
        }

        private async void OnSelectionMessage(object recipient, SelectionMessage message)
        {
            if (!(_dispatcher?.IsAlive ?? false))
            {
                return;
            }

            var dispatcher = _dispatcher?.Target as Dispatcher;
            if (dispatcher == null)
            {
                return;
            }

            await dispatcher.DispatchAsync(() => OnSelectionMessageAsync(message.Id, message.Text));
        }

        private async void HandleDismissed(object? sender, SheetDismissedMessage message)
        {
            var sheet = sender as JSBridge;
            if (sheet != null)
            {
                WeakReferenceMessenger.Default.Unregister<SheetDismissedMessage>(this);
            }

            await UnfocuseHtmlAsync();
        }

        private async Task OnSelectionMessageAsync(string id, string value)
        {
            await SetTextAsync(id, value);
            await UnfocuseHtmlAsync();
            if (!(_dispatcher?.IsAlive ?? false))
            {
                return;
            }

            var dispatcher = _dispatcher?.Target as Dispatcher;
            if (dispatcher == null)
            {
                return;
            }

            await dispatcher.DispatchAsync(() =>
            {
                WeakReferenceMessenger.Default.Send(new SetValueMessage(id, value));
            });
        }

        private async Task UnfocuseHtmlAsync()
        {
            var webView = _reference.IsAlive ? _reference.Target as HybridWebView.HybridWebView : null;
            if (webView == null || string.IsNullOrWhiteSpace(_lastId))
            {
                return;
            }

            var method = $"removeFocus('{_lastId}')";
            _lastId = null;
            await webView.EvaluateJavaScriptAsync(method);
        }

        private Task SetFocusAsync(string? id, bool readOnly)
        {
            if (readOnly && !string.IsNullOrWhiteSpace(id))
            {
                _lastId = id;

                if (!(_sheetService?.IsAlive ?? false))
                {
                    return Task.CompletedTask;
                }

                var sheetService = _sheetService?.Target as ISheetService;
                if (sheetService == null)
                {
                    return Task.CompletedTask;
                }

                var webView = _reference.IsAlive ? _reference.Target as HybridWebView.HybridWebView : null;
                var viewModel = webView?.BindingContext as ClozeViewModel;
                var question = viewModel?.Question;
                var values = question?.GetPossibleValues(id) ?? new List<string>();
                if (values.Count == 0)
                {
                    return Task.CompletedTask;
                }

                var parameters = new NavigationParameters
                {
                    { NavigationParameter.Id, _lastId },
                    { NavigationParameter.Data, string.Join(";", values) },
                };

                WeakReferenceMessenger.Default.Register<SheetDismissedMessage>(this, HandleDismissed);
                return sheetService.OpenAsync(Routes.SelectionSheet, CancellationToken.None, parameters);
            }

            _lastId = null;
            return Task.CompletedTask;
        }

        private async Task SetTextAsync(string? id, string? value)
        {
            var webView = _reference.IsAlive ? _reference.Target as HybridWebView.HybridWebView : null;
            if (webView == null || string.IsNullOrWhiteSpace(id))
            {
                return;
            }

            await webView.EvaluateJavaScriptAsync($"setText('{id}', '{value ?? string.Empty}')");
        }
    }
}
