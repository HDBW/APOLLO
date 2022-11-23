// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class QuestionItemRepository :
        AbstractInMemoryRepository<QuestionItem>,
        IQuestionItemRepository
    {
        public QuestionItemRepository(ILogger<QuestionItemRepository> logger)
            : base(logger)
        {
        }

        public Task<IEnumerable<QuestionItem>> GetItemsByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(new ReadOnlyCollection<QuestionItem>(Items?.Where(i => i.AssessmentId == id).ToList() ?? new List<QuestionItem>()) as IEnumerable<QuestionItem>);
        }
    }
}
