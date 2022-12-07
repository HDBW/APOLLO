// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AssessmentCategoryRepository : AbstractInMemoryRepository<AssessmentCategory>,
        IAssessmentCategoryRepository
    {
        public AssessmentCategoryRepository(ILogger<AssessmentCategoryRepository> logger)
            : base(logger)
        {
        }

        public Task<List<AssessmentCategory>> GetItemByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(new List<AssessmentCategory>());
        }

        public Task<IEnumerable<AssessmentCategory>> GetItemByForeignKeysAsync(List<long> courseIds, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(Items.Where(i => courseIds.Contains(i.CourseId)));
        }
    }
}
