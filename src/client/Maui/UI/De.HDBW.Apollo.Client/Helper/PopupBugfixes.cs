﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Maui.Views;

namespace De.HDBW.Apollo.Client.Helper
{
    public static class PopupBugFixes
    {
        // https://github.com/CommunityToolkit/Maui/issues/1489
        public static void FixButtonTextLayout(this Popup popup, Button? button)
        {
#if ANDROID
            var view = popup.Content as IView;
            if (button == null || view == null || button.DesiredSize == Size.Zero)
            {
                return;
            }

            button.HeightRequest = button.DesiredSize.Height;
            button.WidthRequest = button.DesiredSize.Width;
            var text = button.Text;
            button.Text = null;
            button.Text = text;

#endif
        }
    }
}
