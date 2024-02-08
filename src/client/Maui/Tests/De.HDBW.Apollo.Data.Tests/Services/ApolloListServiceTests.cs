using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.Lists;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Services
{
    public class ApolloListServiceTests : AbstractServiceTestSetup<ApolloListService>
    {
        public ApolloListServiceTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public async Task CancellationTokenTests()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(TokenSource!.Token))
            {
                cts.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Service.GetAsync(null, null, null, cts.Token));
            }
        }

        [Fact]
        public async Task GetAsyncAsyncTest()
        {
            try
            {
                var items = await GetAndValidateEnumListAsync<CareerType>(TokenSource!.Token).ConfigureAwait(false);
            }
            catch (ApolloApiException ex)
            {
                Assert.Null($"Error getting List {ex.ErrorCode}");
            }
        }

        private async Task<List<ApolloListItem>?> GetAndValidateEnumListAsync<TU>(CancellationToken token)
            where TU : Enum
        {
            var typeName = typeof(TU).Name;
            var items = await Service.GetAsync(typeof(TU).Name, null, null, token).ConfigureAwait(false);
            Assert.NotNull(items);
            Assert.Equal(typeName, items!.ItemType);
            var names = Enum.GetNames(typeof(TU));
            if (names.Count() != items.Items.Count())
            {
                Logger.LogError($"List of ItemType {items.ItemType} contained unspecified data.");
                return null;
            }

            var validItems = new List<ApolloListItem>();
            foreach (var item in items!.Items)
            {
                if (!names.Contains(item.Value))
                {
                    Logger.LogError($"List of ItemType {items.ItemType} contained unspecified value.");
                    continue;
                }

                var enumValue = (TU)Enum.ToObject(typeof(TU), item.ListItemId);
                if (enumValue.ToString() != item.Value)
                {
                    Logger.LogError($"List of ItemType {items.ItemType} contained wrong in ListItemId.");
                    continue;
                }

                validItems.Add(item);
            }

            return validItems;
        }

        protected override ApolloListService SetupService(string apiKey, string baseUri, ILogger<ApolloListService> logger, HttpMessageHandler httpClientHandler)
        {
            return new ApolloListService(logger, baseUri, apiKey, httpClientHandler);
        }

        protected override void CleanupAdditionalServices()
        {
        }

        protected override void SetupAdditionalServices(string apiKey, string baseUri, HttpMessageHandler httpClientHandler)
        {
        }
    }
}
