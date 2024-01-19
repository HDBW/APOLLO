﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public abstract class AbstractAuthorizedSwaggerServiceBase : AbstractSwaggerServiceBase
    {
        private static HttpClient? httpClient;

        public AbstractAuthorizedSwaggerServiceBase(
            ILogger logger,
            string baseUrl,
            string authKey,
            HttpMessageHandler httpClientHandler)
            : base(logger, baseUrl, authKey, httpClientHandler)
        {
        }

        public void UpdateAuthorizationHeader(string? authorizationHeader)
        {
            try
            {
                if (httpClient?.DefaultRequestHeaders == null)
                {
                    return;
                }

                var key = "Authorization";

                if (httpClient.DefaultRequestHeaders.Contains(key))
                {
                    httpClient.DefaultRequestHeaders.Remove(key);
                }

                if (string.IsNullOrWhiteSpace(authorizationHeader))
                {
                    return;
                }

                httpClient?.DefaultRequestHeaders.Add(key, authorizationHeader);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error while {nameof(UpdateAuthorizationHeader)} in {GetType().Name}.");
                throw;
            }
        }
    }
}
