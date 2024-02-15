// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using UIKit;

namespace De.HDBW.Apollo.Client.Platforms
{
    public static class KeyboardHelper
    {
        public static void HideKeyboard(object? handler)
        {
            // UIApplication.SharedApplication?.KeyWindow?.EndEditing(true);
            UIApplication.SharedApplication?
                .ConnectedScenes?
                .OfType<UIWindowScene>()?
                .LastOrDefault(x => x.ActivationState == UISceneActivationState.ForegroundActive)?
                .Windows?
                .LastOrDefault(w => w.IsKeyWindow)?
                .EndEditing(true);
        }
    }
}
