// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Enums;

namespace De.HDBW.Apollo.SharedContracts.Helper
{
    public interface IUseCaseBuilder
    {
        Task<bool> BuildAsync(UseCase usecase, CancellationToken token);
    }
}
