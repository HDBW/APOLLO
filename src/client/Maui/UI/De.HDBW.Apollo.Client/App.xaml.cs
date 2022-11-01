// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Views;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Services;

namespace De.HDBW.Apollo.Client;
public partial class App : Application
{
    public App(
        ExtendedSplashScreenView splashScreenView,
        RegistrationView registrationView,
        UseCaseTutorialView useCaseTutorialView,
        IPreferenceService preferenceService,
        ISessionService sessionService)
    {
        InitializeComponent();

        if (preferenceService.GetValue(Preference.IsFirstTime, false))
        {
            MainPage = new NavigationPage(splashScreenView);
        }
        else if (!sessionService.HasRegisteredUser)
        {
            MainPage = new NavigationPage(registrationView);
        }
        else
        {
            MainPage = new NavigationPage(useCaseTutorialView);
        }
    }
}
