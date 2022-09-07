﻿using Graph.Apollo.Cloud.Greeter;
using GreeterServiceTests.IntegrationTests.Helpers;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GreeterServiceTests.IntegrationTests;

public class IntegrationTestBase
{
    private GrpcChannel? _channel;
    private IDisposable? _testContext;

    protected GrpcTestFixture<Startup> Fixture { get; private set; } = default!;

    protected ILoggerFactory LoggerFactory => Fixture.LoggerFactory;

    protected GrpcChannel Channel => _channel ??= CreateChannel();

    protected GrpcChannel CreateChannel()
    {
        return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            LoggerFactory = LoggerFactory,
            HttpHandler = Fixture.Handler
        });
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Fixture = new GrpcTestFixture<Startup>(ConfigureServices);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Fixture.Dispose();
    }

    [SetUp]
    public void SetUp()
    {
        _testContext = Fixture.GetTestContext();
    }

    [TearDown]
    public void TearDown()
    {
        _testContext?.Dispose();
        _channel = null;
    }
}
