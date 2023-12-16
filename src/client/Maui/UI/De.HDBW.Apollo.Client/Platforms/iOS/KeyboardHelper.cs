// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using Microsoft.Maui.Controls.PlatformConfiguration;
using UIKit;

namespace De.HDBW.Apollo.Client.Platforms
{
    public static class KeyboardHelper
    {
        public static void HideKeyboard(object? view)
        {
            UIApplication.SharedApplication?.KeyWindow?.EndEditing(true);
        }
    }
}
