﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.Data.Tests.Extensions;
using De.HDBW.Apollo.Data.Tests.Model;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Repositories
{
    public class QuestionMetaDataRelationRepositoryTests : AbstractDataBaseRepositoryTest<QuestionMetaDataRelation>
    {
        public QuestionMetaDataRelationRepositoryTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override IRepository<QuestionMetaDataRelation> GetRepository(DatabaseTestContext context)
        {
            return new QuestionMetaDataRelationRepository(this.SetupDataBaseConnectionProvider(context), this.SetupLogger<QuestionMetaDataRelationRepository>(OutputHelper));
        }
    }
}
