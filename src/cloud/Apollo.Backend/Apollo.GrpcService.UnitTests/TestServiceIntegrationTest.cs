// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Apollo.Services.Grpc;
using Grpc.Net.Client;

namespace Apollo.GrpcService.UnitTests
{
    [TestCategory("Grpc")]
    [TestClass]
    public class TestServiceIntegrationTest
    {
      
        [TestMethod]
        public async Task GreateTest()
        {
            using var channel = Helpers.GetChannel();
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(new HelloRequest { Name = "me" });

            Assert.IsTrue(reply.Message.Contains(" me"));
        }

     
    }
}
