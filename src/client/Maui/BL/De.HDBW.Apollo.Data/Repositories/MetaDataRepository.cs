﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class MetaDataRepository :
        AbstractInMemoryRepository<MetaDataItem>,
        IMetaDataRepository
    {
    }
}