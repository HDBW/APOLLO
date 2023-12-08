// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Tests.Remote;
using Microsoft.Extensions.Logging;
using Moq;

namespace De.HDBW.Apollo.Data.Tests.Services
{
    public abstract class AbstractServiceTestSetup<TU>
    {
        protected const string BaseUri = "https://apollo-api-hdbw-tst.azurewebsites.net/api";

        protected TU? Service { get; private set; }

        protected HttpClient? Client { get; private set; }

        protected CancellationTokenSource? TokenSource { get; private set; }

        protected ILoggerProvider? LogProvider { get; private set; }

        protected void SetupDefaults()
        {
            TokenSource = new CancellationTokenSource();
            LogProvider = GetLogProvider();
            var handler = GetHttpClientHandler();
            Service = SetupService(LogProvider, handler);
            SetupAdditionalServices(LogProvider, handler);
        }

        protected void Cleanup()
        {
            TokenSource?.Cancel();
            TokenSource?.Dispose();
            TokenSource = null;
            CleanupAdditionalServices();
        }

        protected abstract void CleanupAdditionalServices();

        protected abstract void SetupAdditionalServices(ILoggerProvider provider, HttpMessageHandler httpClientHandler);

        protected abstract TU SetupService(ILoggerProvider provider, HttpMessageHandler httpClientHandler);

        private HttpMessageHandler GetHttpClientHandler()
        {
            return new FakeResponseClientHandler();
        }

        private ILoggerProvider GetLogProvider()
        {
            var loggerMock = new Mock<ILogger>();

            var mock = new Mock<ILoggerProvider>();
            mock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);
            return mock.Object;
        }
    }
}
