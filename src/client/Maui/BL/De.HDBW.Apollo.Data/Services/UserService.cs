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
            IProfileService? profileService,
            HttpMessageHandler httpClientHandler)
            : base(logger, $"{baseUrl}/{nameof(User)}", authKey, httpClientHandler)
        {
            ArgumentNullException.ThrowIfNull(profileService);
            ProfileService = profileService;
        }

        private IProfileService ProfileService { get; }

        public async Task<User?> GetAsync(string id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var response = await DoGetAsync<GetUserResponse>(id, token).ConfigureAwait(false);
            var user = response?.User;
            if (!string.IsNullOrWhiteSpace(user?.Id))
            {
                var profileId = $"Profile-{user.Id}_v01";
                Profile? profile = null;
                try
                {
                    profile = await ProfileService.GetAsync(profileId, token).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    profile = null;
                    Logger?.LogError(ex, $"Unknown error in {nameof(GetAsync)} in {GetType().Name}.");
                }

                user.Profile = profile;
            }

            return user;
        }

        public async Task<string?> SaveAsync(User user, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var profile = user.Profile;
            CreateOrUpdateUserResponse? response = null;
            try
            {
                user.Profile = null;
                var request = new CreateOrUpdateUserRequest(user);
                response = await DoPutAsync<CreateOrUpdateUserResponse>(request, token).ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(response?.Result) && profile != null)
                {
                    await ProfileService.SaveAsync(response.Result, profile, token).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                response = null;
                Logger?.LogError(ex, $"Unknown error in {nameof(SaveAsync)} in {GetType().Name}.");
            }
            finally
            {
                user.Profile = profile;
            }

            return response?.Result;
        }
    }
}
