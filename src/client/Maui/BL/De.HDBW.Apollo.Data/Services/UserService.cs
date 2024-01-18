// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class UserService : AbstractSwaggerServiceBase, IUserService
    {
        public UserService(
            ILogger<UserService> logger,
            string baseUrl,
            string authKey,
            HttpMessageHandler httpClientHandler)
            : base(logger, $"{baseUrl}/{nameof(User)}", authKey, httpClientHandler)
        {
        }

        public async Task<string?> CreateAsync(User user, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var request = new CreateOrUpdateUserRequest()
            var ids = await DoPostAsync<List<string>?>(user, token, action: "insert").ConfigureAwait(false);
            return ids?.FirstOrDefault();
        }

        public async Task<bool> SaveAsync(User user, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await DoPutAsync<bool>(user, token).ConfigureAwait(false);
        }
    }
}
