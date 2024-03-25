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
            : base(logger, $"{baseUrl}/apollouser", authKey, httpClientHandler)
        {
        }

        public async Task<bool> DeleteAsync(string userId, string uniqueId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var request = new ApolloUser() { ApolloUserId = userId, UserObjectId = uniqueId };
            return await DoPostAsync<bool>(request, token).ConfigureAwait(false);
        }

        public class ApolloUser()
        {
            public string ApolloUserId { get; set; } = string.Empty;

            public string UserObjectId { get; set; } = string.Empty;
        }
    }
}
