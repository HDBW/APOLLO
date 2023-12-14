// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AssessmentScoreRepository :
        AbstractDataBaseRepository<AssessmentScore>,
        IAssessmentScoreRepository
    {
        public AssessmentScoreRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<AssessmentScoreRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public async Task<AssessmentScore> CreateItemAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            var score = new AssessmentScore();
            await AddItemAsync(score, token).ConfigureAwait(false);
            return score;
        }

        public async Task<AssessmentScore?> GetItemByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<AssessmentScore>().FirstOrDefaultAsync(i => i.AssessmentId == id).ConfigureAwait(false);
        }
    }
}
