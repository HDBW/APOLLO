// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class CourseItemRepository :
        AbstractDataBaseRepository<CourseItem>,
        ICourseItemRepository
    {
        public CourseItemRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<CourseItemRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public async Task<bool> ResetUnpublishedAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var items = await GetItemsByIdsAsync(ids, token).ConfigureAwait(false);
            if (items?.Any() ?? true)
            {
                return false;
            }

            items.ToList().ForEach((i) => i.UnPublishingDate = null);
            return await UpdateItemsAsync(items, token).ConfigureAwait(false);
        }
    }
}
