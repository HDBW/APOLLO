// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AnswerItemRepository :
        AbstractInMemoryRepository<AnswerItem>,
        IAnswerItemRepository
    {
        public Task<IEnumerable<AnswerItem>> GetItemsByForeignKeysAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(new ReadOnlyCollection<AnswerItem>(_items?.Where(i => ids.Contains(i.QuestionId)).ToList() ?? new List<AnswerItem>()) as IEnumerable<AnswerItem>);
        }
    }
}
