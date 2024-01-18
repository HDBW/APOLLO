// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Backend;
using Invite.Apollo.App.Graph.Common.Backend.Api;
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

        public async Task<User?> SaveAsync(User user, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var request = new CreateOrUpdateUserRequest(user);
            var response = await DoPutAsync<CreateOrUpdateUserResponse>(request, token).ConfigureAwait(false);
            return response?.Result;
        }
    }
}
