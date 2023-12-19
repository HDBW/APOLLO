// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Views;
using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client;
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public MainActivity()
    {
        //var logger = IPlatformApplication.Current.Services.GetService<IDialogService>();
    }

    public override bool DispatchTouchEvent(MotionEvent? e)
    {
        return base.DispatchTouchEvent(e);
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
    }

}
