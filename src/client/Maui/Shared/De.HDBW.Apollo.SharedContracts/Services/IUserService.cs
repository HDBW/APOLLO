// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface IUserService
    {
        void UpdateAuthorizationHeader(string? authorizationHeader);

        Task<User?> GetAsync(string id, CancellationToken token);

        Task<string?> SaveAsync(User user, CancellationToken token);

        Task<bool> DeleteAsync(string? accessToken, CancellationToken token);
    }
}
