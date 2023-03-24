// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.Data.Tests.Extensions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models;

namespace De.HDBW.Apollo.Data.Tests.Repositories
{
    public class MetaDataRepositoryTests : AbstractDataBaseRepositoryTest<MetaDataItem>
    {
        protected override IRepository<MetaDataItem> GetRepository()
        {
            return new MetaDataRepository(this.SetupDataBaseConnectionProvider(), this.SetupLogger<MetaDataRepository>());
        }
    }
}
