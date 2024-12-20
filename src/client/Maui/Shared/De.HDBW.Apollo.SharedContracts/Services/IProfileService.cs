﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface IProfileService
    {
        Task<Profile?> GetAsync(string id, CancellationToken token);

        Task<string?> SaveAsync(string userId, Profile profile, CancellationToken token);
    }
}
