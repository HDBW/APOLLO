// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AnswerMetaDataRelationRepository :
        AbstractDataBaseRepository<AnswerMetaDataRelation>,
        IAnswerMetaDataRelationRepository
    {
        public AnswerMetaDataRelationRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<AnswerMetaDataRelationRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public async Task<IEnumerable<AnswerMetaDataRelation>> GetItemsByForeignKeysAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<AnswerMetaDataRelation>().Where(i => ids.Contains(i.AnswerId)).ToListAsync().ConfigureAwait(false);
        }
    }
}
