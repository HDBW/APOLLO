// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AssessmentScoreRepository :
        AbstractInMemoryRepository<AssessmentScore>,
        IAssessmentScoreRepository
    {
        public AssessmentScoreRepository(ILogger<AssessmentScoreRepository> logger)
            : base(logger)
        {
        }

        public Task<AssessmentScore?> GetItemByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(Items.FirstOrDefault(i => i.AssessmentId == id));
        }

        public int Count()
        {
            return Items.Count();
        }
    }
}
