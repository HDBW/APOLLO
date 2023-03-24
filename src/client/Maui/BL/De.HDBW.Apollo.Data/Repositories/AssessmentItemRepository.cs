// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AssessmentItemRepository :
        AbstractDataBaseRepository<AssessmentItem>,
        IAssessmentItemRepository
    {
        public AssessmentItemRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<AssessmentItemRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public async Task<IEnumerable<AssessmentItem>> GetItemByAssessmentTypeAsync(AssessmentType assessmentType, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<AssessmentItem>().Where(i => i.AssessmentType == assessmentType).ToListAsync().ConfigureAwait(false);
        }
    }
}
