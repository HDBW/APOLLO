// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.Data.Tests.Extensions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace De.HDBW.Apollo.Data.Tests.Repositories
{
    public class AnswerMetaDataRelationRepositoryTests : AbstractDataBaseRepositoryTest<AnswerMetaDataRelation>
    {
        protected override IRepository<AnswerMetaDataRelation> GetRepository()
        {
            return new AnswerMetaDataRelationRepository(this.SetupLogger<AnswerMetaDataRelationRepository>());
        }
    }
}
