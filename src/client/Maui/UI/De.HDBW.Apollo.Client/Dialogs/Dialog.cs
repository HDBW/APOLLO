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
            Size = new Size(Application.Current!.MainPage!.Width, Application.Current!.MainPage!.Height - 40);
#endif
        }

        public void HandleRootSizeChanged(object sender, System.EventArgs e)
        {
            var view = sender as View;
            if (view != null)
            {
#if IOS
                Size = new Size(Application.Current!.MainPage!.Width - 40, view.DesiredSize.Height);
#elif ANDROID
                if (Content != null)
                {
                    if (Content.DesiredSize.IsZero)
                    {
                        view.InvalidateMeasureNonVirtual(Microsoft.Maui.Controls.Internals.InvalidationTrigger.MeasureChanged);
                    }

                    //var size = Content.Measure(Application.Current!.MainPage!.Width - 40, Application.Current!.MainPage!.Height - 40, MeasureFlags.IncludeMargins);
                    Size = new Size(Application.Current!.MainPage!.Width, view.DesiredSize.Height);
                }
#endif
            }
        }

        public void OnSizeChanged(object sender, System.EventArgs e)
        {
#if iOS
            return;   
#elif ANDROID
            var button = sender as Button;
            this.FixButtonTextLayout(button);
#endif
        }
    }
}
