// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface IUnregisterUserService
    {
        void UpdateAuthorizationHeader(string? authorizationHeader);

        Task<bool> DeleteAsync(string accessToken, CancellationToken token);
    }
}
