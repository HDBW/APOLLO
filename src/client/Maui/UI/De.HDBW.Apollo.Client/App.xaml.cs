namespace De.HDBW.Apollo.Client;

using De.HDBW.Apollo.Client.Views;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Services;

public partial class App : Application
{

    public static string ApiEndpoint = "https://fabrikamb2chello.azurewebsites.net/hello";

    public App(
        ExtendedSplashScreenView splashScreenView,
        RegistrationView registrationView,
        UseCaseTutorialView useCaseTutorialView,
        IPreferenceService preferenceService,
        ISessionService sessionService)
    {
        this.InitializeComponent();
        if (preferenceService.GetValue(Preference.IsFirstTime, false))
        {
            this.MainPage = new NavigationPage(splashScreenView);
        }
        else if (!sessionService.HasRegisteredUser)
        {
            this.MainPage = new NavigationPage(registrationView);
        }
        else
        {
            this.MainPage = new NavigationPage(useCaseTutorialView);
        }
    }
}