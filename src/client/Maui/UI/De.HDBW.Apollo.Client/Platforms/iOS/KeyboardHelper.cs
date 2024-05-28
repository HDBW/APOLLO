﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.Maui.Platform;
using UIKit;

namespace De.HDBW.Apollo.Client.Platforms
{
    public static class KeyboardHelper
    {
        public static void HideKeyboard(object? handler)
        {
            // UIApplication.SharedApplication?.KeyWindow?.EndEditing(true);
            try
            {
                KeyboardAutoManagerScroll.Disconnect();
                UIApplication.SharedApplication?
                .ConnectedScenes?
                .OfType<UIWindowScene>()?
                .LastOrDefault(x => x.ActivationState == UISceneActivationState.ForegroundActive)?
                .Windows?
                .LastOrDefault(w => w.IsKeyWindow)?
                .EndEditing(true);
                KeyboardAutoManagerScroll.Connect();
            }
            catch
            {
            }
        }
    }
}
