// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class UnregisterUserService : AbstractAuthorizedSwaggerServiceBase, IUnregisterUserService
    {
        public UnregisterUserService(
            ILogger<UnregisterUserService>? logger,
            string baseUrl,
            string authKey,
            HttpMessageHandler httpClientHandler)
            : base(logger, $"{baseUrl}", authKey, httpClientHandler)
        {
        }

        public Task<bool> DeleteAsync(string accessToken, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(true);
        }
    }
}
