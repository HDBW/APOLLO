// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.ViewModels.Assessments;

namespace De.HDBW.Apollo.Client.Views.Assessments
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class ClozeView : ContentPage
    {
        public ClozeView(ClozeViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            PART_WebView.JSInvokeTarget = new JSBridge(PART_WebView);
            PART_WebView.ProxyRequestReceived += HandleRequest;

#if IOS
            var webView = PART_WebView.Handler?.PlatformView as WebKit.WKWebView;
            if (webView == null)
            {
                return;
            }

            webView.ScrollView.ScrollEnabled = false;
#endif
        }

        public ClozeViewModel? ViewModel
        {
            get
            {
                return BindingContext as ClozeViewModel;
            }
        }

        protected override void OnAppearing()
        {
            WeakReferenceMessenger.Default.Register<ReloadMessage>(this, HandleReload);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            WeakReferenceMessenger.Default.Unregister<ReloadMessage>(this);
            base.OnAppearing();
        }

        private void HandleReload(object recipient, ReloadMessage message)
        {
            if (ViewModel == null || !ViewModel.IsWebViewLoaded)
            {
                return;
            }

            PART_WebView.Reload();
        }

#if IOS
        private async void HandleNavigated(object sender, Microsoft.Maui.Controls.WebNavigatedEventArgs e)
        {
            var completed = await PART_WebView.EvaluateJavaScriptAsync("document.readyState");
            if (completed == null)
            {
                return;
            }

            var height = await PART_WebView.EvaluateJavaScriptAsync("document.body.scrollHeight");
            if (!double.TryParse(height, out double documentHeigh))
            {
                return;
            }

            PART_WebView.HeightRequest = documentHeigh;
        }
#else
        private void HandleNavigated(object sender, WebNavigatedEventArgs e)
        {
        }

#endif

        private Task HandleRequest(HybridWebView.HybridWebViewProxyEventArgs arg)
        {
            if ((ViewModel?.IsWebViewLoaded ?? false) && ViewModel?.Question?.ClozeHtml != null)
            {
                arg.ResponseContentType = "text/html";
                arg.ResponseStream = ViewModel.Question.ClozeHtml.ToStream();
            }
            else
            {
                arg.ResponseStream = new MemoryStream();
            }

            return Task.CompletedTask;
        }

        private void HandleLoaded(object sender, EventArgs e)
        {
            if (ViewModel == null || ViewModel.IsWebViewLoaded)
            {
                return;
            }

            ViewModel.IsWebViewLoaded = true;
            PART_WebView.Reload();
        }

        private void OnMessageReceived(object sender, HybridWebView.HybridWebViewRawMessageReceivedEventArgs e)
        {
        }
    }
}
