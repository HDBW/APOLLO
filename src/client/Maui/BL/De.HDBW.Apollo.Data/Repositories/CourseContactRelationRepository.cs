// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class CourseContactRelationRepository :
        AbstractInMemoryRepository<CourseContactRelation>,
        ICourseContactRelationRepository
    {
        public CourseContactRelationRepository(ILogger<CourseContactRelationRepository> logger)
           : base(logger)
        {
        }

        public Task<IEnumerable<CourseContactRelation>> GetItemsByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var result = new ReadOnlyCollection<CourseContactRelation>(Items.Where(i => i.CourseId == id).ToList());
            return Task.FromResult(result as IEnumerable<CourseContactRelation>);
        }
    }
}
