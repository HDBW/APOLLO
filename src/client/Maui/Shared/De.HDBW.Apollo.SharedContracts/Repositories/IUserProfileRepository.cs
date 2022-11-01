// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.SharedContracts.Repositories
{
    public interface IUserProfileRepository :
        IRepository<UserProfile>,
        IDatabaseRepository<UserProfile>
    {
    }
}
