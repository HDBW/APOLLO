// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using Apollo.Services.Grpc;
using Grpc.Net.Client;
using static Apollo.Services.Grpc.TrainingService;

namespace Apollo.GrpcService.UnitTests
{
    [TestClass]
    public class TrainingTests
    {
        private const string testId1 = "testtraining-01";

        private static GrpcChannel GetChannel()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7064");
            return channel;
        }

        private static TrainingServiceClient GetCliant()
        {
            var channel = GetChannel();
            return new TrainingServiceClient(channel);
        }

        [TestMethod]
        public async Task GetTrainingTest()
        {
            var client = GetCliant();
            var reply = await client.GetTrainingAsync(new GetTrainingsRequest { Id = testId1 });
        }

        [TestMethod]
        public async Task QueryTrainingTest()
        {
            var client = GetCliant();
            var reply = await client.QueryTrainingsAsync(new QueryTrainingsRequest { Contains = "testtraining"});
        }

        [TestMethod]
        public async Task AddTrainingTest()
        {
            var channel = GetChannel();
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });
        }

        [TestMethod]
        public async Task DeleteTrainingTest()
        {
            var channel = GetChannel();
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });
        }
    }
}
