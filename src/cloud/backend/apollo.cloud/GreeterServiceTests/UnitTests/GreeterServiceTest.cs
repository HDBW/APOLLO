using Graph.Apollo.Cloud.Common;
using Graph.Apollo.Cloud.Greeter.Services;
using GreeterServiceTests.UnitTests.Helpers;
using Moq;
using NUnit.Framework;

namespace GreeterServiceTests.UnitTests;

[TestFixture]
public class GreeterServiceTest
{
    //TODO: AddLogger Test current call is for IGreeter constructor. prerequisite test telemetry first

    [Test]
    public async Task SayHelloUnaryTest()
    {
        // Arrange
        var mockGreeter = CreateGreeterMock();
        var service = new GreeterService(mockGreeter.Object);

        // Act
        var response = await service.SayHelloUnary(new HelloRequest { Name = "Joe" }, TestServerCallContext.Create());

        // Assert
        mockGreeter.Verify(v => v.Greet("Joe"));
        Assert.AreEqual("Hello Joe", response.Message);
    }

    [Test]
    public async Task SayHelloServerStreamingTest()
    {
        // Arrange
        var service = new GreeterService(CreateGreeterMock().Object);

        var cts = new CancellationTokenSource();
        var callContext = TestServerCallContext.Create(cancellationToken: cts.Token);
        var responseStream = new TestServerStreamWriter<HelloReply>(callContext);

        // Act
        using var call = service.SayHelloServerStreaming(new HelloRequest { Name = "Joe" }, responseStream, callContext);

        // Assert
        Assert.IsFalse(call.IsCompletedSuccessfully, "Method should run until cancelled.");

        cts.Cancel();

        await call;
        responseStream.Complete();

        var allMessages = new List<HelloReply>();
        await foreach (var message in responseStream.ReadAllAsync())
        {
            allMessages.Add(message);
        }

        Assert.GreaterOrEqual(allMessages.Count, 1);

        Assert.AreEqual("Hello Joe 1", allMessages[0].Message);
    }

    [Test]
    public async Task SayHelloClientStreamingTest()
    {
        // Arrange
        var service = new GreeterService(CreateGreeterMock().Object);

        var callContext = TestServerCallContext.Create();
        var requestStream = new TestAsyncStreamReader<HelloRequest>(callContext);

        // Act
        using var call = service.SayHelloClientStreaming(requestStream, callContext);

        requestStream.AddMessage(new HelloRequest { Name = "James" });
        requestStream.AddMessage(new HelloRequest { Name = "Jo" });
        requestStream.AddMessage(new HelloRequest { Name = "Lee" });
        requestStream.Complete();

        // Assert
        var response = await call;
        Assert.AreEqual("Hello James, Jo, Lee", response.Message);
    }

    [Test]
    public async Task SayHelloBidirectionStreamingTest()
    {
        // Arrange
        var service = new GreeterService(CreateGreeterMock().Object);

        var callContext = TestServerCallContext.Create();
        var requestStream = new TestAsyncStreamReader<HelloRequest>(callContext);
        var responseStream = new TestServerStreamWriter<HelloReply>(callContext);

        // Act
        using var call = service.SayHelloBidirectionalStreaming(requestStream, responseStream, callContext);

        // Assert
        requestStream.AddMessage(new HelloRequest { Name = "James" });
        Assert.AreEqual("Hello James", (await responseStream.ReadNextAsync())!.Message);

        requestStream.AddMessage(new HelloRequest { Name = "Jo" });
        Assert.AreEqual("Hello Jo", (await responseStream.ReadNextAsync())!.Message);

        requestStream.AddMessage(new HelloRequest { Name = "Lee" });
        Assert.AreEqual("Hello Lee", (await responseStream.ReadNextAsync())!.Message);

        requestStream.Complete();

        await call;
        responseStream.Complete();

        Assert.IsNull(await responseStream.ReadNextAsync());
    }

    private static Mock<IGreeter> CreateGreeterMock()
    {
        var mockGreeter = new Mock<IGreeter>();
        mockGreeter.Setup(m => m.Greet(It.IsAny<string>())).Returns((string s) => $"Hello {s}");
        return mockGreeter;
    }


}