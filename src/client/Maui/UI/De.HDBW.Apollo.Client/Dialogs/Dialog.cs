using CommunityToolkit.Maui.Views;
using De.HDBW.Apollo.Client.Helper;

namespace De.HDBW.Apollo.Client.Dialogs
{
    public partial class Dialog : Popup
    {
        public Dialog()
            : base()
        {
#if IOS
            Size = new Size(Application.Current!.MainPage!.Width - 40, Application.Current!.MainPage!.Height - 40);
#elif ANDROID
            //Size = new Size(Application.Current!.MainPage!.Width - 40, 0);
#endif
        }

        public void HandleRootSizeChanged(object sender, System.EventArgs e)
        {
            var view = sender as View;
            if (view != null)
            {
                Size = new Size(Application.Current!.MainPage!.Width - 40, view.DesiredSize.Height);
            }
        }

        public void OnSizeChanged(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            if (!(button?.IsLoaded ?? false) || Size.Width == 0 || button.Width != 0)
            {
                return;
            }

            this.FixButtonTextLayout(button);
        }
    }
}