// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.Data.Tests.Extensions;
using De.HDBW.Apollo.Data.Tests.Helper;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Xunit;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Services
{
    public class UnregisterUserServiceTests : AbstractServiceTestSetup<UnregisterUserService>
    {
        private IServiceProvider? _serviceProvider;
        private ServiceCollection? _serviceCollection;

        public UnregisterUserServiceTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void UpdateAuthorizationHeader()
        {
            Assert.NotNull(Service);
            Assert.NotNull(_serviceProvider);
            Assert.NotNull(_serviceCollection);

            var authentication = new AuthenticationResult(
                         accessToken: "Mock",
                         isExtendedLifeTimeToken: true,
                         uniqueId: "Mock",
                         expiresOn: DateTimeOffset.MaxValue,
                         extendedExpiresOn: DateTimeOffset.MaxValue,
                         tenantId: "Mock",
                         account: new Data.Mock.DummyAccount(),
                         idToken: "Mock",
                         scopes: new List<string>(),
                         correlationId: Guid.Empty,
                         tokenType: "Bearer",
                         authenticationResultMetadata: null,
                         claimsPrincipal: null,
                         spaAuthCode: null,
                         additionalResponseParameters: null);

            var authenticatedServices = AuthenticatedServiceHelper.GetAuthenticatedServices();
            Assert.NotEmpty(authenticatedServices);

            Assert.Contains(authenticatedServices, x => x.InterfaceType == typeof(IUnregisterUserService));
            Assert.Contains(authenticatedServices, x => x.InterfaceType == typeof(IApolloListService));
            Assert.Contains(authenticatedServices, x => x.InterfaceType == typeof(IProfileService));
            Assert.Contains(authenticatedServices, x => x.InterfaceType == typeof(IUserService));
            Assert.DoesNotContain(authenticatedServices, x => x.InterfaceType == typeof(IOccupationService));

            AuthenticatedServiceHelper.UpdateAuthorizationHeader(_serviceProvider!, authenticatedServices, authentication?.CreateAuthorizationHeader() !);
        }

        protected override void SetupAdditionalServices(string apiKey, string baseUri, ILogger<UnregisterUserService> logger, HttpMessageHandler httpClientHandler)
        {
            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddSingleton<IUnregisterUserService>((serviceProvider) =>
            {
                return Service;
            });

            _serviceCollection.AddSingleton<IApolloListService>((serviceProvider) =>
            {
                return new ApolloListService(this.SetupLogger<ApolloListService>(OutputHelper), baseUri, apiKey, httpClientHandler);
            });

            _serviceCollection.AddSingleton<IProfileService>((serviceProvider) =>
            {
                return new ProfileService(this.SetupLogger<ProfileService>(OutputHelper), baseUri, apiKey, httpClientHandler);
            });

            _serviceCollection.AddSingleton<IUserService>((serviceProvider) =>
            {
                return new UserService(this.SetupLogger<UserService>(OutputHelper), baseUri, apiKey, serviceProvider.GetService<IProfileService>(), httpClientHandler);
            });

            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }

        protected override UnregisterUserService SetupService(string apiKey, string baseUri, ILogger<UnregisterUserService> logger, HttpMessageHandler httpClientHandler)
        {
            return new UnregisterUserService(this.SetupLogger<UnregisterUserService>(OutputHelper), baseUri, apiKey, httpClientHandler);
        }

        protected override void CleanupAdditionalServices()
        {
        }
    }
}
