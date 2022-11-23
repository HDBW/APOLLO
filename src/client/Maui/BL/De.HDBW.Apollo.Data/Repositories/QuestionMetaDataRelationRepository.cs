// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class QuestionMetaDataRelationRepository :
        AbstractInMemoryRepository<QuestionMetaDataRelation>,
        IQuestionMetaDataRelationRepository
    {
        public QuestionMetaDataRelationRepository(ILogger<QuestionMetaDataRelationRepository> logger)
            : base(logger)
        {
        }

        public Task<IEnumerable<QuestionMetaDataRelation>> GetItemsByForeignKeysAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var notInList = Items?.Where(i => i.QuestionId == 1);
            return Task.FromResult(new ReadOnlyCollection<QuestionMetaDataRelation>(Items?.Where(i => ids.Contains(i.QuestionId)).ToList() ?? new List<QuestionMetaDataRelation>()) as IEnumerable<QuestionMetaDataRelation>);
        }
    }
}
