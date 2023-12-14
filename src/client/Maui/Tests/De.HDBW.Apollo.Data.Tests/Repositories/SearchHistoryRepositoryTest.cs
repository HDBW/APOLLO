// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.Data.Tests.Extensions;
using De.HDBW.Apollo.Data.Tests.Model;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Xunit;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Repositories
{
    public class SearchHistoryRepositoryTest : AbstractDataBaseRepositoryTest<SearchHistory>
    {
        public SearchHistoryRepositoryTest(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData(9, 10, 9, null)]
        [InlineData(10, 10, 10, null)]
        [InlineData(50, 10, 10, null)]
        [InlineData(50, 10, 10, "it")]
        public async Task GetMaxItemsAsync(int itemsToGenerate, int resultLimit, int expectedResultCount, string query)
        {
            using (var context = new DatabaseTestContext(Path.GetTempFileName(), Logger))
            {
                var repository = GetRepository(context) as ISearchHistoryRepository;
                var data = new List<SearchHistory>();
                await repository.ResetItemsAsync(data, CancellationToken.None);

                for (int i = 0; i < itemsToGenerate; i++)
                {
                    data.Add(new SearchHistory() { Ticks = i, Query = $"Item{i}" });
                }

                await repository.AddItemsAsync(data, CancellationToken.None);

                var result = await repository.GetMaxItemsAsync(resultLimit, query, CancellationToken.None);
                var resultList = result.ToList();
                Assert.NotNull(result);
                Assert.True(result.Count() == expectedResultCount);
                for (int i = 0; i < expectedResultCount; i++)
                {
                    Assert.Equal(itemsToGenerate - 1 - i, resultList[i].Ticks);
                }
            }
        }

        protected override IRepository<SearchHistory> GetRepository(DatabaseTestContext context)
        {
            return new SearchHistoryRepository(this.SetupDataBaseConnectionProvider(context), this.SetupLogger<SearchHistoryRepository>(OutputHelper));
        }
    }
}
