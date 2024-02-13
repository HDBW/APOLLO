using System.Text.Json;
using De.HDBW.Apollo.Data.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.Lists;
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
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Service.GetAsync(null, null, cts.Token));
            }
        }

        [Fact]
        public async Task GetAsyncTest()
        {
            try
            {
                var types = new Dictionary<Type, Task<List<ApolloListItem>?>>()
                {
                    { typeof(ContactType), GetAndValidateEnumListAsync<ContactType>(TokenSource!.Token) },
                    { typeof(CompletionState), GetAndValidateEnumListAsync<CompletionState>(TokenSource!.Token) },
                    { typeof(DriversLicense), GetAndValidateEnumListAsync<DriversLicense>(TokenSource!.Token) },
                    { typeof(EducationType), GetAndValidateEnumListAsync<EducationType>(TokenSource!.Token) },
                    { typeof(LanguageNiveau), GetAndValidateEnumListAsync<LanguageNiveau>(TokenSource!.Token) },
                    { typeof(RecognitionType), GetAndValidateEnumListAsync<RecognitionType>(TokenSource!.Token) },
                    { typeof(SchoolGraduation), GetAndValidateEnumListAsync<SchoolGraduation>(TokenSource!.Token) },
                    { typeof(ServiceType), GetAndValidateEnumListAsync<ServiceType>(TokenSource!.Token) },
                    { typeof(StaffResponsibility), GetAndValidateEnumListAsync<StaffResponsibility>(TokenSource!.Token) },
                    { typeof(UniversityDegree), GetAndValidateEnumListAsync<UniversityDegree>(TokenSource!.Token) },
                    { typeof(TypeOfSchool), GetAndValidateEnumListAsync<TypeOfSchool>(TokenSource!.Token) },
                    { typeof(VoluntaryServiceType), GetAndValidateEnumListAsync<VoluntaryServiceType>(TokenSource!.Token) },
                    { typeof(Willing), GetAndValidateEnumListAsync<Willing>(TokenSource!.Token) },
                    { typeof(YearRange), GetAndValidateEnumListAsync<YearRange>(TokenSource!.Token) },
                    { typeof(WorkingTimeModel), GetAndValidateEnumListAsync<WorkingTimeModel>(TokenSource!.Token) },
                    { typeof(CareerType), GetAndValidateEnumListAsync<CareerType>(TokenSource!.Token) },
                };

                await Task.WhenAll(types.Values);
                Assert.All(types, (x) => { Assert.True(x.Value.Result != null); });
            }
            catch (ApolloApiException ex)
            {
                Assert.Null($"Error getting List {ex.ErrorCode}");
            }
        }

        protected override ApolloListService SetupService(string apiKey, string baseUri, ILogger<ApolloListService> logger, HttpMessageHandler httpClientHandler)
        {
            return new ApolloListService(logger, baseUri, apiKey, httpClientHandler);
        }

        protected override void CleanupAdditionalServices()
        {
        }

        protected override void SetupAdditionalServices(string apiKey, string baseUri, ILogger<ApolloListService> logger, HttpMessageHandler httpClientHandler)
        {
        }

        private async Task<List<ApolloListItem>?> GetAndValidateEnumListAsync<TU>(CancellationToken token)
            where TU : Enum
        {
            var typeName = typeof(TU).Name;
            var items = await Service.GetAsync(typeof(TU).Name, null, token).ConfigureAwait(false);
            Assert.NotNull(items);
            var names = Enum.GetNames(typeof(TU));
            var returnedNames = items!.Select(x => x.Value).ToList();
            var additionalNames = returnedNames.Where(x => !names.Contains(x)).ToList();
            var missingNames = names.Where(x => !returnedNames.Contains(x)).ToList();
            if (missingNames.Any())
            {
                var missingItems = missingNames.Select(x => JsonSerializer.Serialize(new ApolloListItem() { ListItemId = (int)Enum.Parse(typeof(TU), x), Value = x, Lng = null }));
                Logger.LogError($"List of ItemType {typeof(TU)} is missing specified Values.{Environment.NewLine}The following ListItems are missing:{Environment.NewLine}{string.Join(Environment.NewLine, missingItems)}");
                return null;
            }

            if (additionalNames.Any())
            {
                var additionalItems = items!.Where(x => additionalNames.Contains(x.Value)).Select(x => JsonSerializer.Serialize(x));
                Logger.LogError($"List of ItemType {typeof(TU)} is unknown specified Values.{Environment.NewLine}The following ListItems unknow to the client:{Environment.NewLine}{string.Join(Environment.NewLine, additionalItems)}");
                return null;
            }

            var validItems = new List<ApolloListItem>();
            foreach (var item in items)
            {
                TU enumValue = (TU)Enum.ToObject(typeof(TU), item.ListItemId);
                if (enumValue.ToString() != item.Value)
                {
                    Logger.LogError($"List of ItemType {typeof(TU)} contained differet ListItemId than specified in type.");
                    continue;
                }

                validItems.Add(item);
            }

            return validItems.Count == items.Count ? validItems : null;
        }
    }
}
