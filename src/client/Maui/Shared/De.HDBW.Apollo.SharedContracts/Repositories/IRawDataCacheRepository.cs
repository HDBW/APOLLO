// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using De.HDBW.Apollo.SharedContracts.Models;

namespace De.HDBW.Apollo.SharedContracts.Repositories
{
    public interface IRawDataCacheRepository
        : IRepository<CachedRawData>, IDatabaseRepository<CachedRawData>
    {
        Task<CachedRawData> GetItemAsync(string sessionId, string? rawdataId, CancellationToken token);

        Task<IEnumerable<CachedRawData>> GetItemsAsync(string sessionId, IEnumerable<string> rawdataIds, CancellationToken token);
    }
}
