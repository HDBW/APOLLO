using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.Data.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
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
        public async Task RegisterUserAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            string? userId = null;
            var testuser = new User() { ObjectId = "NewUser", Name = "NewUser" };
            User? createdUser = null;
            User? updatedUser = null;
            try
            {
                userId = await Service.SaveAsync(testuser, TokenSource!.Token);
                Assert.NotNull(userId);
                createdUser = await Service.GetUserAsync(userId!, TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode ErrorCodes.TrainingErrors.GetTrainingError;
                Assert.Equal(ErrorCodes.UserErrors.CreateOrUpdateUserError, ex.ErrorCode);
            }

            Assert.NotNull(userId);
            Assert.NotNull(createdUser);
            Assert.Equal(testuser.ObjectId, createdUser!.ObjectId);
            Assert.Equal(testuser.Name, createdUser.Name);

            testuser.Id = userId;
            testuser.Name = "Fritz";
            try
            {
                var updatedId = await Service.SaveAsync(testuser, TokenSource!.Token);
                updatedUser = await Service.GetUserAsync(userId!, TokenSource!.Token);
                createdUser = await Service.GetUserAsync(userId!, TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode ErrorCodes.TrainingErrors.GetTrainingError;
                Assert.Equal(ErrorCodes.UserErrors.CreateOrUpdateUserError, ex.ErrorCode);
            }

            Assert.NotNull(createdUser);
            Assert.NotNull(updatedUser);
            Assert.Equal(testuser.ObjectId, updatedUser!.ObjectId);
            Assert.Equal(testuser.Name, createdUser!.Name);
        }

        [Fact]
        public async Task GetUserAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            string userId = "User-5DE545AEF9974FD6826151725A9961F8";
            User? user = null;
            try
            {
                user = await Service.GetUserAsync(userId, TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode ErrorCodes.TrainingErrors.GetTrainingError;
                Assert.Equal(ErrorCodes.UserErrors.CreateOrUpdateUserError, ex.ErrorCode);
            }

            Assert.NotNull(user);
            Assert.Equal(userId, user!.Id);
        }

        [Fact]
        public async Task SaveUserAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            string? userId = null;
            try
            {
                var testuser = new User() { Id = "SER01", ObjectId = "Dummy", Name = "Dummy", IdentityProvicer = "Dummy" };
                userId = await Service.SaveAsync(testuser, TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode ErrorCodes.TrainingErrors.GetTrainingError;
                Assert.Equal(ErrorCodes.UserErrors.CreateOrUpdateUserError, ex.ErrorCode);
            }

            Assert.NotNull(userId);
        }

        [Fact]
        public async Task SaveProfileAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            string userId = "User-5DE545AEF9974FD6826151725A9961F8";
            User? user = null;
            try
            {
                user = await Service.GetUserAsync(userId, TokenSource!.Token);
                Assert.NotNull(user);
                var profile = user?.Profile ?? new Profile();
                user!.Profile = profile;
                var savedUserId = await Service.SaveAsync(user, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
                user = await Service.GetUserAsync(userId, TokenSource!.Token);

                Assert.NotNull(user);
                Assert.Equal(userId, user!.Id);
                Assert.NotNull(user.Profile);
                //Assert.False(string.IsNullOrWhiteSpace(user!.Profile!.Id));

                // Create a c
                user.Profile!.CareerInfos = user.Profile!.CareerInfos ?? new List<CareerInfo>();
                var careerInfo = new CareerInfo();
                careerInfo.CareerType = CareerType.Homemaker.ToApolloListItem()!;
                user.Profile!.CareerInfos.Add(careerInfo);

                savedUserId = await Service.SaveAsync(user, TokenSource!.Token);
                Assert.Equal(userId, savedUserId);
                user = await Service.GetUserAsync(userId, TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode ErrorCodes.TrainingErrors.GetTrainingError;
                Assert.Equal(ErrorCodes.UserErrors.CreateOrUpdateUserError, ex.ErrorCode);
            }
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
