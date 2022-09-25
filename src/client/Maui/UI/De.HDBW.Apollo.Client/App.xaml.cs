namespace De.HDBW.Apollo.Client;

using De.HDBW.Apollo.Client.Views;

public partial class App : Application
{
    public App(AppShell shell, ExtendedSplashScreenView splashScreenView)
    {
        this.InitializeComponent();
        this.MainPage = new NavigationPage(splashScreenView);
    }
}