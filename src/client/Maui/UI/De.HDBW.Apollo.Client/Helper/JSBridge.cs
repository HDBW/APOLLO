// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.Views.Assessments;
using The49.Maui.BottomSheet;

namespace De.HDBW.Apollo.Client.Helper
{
    public sealed class JSBridge
    {
        private readonly WeakReference _reference;
        private readonly IDispatcher _dispatcher;
        private string? _lastId;
        private BottomSheet? _sheet;

        public JSBridge(HybridWebView.HybridWebView webView)
        {
            _reference = new WeakReference(webView);
            _dispatcher = webView.Dispatcher;
            WeakReferenceMessenger.Default.Register<SelectionMessage>(this, OnSelectionMessage);
        }

        /// <summary>
        /// Called from JS.
        /// </summary>
        public async void SetValue(string id, string value)
        {
            WeakReferenceMessenger.Default.Send(new SetValueMessage(id, value));
        }

        /// <summary>
        /// Called from JS.
        /// </summary>
        public async void RemovedFocused(string id)
        {
            var sheet = _sheet;
            if (sheet != null)
            {
                _sheet = null;
                try
                {
                    if (sheet.Parent != null)
                    {
                        await sheet.DismissAsync();
                    }
                }
                catch
                {
                }
            }
        }

        public async void SetFocused(string? id, bool readOnly)
        {
            await _dispatcher.DispatchAsync(() => SetFocusAsync(id, readOnly));
        }

        private async void OnSelectionMessage(object recipient, SelectionMessage message)
        {
            await _dispatcher.DispatchAsync(() => OnSelectionMessageAsync(message.Text));
        }

        private async void HandleDismissed(object? sender, DismissOrigin e)
        {
            var sheet = sender as BottomSheet;
            if (sheet != null)
            {
                _sheet = null;
                sheet.Dismissed -= HandleDismissed;
            }

            await UnfocuseHtmlAsync();
        }

        private async Task OnSelectionMessageAsync(string value)
        {
            await SetTextAsync(_lastId, value);
            await UnfocuseHtmlAsync();
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
            if (readOnly)
            {
                _lastId = id;
                _sheet = new SelectionSheet();
                _sheet.Dismissed += HandleDismissed;
                return _sheet.ShowAsync(true);
            }

            _lastId = null;
            var sheet = _sheet;
            if (sheet != null)
            {
                _sheet = null;
                return sheet.DismissAsync();
            }

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
