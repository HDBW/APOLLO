// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models;

namespace De.HDBW.Apollo.SharedContracts.Repositories
{
    public interface IRepository<TU>
        where TU : IEntity, new()
    {
        Task<IEnumerable<TU>> GetItemsAsync(CancellationToken token);

        Task<TU?> GetItemByIdAsync(long id, CancellationToken token);

        Task<IEnumerable<TU>> GetItemsByIdsAsync(IEnumerable<long> ids, CancellationToken token);

        Task<bool> AddItemAsync(TU item, CancellationToken token);

        Task<bool> AddItemsAsync(IEnumerable<TU> items, CancellationToken token);

        Task<bool> RemoveItemAsync(TU item, CancellationToken token);

        Task<bool> RemoveItemsAsync(IEnumerable<TU> items, CancellationToken token);

        Task<bool> RemoveItemByIdAsync(long id, CancellationToken token);

        Task<bool> RemoveItemsByIdsAsync(IEnumerable<long> ids, CancellationToken token);

        Task<bool> ResetItemsAsync(IEnumerable<TU>? items, CancellationToken token);

    }
}
