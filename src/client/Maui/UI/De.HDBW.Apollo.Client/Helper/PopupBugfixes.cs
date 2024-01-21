using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Platform;

namespace De.HDBW.Apollo.Client.Helper
{
    public static class PopupBugFixes
    {

        // https://github.com/CommunityToolkit/Maui/issues/1489
        public static void FixButtonTextLayout(this Popup popup, Button? button)
        {
#if ANDROID
            var view = popup.Content as IView;
            if (button == null || view == null)
            {
                return;
            }

            button.HeightRequest = button.Height;
            button.WidthRequest = button.Width;
            var text = button.Text;
            button.Text = null;
            button.Text = text;

#endif
        }
    }
}
