// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class CategoryRecomendationItemRepository :
        AbstractDataBaseRepository<CategoryRecomendationItem>,
        ICategoryRecomendationItemRepository
    {
        public CategoryRecomendationItemRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<CategoryRecomendationItemRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public async Task<IEnumerable<CategoryRecomendationItem>> GetItemsByForeignKeysAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<CategoryRecomendationItem>().Where(i => ids.Contains(i.CategoryId)).ToListAsync().ConfigureAwait(false);
        }
    }
}
