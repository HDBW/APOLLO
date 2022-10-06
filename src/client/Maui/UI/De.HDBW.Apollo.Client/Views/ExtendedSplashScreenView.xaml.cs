using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;
public partial class ExtendedSplashScreenView
{
    public ExtendedSplashScreenView(ExtendedSplashScreenViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public ExtendedSplashScreenViewModel? Viemodel
    {
        get
        {
            return BindingContext as ExtendedSplashScreenViewModel;
        }
    }
}