// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.SharedContracts.Repositories
{
    public interface IAssessmentScoreRepository :
        IRepository<AssessmentScore>,
        IDatabaseRepository<AssessmentScore>
    {
        Task<AssessmentScore?> GetItemByForeignKeyAsync(long id, CancellationToken token);

        Task<AssessmentScore> CreateItemAsync(CancellationToken token);
    }
}
