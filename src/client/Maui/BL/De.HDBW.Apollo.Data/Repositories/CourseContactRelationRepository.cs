// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class CourseContactRelationRepository :
        AbstractDataBaseRepository<CourseContactRelation>,
        ICourseContactRelationRepository
    {
        public CourseContactRelationRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<CourseContactRelationRepository> logger)
           : base(dataBaseConnectionProvider, logger)
        {
        }

        public async Task<IEnumerable<CourseContactRelation>> GetItemsByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<CourseContactRelation>().Where(i => i.CourseId == id).ToListAsync().ConfigureAwait(false);
        }
    }
}
