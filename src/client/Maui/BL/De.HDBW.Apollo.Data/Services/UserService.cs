// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.Data.Services
{
    public class UserService : IUserService
    {
        public Task<bool> SaveAsync(User user, CancellationToken token)
        {
            return Task.FromResult(false);
        }
    }
}
