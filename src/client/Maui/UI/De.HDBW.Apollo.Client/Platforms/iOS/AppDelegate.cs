// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Foundation;
using Microsoft.Identity.Client;
using UIKit;

namespace De.HDBW.Apollo.Client;
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
    {
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
        return base.OpenUrl(app, url, options);
    }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // https://stackoverflow.com/questions/26931083/kvo-of-contentsize-in-c-sharp
        AddObserver("UITouchPhaseBegan", NSKeyValueObservingOptions.OldNew, OnTouch);
        return base.FinishedLaunching(application, launchOptions);
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    private void OnTouch(NSObservedChange change)
    {
    }
}
