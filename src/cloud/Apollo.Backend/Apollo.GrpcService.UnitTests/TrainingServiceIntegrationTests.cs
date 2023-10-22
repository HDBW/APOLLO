// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using Apollo.Services.Grpc;
using Grpc.Net.Client;
using static Apollo.Services.Grpc.TrainingService;

namespace Apollo.GrpcService.UnitTests
{
    [TestClass]
    public class TrainingServiceIntegrationTests
    {
        private const string testId1 = "testtraining-01";

     
        private static TrainingServiceClient GetClient()
        {
            var channel = Helpers.GetChannel();
            return new TrainingServiceClient(channel);
        }

        [TestMethod]
        public async Task GetTrainingTest()
        {
            var client = GetClient();
            var reply = await client.GetTrainingAsync(new GetTrainingsRequest { Id = testId1 });
        }

        [TestMethod]
        public async Task QueryTrainingTest()
        {
            var client = GetClient();
            var reply = await client.QueryTrainingsAsync(new QueryTrainingsRequest { Contains = "testtraining"});
        }

        [TestMethod]
        public async Task AddTrainingTest()
        {
            var client = GetClient();
            var reply = await client.CreateOrUpdateTrainingAsync(new CreateOrUpdateRequest  { });

        }

        [TestMethod]
        public async Task DeleteTrainingTest()
        {
            var client = GetClient();
            var reply = await client.DeleteTrainingsAsync(new DeleteTrainingsRequest  { });
        }
    }
}
