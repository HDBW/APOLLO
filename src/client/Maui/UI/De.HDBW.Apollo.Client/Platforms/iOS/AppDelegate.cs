// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CoreGraphics;
using De.HDBW.Apollo.Client.Contracts;
using Foundation;
using Microsoft.Identity.Client;
using UIKit;

namespace De.HDBW.Apollo.Client
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        public ITouchService? TouchService { get; set; }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
            return base.OpenUrl(app, url, options);
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var result = base.FinishedLaunching(application, launchOptions);
            if (result)
            {
                TouchService = IPlatformApplication.Current?.Services.GetService<ITouchService>();
                UITapGestureRecognizer tap = new UITapGestureRecognizer(Self, new ObjCRuntime.Selector("gestureRecognizer:shouldReceiveTouch:"));
                tap.Delegate = (IUIGestureRecognizerDelegate)Self;
                IPlatformApplication.Current?.Application?.Windows?.Select(x => x.Handler?.PlatformView as UIWindow).Where(x => x != null).FirstOrDefault(x => x.IsKeyWindow)?.AddGestureRecognizer(tap);
            }

            return result;
        }

        [Export("gestureRecognizer:shouldReceiveTouch:")]
        public bool ShouldReceiveTouch(UIGestureRecognizer gestureRecognizer, UITouch touch)
        {
            var point = touch?.LocationInView(touch.Window) ?? CGPoint.Empty;
            switch (gestureRecognizer?.State)
            {
                case UIGestureRecognizerState.Began:
                    TouchService?.TouchDownReceived((float)point.X, (float)point.Y);
                    break;
                case UIGestureRecognizerState.Ended:
                    TouchService?.TouchUpReceived((float)point.X, (float)point.Y);
                    break;
            }

            return false;
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
