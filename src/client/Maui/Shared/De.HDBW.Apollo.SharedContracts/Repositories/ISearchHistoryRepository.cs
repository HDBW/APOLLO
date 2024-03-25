// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Models;

namespace De.HDBW.Apollo.SharedContracts.Repositories
{
    public interface ISearchHistoryRepository :
        IRepository<SearchHistory>,
        IDatabaseRepository<SearchHistory>
    {
        Task<SearchHistory?> GetItemsByQueryAsync(string query, CancellationToken none);

        Task<IEnumerable<SearchHistory>> GetMaxItemsAsync(int limit, string? query, CancellationToken token);
    }
}
