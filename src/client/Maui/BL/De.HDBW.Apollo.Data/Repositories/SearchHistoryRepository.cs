// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class SearchHistoryRepository :
        AbstractDataBaseRepository<SearchHistory>,
        ISearchHistoryRepository
    {
        public SearchHistoryRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<SearchHistoryRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public async Task<SearchHistory?> GetItemsByQueryAsync(string query, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<SearchHistory>().FirstOrDefaultAsync(x => x.Query != null && x.Query == query).ConfigureAwait(false);
        }

        public async Task<IEnumerable<SearchHistory>> GetMaxItemsAsync(int limit, string? query, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(query))
            {
                return await asyncConnection.Table<SearchHistory>().Where(x => x.Query != null).OrderBy(x => x.Ticks).Take(limit).Take(limit).ToListAsync().ConfigureAwait(false);
            }

            return await asyncConnection.QueryAsync<SearchHistory>("SELECT * FROM SearchHistory WHERE Query != NULL AND Query LIKE ? ORDER BY Ticks LIMIT ?", $"%{query}%", limit);
        }
    }
}
