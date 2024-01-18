using De.HDBW.Apollo.Data.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Services
{
    public class UserServiceTests : AbstractServiceTestSetup<UserService>
    {
        public UserServiceTests(ITestOutputHelper outputHelper)
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
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Service.SaveAsync(null, cts.Token));
            }
        }

        [Fact]
        public async Task SaveUserAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            User? user = null;
            try
            {
                var testuser = new User() { ObjectId = "Dummy", Name = "Dummy" };
                user = await Service.SaveAsync(testuser, TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode ErrorCodes.TrainingErrors.GetTrainingError;
                Assert.Equal(ErrorCodes.UserErrors.CreateOrUpdateUserError, ex.ErrorCode);
            }

            Assert.NotNull(user);
        }

        protected override UserService SetupService(string apiKey, string baseUri, ILogger<UserService> logger, HttpMessageHandler httpClientHandler)
        {
            return new UserService(logger, baseUri, apiKey, httpClientHandler);
        }

        protected override void CleanupAdditionalServices()
        {
        }

        protected override void SetupAdditionalServices(string apiKey, string baseUri, HttpMessageHandler httpClientHandler)
        {
        }
    }
}
