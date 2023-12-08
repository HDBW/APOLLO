// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Tests.Extensions;
using De.HDBW.Apollo.Data.Tests.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Services
{
    public abstract class AbstractServiceTestSetup<TU> : IDisposable
    {
        protected const string BaseUri = "https://apollo-api-hdbw-tst.azurewebsites.net/api";
        private bool _disposed;

        protected AbstractServiceTestSetup(ITestOutputHelper outputHelper)
        {
            SetupSecrets();
            TokenSource = new CancellationTokenSource();
            LogProvider = this.SetupLoggerProvider<TU>(outputHelper);
            ArgumentException.ThrowIfNullOrWhiteSpace(APIKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(BaseUri);
            var logger = this.SetupLogger<LoggingHttpMessageHandler>(outputHelper);
            var handler = new ContentLoggingHttpMessageHandler(logger);
            Service = SetupService(APIKey, BaseUri, LogProvider, handler);
            SetupAdditionalServices(APIKey, BaseUri, LogProvider, handler);
        }

        ~AbstractServiceTestSetup()
        {
            Dispose(false);
        }

        protected TU Service { get; private set; }

        protected CancellationTokenSource? TokenSource { get; private set; }

        protected ILoggerProvider? LogProvider { get; private set; }

        private string? APIKey { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Cleanup();
            }

            _disposed = true;
        }

        protected void SetupSecrets()
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets(GetType().Assembly, false)
            .Build();
            APIKey = configuration?.GetChildren().FirstOrDefault(c => c.Key == "SwaggerAPIToken")?.Value;
        }

        protected void Cleanup()
        {
            APIKey = null;
            TokenSource?.Cancel();
            TokenSource?.Dispose();
            TokenSource = null;
            CleanupAdditionalServices();
        }

        protected abstract void CleanupAdditionalServices();

        protected abstract void SetupAdditionalServices(string apiKey, string baseUri, ILoggerProvider provider, HttpMessageHandler httpClientHandler);

        protected abstract TU SetupService(string apiKey, string baseUri, ILoggerProvider provider, HttpMessageHandler httpClientHandler);

    }
}
