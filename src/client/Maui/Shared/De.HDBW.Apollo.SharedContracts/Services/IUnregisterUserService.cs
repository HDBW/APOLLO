﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface IUnregisterUserService
    {
        Task<bool> DeleteAsync(string userId, string objectId, CancellationToken token);
    }
}
