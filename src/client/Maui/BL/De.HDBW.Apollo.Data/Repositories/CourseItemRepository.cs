// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class CourseItemRepository :
        AbstractInMemoryRepository<CourseItem>,
        ICourseItemRepository
    {
        public CourseItemRepository(ILogger<CourseItemRepository> logger)
            : base(logger)
        {
        }

        public Task<bool> ResetUnpublishedAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Items.Where(i => ids.Contains(i.Id)).ToList().ForEach((i) => i.UnPublishingDate = null);
            return Task.FromResult(true);
        }
    }
}
