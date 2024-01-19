// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Services;

namespace De.HDBW.Apollo.Client
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        private ISessionService _sessionService;
        private IPreferenceService _preferenceService;

        public App(
            IServiceProvider provider,
            IPreferenceService preferenceService,
            ISessionService sessionService,
            INavigationService navigationService)
        {
            _serviceProvider = provider;
            _sessionService = sessionService;
            _preferenceService = preferenceService;
            InitializeComponent();
            _preferenceService.SetValue(Preference.IsFirstTime, true);
            if (_preferenceService.GetValue(Preference.IsFirstTime, true))
            {
                _preferenceService.SetValue(Preference.IsFirstTime, false);
                MainPage = new NavigationPage(Routing.GetOrCreateContent(Routes.ExtendedSplashScreenView, _serviceProvider) as Page);
            }
            else if (!_sessionService.HasRegisteredUser)
            {
                MainPage = new NavigationPage(Routing.GetOrCreateContent(Routes.RegistrationView, _serviceProvider) as Page);
            }
            else
            {
                navigationService.PushToRootAsync(Routes.Shell, CancellationToken.None);
            }
        }
    }
}
