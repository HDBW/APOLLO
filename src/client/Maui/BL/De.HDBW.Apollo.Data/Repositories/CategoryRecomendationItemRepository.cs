// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class CategoryRecomendationItemRepository :
        AbstractInMemoryRepository<CategoryRecomendationItem>,
        ICategoryRecomendationItemRepository
    {
        public CategoryRecomendationItemRepository(ILogger<CategoryRecomendationItemRepository> logger)
            : base(logger)
        {
        }

        public Task<IEnumerable<CategoryRecomendationItem>> GetItemsByForeignKeysAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var result = new ReadOnlyCollection<CategoryRecomendationItem>(Items.Where(i => ids.Contains(i.CategoryId)).ToList());
            return Task.FromResult(result as IEnumerable<CategoryRecomendationItem>);
        }
    }
}
