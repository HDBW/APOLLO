// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class MetaDataMetaDataRelationRepository :
        AbstractDataBaseRepository<MetaDataMetaDataRelation>,
        IMetaDataMetaDataRelationRepository
    {
        public MetaDataMetaDataRelationRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<MetaDataMetaDataRelationRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public async Task<IEnumerable<MetaDataMetaDataRelation>> GetItemsBySourceIdsAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<MetaDataMetaDataRelation>().Where(i => ids.Contains(i.SourceId)).ToListAsync().ConfigureAwait(false);
        }
    }
}
