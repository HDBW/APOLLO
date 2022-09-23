namespace De.HDBW.Apollo.Client;

public partial class App : Application
{
    public App(AppShell shell)
    {
        this.InitializeComponent();
        this.MainPage = shell;
    }
}