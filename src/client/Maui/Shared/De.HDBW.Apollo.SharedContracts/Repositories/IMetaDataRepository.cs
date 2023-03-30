// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models;

namespace De.HDBW.Apollo.SharedContracts.Repositories
{
    public interface IMetaDataRepository :
        IRepository<MetaDataItem>,
        IDatabaseRepository<MetaDataItem>
    {
    }
}
