// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Models;

namespace De.HDBW.Apollo.SharedContracts.Repositories
{
    public interface IFavoriteRepository
    {
        Task<bool> DeleteFavoriteAsync(string apiId, CancellationToken token);

        Task<Favorite?> GetItemByApiIdAsync(string apiId, CancellationToken token);

        Task<IEnumerable<Favorite?>?> GetItemsAsync(CancellationToken token);

        Task<bool> SaveAsync(Favorite favorite, CancellationToken token);
    }
}
