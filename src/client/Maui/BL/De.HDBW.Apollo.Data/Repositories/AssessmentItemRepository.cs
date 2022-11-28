// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AssessmentItemRepository :
        AbstractInMemoryRepository<AssessmentItem>,
        IAssessmentItemRepository
    {
        public AssessmentItemRepository(ILogger<AssessmentItemRepository> logger)
            : base(logger)
        {
        }

        public Task<IEnumerable<AssessmentItem>> GetItemByAssessmentTypeAsync(AssessmentType assessmentType, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(Items.Where(i => i.AssessmentType == assessmentType));
        }
    }
}
