// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Views;
using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client;
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public MainActivity()
    {
        TouchService = IPlatformApplication.Current?.Services.GetService<ITouchService>();
    }

    public ITouchService? TouchService { get; }

    public override bool DispatchTouchEvent(MotionEvent? e)
    {
        switch (e?.Action)
        {
            case MotionEventActions.Down:
                TouchService?.TouchDownReceived(e.RawX, e.RawY);
                break;
            case MotionEventActions.Up:
                TouchService?.TouchUpReceived(e.RawX, e.RawY);
                break;
        }

        return base.DispatchTouchEvent(e);
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
    }
}
