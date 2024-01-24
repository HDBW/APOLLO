﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Tests.Extensions;
using De.HDBW.Apollo.Data.Tests.Helper;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Services
{
    public abstract class AbstractServiceTestSetup<TU> : IDisposable
    {
        private bool _disposed;

        protected AbstractServiceTestSetup(ITestOutputHelper outputHelper)
        {
            SetupSecrets();
            TokenSource = new CancellationTokenSource();
            Logger = this.SetupLogger<TU>(outputHelper);
            ArgumentException.ThrowIfNullOrWhiteSpace(APIKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(BaseUri);
            var logger = this.SetupLogger<LoggingHttpMessageHandler>(outputHelper);
            var handler = new ContentLoggingHttpMessageHandler(logger);
            Service = SetupService(APIKey, BaseUri, Logger, handler);
            SetupAdditionalServices(APIKey, BaseUri, handler);
        }

        ~AbstractServiceTestSetup()
        {
            Dispose(false);
        }

        protected TU Service { get; private set; }

        protected CancellationTokenSource TokenSource { get; private set; }

        protected ILogger<TU> Logger { get; private set; }

        private string APIKey { get; set; }

        private string BaseUri { get; set; }

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
            var userSecrets = new UserSecretsService();
            Assert.True(userSecrets.LoadSecrets());
            APIKey = userSecrets["SwaggerAPIToken"];
            BaseUri = userSecrets["SwaggerAPIURL"];
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

        protected abstract void SetupAdditionalServices(string apiKey, string baseUri, HttpMessageHandler httpClientHandler);

        protected abstract TU SetupService(string apiKey, string baseUri, ILogger<TU> logger, HttpMessageHandler httpClientHandler);
    }
}