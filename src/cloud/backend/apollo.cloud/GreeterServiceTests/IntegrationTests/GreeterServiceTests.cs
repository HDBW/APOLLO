using Graph.Apollo.Cloud.Common;
using Grpc.Core;

namespace GreeterServiceTests.IntegrationTests;

[TestFixture]
public class GreeterServiceTests : IntegrationTestBase
{
    //TODO: TEST Healthprobe for service see https://docs.microsoft.com/en-us/aspnet/core/grpc/health-checks?view=aspnetcore-6.0

    [Test]
    public async Task SayHelloUnaryTest()
    {
        // Arrange
        var client = new Greeter.GreeterClient(Channel);
        const string name = "Joe";

        // Act
        var response = await client.SayHelloUnaryAsync(new HelloRequest { Name = name });

        // Assert
        Assert.That(response.Message, Is.EqualTo($"Hello {name}"));
    }

    [Test]
    public async Task SayHelloClientStreamingTest()
    {
        // Arrange
        var client = new Greeter.GreeterClient(Channel);

        var names = new[] { "James", "Jo", "Lee" };
        HelloReply response;

        // Act
        using var call = client.SayHelloClientStreaming();
        foreach (var name in names)
        {
            await call.RequestStream.WriteAsync(new HelloRequest { Name = name });
        }
        await call.RequestStream.CompleteAsync();

        response = await call;

        // Assert
        Assert.That(response.Message, Is.EqualTo("Hello James, Jo, Lee"));
    }

    [Test]
    public async Task SayHelloServerStreamingTest()
    {
        // Arrange
        var client = new Greeter.GreeterClient(Channel);

        var cts = new CancellationTokenSource();
        var hasMessages = false;
        var callCancelled = false;
        const string name = "Joe";

        // Act
        using var call = client.SayHelloServerStreaming(new HelloRequest { Name = name }, cancellationToken: cts.Token);
        try
        {
            await foreach (var message in call.ResponseStream.ReadAllAsync(cts.Token))
            {
                hasMessages = true;
                cts.Cancel();
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        {
            callCancelled = true;
        }

        // Assert
        Assert.IsTrue(hasMessages);
        Assert.IsTrue(callCancelled);
    }

    [Test]
    public async Task SayHelloBidirectionStreamingTest()
    {
        // Arrange
        var client = new Greeter.GreeterClient(Channel);

        var names = new[] { "James", "Jo", "Lee" };
        var messages = new List<string>();

        // Act
        using var call = client.SayHelloBidirectionalStreaming();
        foreach (var name in names)
        {
            await call.RequestStream.WriteAsync(new HelloRequest { Name = name });

            Assert.IsTrue(await call.ResponseStream.MoveNext());
            messages.Add(call.ResponseStream.Current.Message);
        }

        await call.RequestStream.CompleteAsync();

        // Assert
        Assert.That(messages.Count, Is.EqualTo(3));
        Assert.That(messages[0], Is.EqualTo("Hello James"));
    }
}