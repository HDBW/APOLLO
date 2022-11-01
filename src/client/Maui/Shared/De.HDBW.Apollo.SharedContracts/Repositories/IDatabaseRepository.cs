// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models;

namespace De.HDBW.Apollo.SharedContracts.Repositories
{
    public interface IDatabaseRepository
    {
    }

    public interface IDatabaseRepository<TU> : IDatabaseRepository
        where TU : IEntity, new()
    {
        Task<bool> AddOrUpdateItemsAsync(IEnumerable<TU> items, CancellationToken token);

        Task<bool> AddOrUpdateItemAsync(TU item, CancellationToken token);

        Task<bool> UpdateItemAsync(TU item, CancellationToken token);

        Task<bool> UpdateItemsAsync(IEnumerable<TU> items, CancellationToken token);
    }
}
