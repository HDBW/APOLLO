// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AnswerMetaDataRelationRepository :
        AbstractInMemoryRepository<AnswerMetaDataRelation>,
        IAnswerMetaDataRelationRepository
    {
        public AnswerMetaDataRelationRepository(ILogger<AnswerMetaDataRelationRepository> logger)
            : base(logger)
        {
        }

        public Task<IEnumerable<AnswerMetaDataRelation>> GetItemsByForeignKeysAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(new ReadOnlyCollection<AnswerMetaDataRelation>(Items?.Where(i => ids.Contains(i.AnswerId)).ToList() ?? new List<AnswerMetaDataRelation>()) as IEnumerable<AnswerMetaDataRelation>);
        }
    }
}
