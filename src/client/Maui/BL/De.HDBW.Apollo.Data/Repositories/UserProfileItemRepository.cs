// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class UserProfileItemRepository :
        AbstractInMemoryRepository<UserProfileItem>,
        IUserProfileItemRepository
    {
        public UserProfileItemRepository(ILogger<UserProfileItemRepository> logger)
            : base(logger)
        {
        }
    }
}
