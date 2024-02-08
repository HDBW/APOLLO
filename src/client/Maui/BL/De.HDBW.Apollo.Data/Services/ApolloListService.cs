// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.Lists;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class ApolloListService : AbstractAuthorizedSwaggerServiceBase, IApolloListService
    {
        public ApolloListService(
            ILogger<ApolloListService>? logger,
            string baseUrl,
            string authKey,
            HttpMessageHandler httpClientHandler)
             : base(logger, $"{baseUrl}/{"List"}/query", authKey, httpClientHandler)
        {
        }

        public async Task<ApolloList?> GetAsync(string type, IEnumerable<string>? ids, CultureInfo? country, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var request = new GetListRequest();
            request.ItemType = type;
            request.Ids = ids?.ToArray() ?? Array.Empty<string>();
            request.Lng = country?.ThreeLetterISOLanguageName;
            var result = await DoPostAsync<GetListResponse>(request, token);
            return result?.Result;
        }
    }
}
