// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AnswerItemResultRepository :
        AbstractInMemoryRepository<AnswerItemResult>,
        IAnswerItemResultRepository
    {
        public AnswerItemResultRepository(ILogger<AnswerItemResultRepository> logger)
            : base(logger)
        {
        }

        public Task<IEnumerable<AnswerItemResult>> GetItemsByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(new ReadOnlyCollection<AnswerItemResult>(Items?.Where(i => i.AssessmentItemId == id).ToList() ?? new List<AnswerItemResult>()) as IEnumerable<AnswerItemResult>);
        }

        public Task<long> GetNextIdAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(Items.Any() ? Items.Max(m => m.Id) + 1 : 0);
        }
    }
}
