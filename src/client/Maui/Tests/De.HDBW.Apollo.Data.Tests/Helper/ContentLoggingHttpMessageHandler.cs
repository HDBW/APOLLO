// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Tests.Helper
{
    internal sealed class ContentLoggingHttpMessageHandler : LoggingHttpMessageHandler
    {
        public ContentLoggingHttpMessageHandler(ILogger logger)
            : base(logger)
        {
            Logger = logger;
            InnerHandler = new WrappingHttpClientHandler(logger);
        }

        private ILogger Logger { get; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InnerHandler?.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.TryAddWithoutValidation("Request-Id", Guid.NewGuid().ToString());
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var content = await (response.Content?.ReadAsStringAsync(cancellationToken) ?? Task.FromResult("NULL"));
            Logger.LogDebug($"Response: {content}");
            return response;
        }

        internal sealed class WrappingHttpClientHandler : HttpClientHandler
        {
            public WrappingHttpClientHandler(ILogger logger)
            {
                Logger = logger;
            }

            private ILogger Logger { get; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var content = await (request.Content?.ReadAsStringAsync(cancellationToken) ?? Task.FromResult("NULL"));
                Logger.LogDebug($"Request: {content}");
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
