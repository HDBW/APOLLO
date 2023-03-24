// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AssessmentCategoryRepository : AbstractDataBaseRepository<AssessmentCategory>,
        IAssessmentCategoryRepository
    {
        public AssessmentCategoryRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<AssessmentCategoryRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public Task<IEnumerable<AssessmentCategory>> GetItemByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return GetItemByForeignKeysAsync(new List<long>() { id }, token);
        }

        public async Task<IEnumerable<AssessmentCategory>> GetItemByForeignKeysAsync(List<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<AssessmentCategory>().Where(i => ids.Contains(i.CourseId)).ToListAsync().ConfigureAwait(false);
        }
    }
}
