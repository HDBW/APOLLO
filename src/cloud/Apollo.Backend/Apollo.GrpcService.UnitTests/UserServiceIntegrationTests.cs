// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using Apollo.Services.Grpc;
using Grpc.Net.Client;
using static Apollo.Services.Grpc.TrainingService;
using static Apollo.Services.Grpc.UserService;

namespace Apollo.GrpcService.UnitTests
{
    [TestClass]
    public class UserServiceIntegrationTests
    {
        private const string testId1 = "testuser-01";

    

        [TestMethod]
        public async Task GetUserTest()
        {
            var client = Helpers.GetClient();
            var reply = await client.GetUserAsync(new GetUserRequest { Id = testId1 });
        }

        [TestMethod]
        public async Task QueryUserTest()
        {
            var client = Helpers.GetClient();
            var reply = await client.QueryUsersAsync(new QueryUsersRequest { Contains = "testtraining"});
        }

        [TestMethod]
        public async Task AddOrUpdateUserTest()
        {
            var client = Helpers.GetClient();
            var reply = await client.CreateOrUpdateUserAsync(new CreateOrUpdateUserRequest  { });

        }

        [TestMethod]
        public async Task DeleteUsersTest()
        {
            var client = Helpers.GetClient();
            var reply = await client.DeleteUsersAsync(new DeleteUsersRequest  { });
        }
    }
}
