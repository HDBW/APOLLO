using System.Diagnostics;
using Graph.Apollo.Cloud.Common;
using Graph.Apollo.Cloud.Greeter.Services;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Greeter = Graph.Apollo.Cloud.Common.Greeter;

namespace GreeterServiceTests.IntegrationTests
{
    public class MockedGreeterServiceTests : IntegrationTestBase
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            var mockGreeter = new Mock<IGreeter>();
            mockGreeter.Setup(
                m => m.Greet(It.IsAny<string>())).Returns((string s) =>
            {
                if (string.IsNullOrEmpty(s))
                {
                    throw new ArgumentException("Name not provided.");
                }
                return $"Test {s}";
            });

            services.AddSingleton(mockGreeter.Object);
        }

        [Test]
        public async Task SayHelloUnaryTest_MockGreeter_Success()
        {
            // Arrange
            var client = new Greeter.GreeterClient(Channel);

            // Act
            var response = await client.SayHelloUnaryAsync(
                new HelloRequest { Name = "Joe" });

            // Assert
            Debug.Assert(response != null, nameof(response) + " != null");
            Assert.That(response.Message, Is.EqualTo("Test Joe"));
        }

        [Test]
        public async Task SayHelloUnaryTest_MockGreeter_Error()
        {
            // Arrange
            var client = new Greeter.GreeterClient(Channel);

            // Act
            try
            {
                await client.SayHelloUnaryAsync(new HelloRequest { Name = "" });
                Assert.Fail();
            }
            catch (RpcException ex)
            {
                // Assert
                StringAssert.Contains("Name not provided.", ex.Status.Detail);
            }
        }
    }
}
