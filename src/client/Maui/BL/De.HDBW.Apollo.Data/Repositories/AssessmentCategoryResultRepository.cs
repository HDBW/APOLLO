// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AssessmentCategoryResultRepository :
        AbstractInMemoryRepository<AssessmentCategoryResult>,
        IAssessmentCategoryResultRepository
    {
        public AssessmentCategoryResultRepository(ILogger<AssessmentCategoryResultRepository> logger)
            : base(logger)
        {
        }

        public Task<IEnumerable<AssessmentCategoryResult>> GetItemsByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(new ReadOnlyCollection<AssessmentCategoryResult>(Items?.Where(i => i.CategoryId == id).ToList() ?? new List<AssessmentCategoryResult>()) as IEnumerable<AssessmentCategoryResult>);
        }

        public int Count() => Items.Count;
    }
}
