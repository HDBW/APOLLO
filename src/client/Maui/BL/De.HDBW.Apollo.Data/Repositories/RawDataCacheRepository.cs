// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class RawDataCacheRepository : AbstractDataBaseRepository<CachedRawData>, IRawDataCacheRepository
    {
        public RawDataCacheRepository(
            IDataBaseConnectionProvider dataBaseConnectionProvider,
            ILogger<RawDataCacheRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public async Task<CachedRawData> GetItemAsync(string sessionId, string? rawdataId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            if (rawdataId == null)
            {
                return await asyncConnection.Table<CachedRawData>().FirstOrDefaultAsync(x => x.SessionId == sessionId).ConfigureAwait(false);
            }

            return await asyncConnection.Table<CachedRawData>().FirstOrDefaultAsync(x => x.SessionId == sessionId && x.RawDataId == rawdataId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CachedRawData>> GetItemsAsync(string sessionId, IEnumerable<string> rawdataIds, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<CachedRawData>().Where(x => x.SessionId == sessionId && rawdataIds.Contains(x.RawDataId)).ToListAsync().ConfigureAwait(false);
        }
    }
}
