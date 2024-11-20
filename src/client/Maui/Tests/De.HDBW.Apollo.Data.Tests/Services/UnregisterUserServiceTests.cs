// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.Data.Tests.Extensions;
using De.HDBW.Apollo.Data.Tests.Helper;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Services
{
    public class UnregisterUserServiceTests : AbstractServiceTestSetup<UnregisterUserService>
    {
        private IServiceProvider? _serviceProvider;

        public UnregisterUserServiceTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void UpdateAuthorizationHeader()
        {
            Assert.NotNull(Service);
            Assert.NotNull(_serviceProvider);
            var header = "MockHeader";
            var description = typeof(AbstractSwaggerServiceBase).GetProperty("HttpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            AuthenticatedServiceHelper.UpdateAuthorizationHeader(_serviceProvider!, null);
            foreach (var serviceType in AuthenticatedServiceHelper.AuthenticatedServices)
            {
                var service = _serviceProvider!.GetService(serviceType) as AbstractSwaggerServiceBase;
                Assert.NotNull(service);
                var httpClient = description!.GetValue(service) as HttpClient;
                Assert.NotNull(httpClient);
                Assert.False(httpClient!.DefaultRequestHeaders.Contains("Authorization"));
            }

            AuthenticatedServiceHelper.UpdateAuthorizationHeader(_serviceProvider!, header);
            Assert.True(AuthenticatedServiceHelper.AuthenticatedServices.Any());
            foreach (var serviceType in AuthenticatedServiceHelper.AuthenticatedServices)
            {
                var service = _serviceProvider!.GetService(serviceType) as AbstractSwaggerServiceBase;
                Assert.NotNull(service);
                var httpClient = description!.GetValue(service) as HttpClient;
                Assert.NotNull(httpClient);
                Assert.True(httpClient!.DefaultRequestHeaders.Contains("Authorization"));
                Assert.Equal(header, httpClient.DefaultRequestHeaders.GetValues("Authorization").First());
            }

            AuthenticatedServiceHelper.UpdateAuthorizationHeader(_serviceProvider!, null);
            foreach (var serviceType in AuthenticatedServiceHelper.AuthenticatedServices)
            {
                var service = _serviceProvider!.GetService(serviceType) as AbstractSwaggerServiceBase;
                Assert.NotNull(service);
                var httpClient = description!.GetValue(service) as HttpClient;
                Assert.NotNull(httpClient);
                Assert.False(httpClient!.DefaultRequestHeaders.Contains("Authorization"));
            }
        }

        protected override void SetupAdditionalServices(string apiKey, string baseUri, ILogger<UnregisterUserService> logger, HttpMessageHandler httpClientHandler)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IUnregisterUserService>((serviceProvider) =>
            {
                return Service;
            });

            serviceCollection.AddSingleton<IApolloListService>((serviceProvider) =>
            {
                return new ApolloListService(this.SetupLogger<ApolloListService>(OutputHelper), baseUri, apiKey, httpClientHandler);
            });

            serviceCollection.AddSingleton<IProfileService>((serviceProvider) =>
            {
                return new ProfileService(this.SetupLogger<ProfileService>(OutputHelper), baseUri, apiKey, httpClientHandler);
            });

            serviceCollection.AddSingleton<IUserService>((serviceProvider) =>
            {
                return new UserService(this.SetupLogger<UserService>(OutputHelper), baseUri, apiKey, serviceProvider.GetService<IProfileService>(), httpClientHandler);
            });

            _serviceProvider = serviceCollection.BuildServiceProvider();
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
