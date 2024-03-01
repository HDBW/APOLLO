// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.Data.Tests.Extensions;
using De.HDBW.Apollo.Data.Tests.Model;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Repositories
{
    public class QuestionItemRepositoryTests : AbstractDataBaseRepositoryTest<QuestionItem>
    {
        public QuestionItemRepositoryTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override IRepository<QuestionItem> GetRepository(DatabaseTestContext context)
        {
            return new QuestionItemRepository(this.SetupDataBaseConnectionProvider(context), this.SetupLogger<QuestionItemRepository>(OutputHelper));
        }
    }
}
