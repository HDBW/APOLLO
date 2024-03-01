// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views.InputMethods;

namespace De.HDBW.Apollo.Client.Platforms
{
    public static class KeyboardHelper
    {
        public static void HideKeyboard(object? view)
        {
            try
            {
                var context = Android.App.Application.Context;
                var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
                if (inputMethodManager == null)
                {
                    return;
                }

                var activity = context as Activity;
                var androidView = view as Android.Views.View;
                IBinder? token = null;
                if (activity != null)
                {
                    token = activity.CurrentFocus?.WindowToken;
                }
                else if (androidView != null)
                {
                    token = androidView.WindowToken;
                }

                if (token == null)
                {
                    return;
                }

                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
                activity?.Window?.DecorView.ClearFocus();
                androidView?.ClearFocus();
            }
            catch
            {
            }
        }
    }
}
