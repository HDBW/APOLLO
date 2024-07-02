// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using HybridWebView;
using static Android.Views.ViewGroup;

namespace De.HDBW.Apollo.Client.Platforms
{
    public class CustomWebViewHandler : HybridWebViewHandler
    {
        public CustomWebViewHandler()
            : base()
        {
        }

        protected override CustomWebView CreatePlatformView()
        {
            var platformView = new CustomWebView(this, Context!)
            {
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent),
            };

            platformView.Settings.JavaScriptEnabled = true;
            platformView.Settings.DomStorageEnabled = true;
            platformView.Settings.SetSupportMultipleWindows(true);

            return platformView;
        }
    }
}
