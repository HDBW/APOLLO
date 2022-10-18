// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Graph.Apollo.Cloud.Common.Models.Assessment;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AnswerMetaDataRelationRepository :
        AbstractInMemoryRepository<AnswerMetaDataRelation>,
        IAnswerMetaDataRelationRepository
    {
        public Task<IEnumerable<AnswerMetaDataRelation>> GetItemsByForeignKeysAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(new ReadOnlyCollection<AnswerMetaDataRelation>(_items?.Where(i => ids.Contains(i.AnswerId)).ToList() ?? new List<AnswerMetaDataRelation>()) as IEnumerable<AnswerMetaDataRelation>);
        }
    }
}
