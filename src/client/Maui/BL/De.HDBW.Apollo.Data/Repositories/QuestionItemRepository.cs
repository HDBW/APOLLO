// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Graph.Apollo.Cloud.Common.Models.Assessment;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class QuestionItemRepository :
        AbstractInMemoryRepository<QuestionItem>,
        IQuestionItemRepository
    {
        public Task<IEnumerable<QuestionItem>> GetItemsByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(new ReadOnlyCollection<QuestionItem>(_items?.Where(i => i.AssessmentId == id).ToList() ?? new List<QuestionItem>()) as IEnumerable<QuestionItem>);
        }
    }
}
