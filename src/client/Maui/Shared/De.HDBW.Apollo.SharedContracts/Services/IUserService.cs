// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface IUserService
    {
        Task<string?> CreateAsync(User user, CancellationToken token);

        Task<bool> SaveAsync(User user, CancellationToken token);
    }
}
