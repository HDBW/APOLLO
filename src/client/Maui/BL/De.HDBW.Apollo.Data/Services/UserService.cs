// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class UserService : AbstractAuthorizedSwaggerServiceBase, IUserService
    {
        public UserService(
            ILogger<UserService>? logger,
            string baseUrl,
            string authKey,
            HttpMessageHandler httpClientHandler)
            : base(logger, $"{baseUrl}/{nameof(User)}", authKey, httpClientHandler)
        {
        }

        public async Task<User?> GetUserAsync(string id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var response = await DoGetAsync<GetUserRespnse>(id, token).ConfigureAwait(false);
            return response?.User;
        }

        public async Task<string?> SaveAsync(User user, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var request = new CreateOrUpdateUserRequest(user);
            var response = await DoPutAsync<CreateOrUpdateUserResponse>(request, token).ConfigureAwait(false);
            return response?.Result;
        }
    }
}
