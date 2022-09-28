namespace De.HDBW.Apollo.Client.Views;

using De.HDBW.Apollo.Client.ViewModels;

public partial class ExtendedSplashScreenView
{
    public ExtendedSplashScreenView(ExtendedSplashScreenViewModel model)
    {
        this.InitializeComponent();
        this.BindingContext = model;
    }

    public ExtendedSplashScreenViewModel? Viemodel
    {
        get
        {
            return this.BindingContext as ExtendedSplashScreenViewModel;
        }
    }
}