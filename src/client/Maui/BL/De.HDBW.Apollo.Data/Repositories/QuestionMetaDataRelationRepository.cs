// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Graph.Apollo.Cloud.Common.Models.Assessment;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class QuestionMetaDataRelationRepository :
        AbstractInMemoryRepository<QuestionMetaDataRelation>,
        IQuestionMetaDataRelationRepository
    {
        public Task<IEnumerable<QuestionMetaDataRelation>> GetItemsByForeignKeysAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(new ReadOnlyCollection<QuestionMetaDataRelation>(_items?.Where(i => ids.Contains(i.QuestionId)).ToList() ?? new List<QuestionMetaDataRelation>()) as IEnumerable<QuestionMetaDataRelation>);
        }
    }
}
