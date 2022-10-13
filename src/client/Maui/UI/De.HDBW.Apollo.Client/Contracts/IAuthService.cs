// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IAuthService
    {
        Task<AuthenticationResult?> SignInInteractively(CancellationToken cancellationToken);

        Task<AuthenticationResult?> AcquireTokenSilent(CancellationToken cancellationToken);

        Task LogoutAsync(CancellationToken cancellationToken);
    }
}
