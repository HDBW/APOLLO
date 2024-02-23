// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api.Apollo.RestService.Apollo.Common.Messages;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class ProfileService : AbstractAuthorizedSwaggerServiceBase, IProfileService
    {
        public ProfileService(
            ILogger<UserService>? logger,
            string baseUrl,
            string authKey,
            HttpMessageHandler httpClientHandler)
            : base(logger, $"{baseUrl}/{nameof(Profile)}", authKey, httpClientHandler)
        {
        }

        public async Task<Profile?> GetProfileAsync(string id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var response = await DoGetAsync<GetProfileResponse>(id, token).ConfigureAwait(false);
            return response?.Profile;
        }

        public async Task<string?> SaveAsync(string userId, Profile profile, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var request = new CreateOrUpdateProfileRequest() { Profile = profile, UserId = userId };
            var response = await DoPutAsync<CreateOrUpdateProfileResponse>(request, token).ConfigureAwait(false);
            return response?.Result;
        }
    }
}
