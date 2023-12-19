﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Foundation;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Identity.Client;
using ProtoBuf.Meta;
using UIKit;

namespace De.HDBW.Apollo.Client;
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate, IUIGestureRecognizerDelegate
{
    public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
    {
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
        return base.OpenUrl(app, url, options);
    }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // https://stackoverflow.com/questions/26931083/kvo-of-contentsize-in-c-sharp
        var result = base.FinishedLaunching(application, launchOptions);
        if (result)
        {
            UITapGestureRecognizer tap = new UITapGestureRecognizer(Self, new ObjCRuntime.Selector("gestureRecognizer:shouldReceiveTouch:"));
            tap.Delegate = (IUIGestureRecognizerDelegate)Self;
            IPlatformApplication.Current?.Application?.Windows?.Select(x => x.Handler?.PlatformView as UIWindow).Where(x => x != null).FirstOrDefault(x => x.IsKeyWindow)?.AddGestureRecognizer(tap);
        }
        var x = IPlatformApplication.Current.Services.GetRequiredService<IImageSourceServiceProvider>();
        x.Get
        return result;
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    [Export("gestureRecognizer:shouldReceiveTouch:")]
    public bool ShouldReceiveTouch(UIGestureRecognizer gestureRecognizer, UITouch touch)
    {
        return false;
    }
}
