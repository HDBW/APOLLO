﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Services;

namespace De.HDBW.Apollo.Client
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISessionService _sessionService;
        private readonly IPreferenceService _preferenceService;

        public App(
            IServiceProvider provider,
            IPreferenceService preferenceService,
            ISessionService sessionService,
            INetworkService networkService,
            INavigationService navigationService)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(preferenceService);
            ArgumentNullException.ThrowIfNull(sessionService);
            ArgumentNullException.ThrowIfNull(networkService);
            ArgumentNullException.ThrowIfNull(navigationService);
            _serviceProvider = provider;
            _sessionService = sessionService;
            _preferenceService = preferenceService;
            InitializeComponent();
            if (!_preferenceService.GetValue(Preference.ConfirmedDataUsage, false))
            {
                MainPage = new NavigationPage(Routing.GetOrCreateContent(Routes.ExtendedSplashScreenView, _serviceProvider) as Page);
            }
            else if (!_sessionService.HasRegisteredUser && networkService.HasNetworkConnection)
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
