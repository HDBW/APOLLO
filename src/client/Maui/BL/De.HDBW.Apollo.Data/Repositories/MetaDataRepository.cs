﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class MetaDataRepository :
        AbstractDataBaseRepository<MetaDataItem>,
        IMetaDataRepository
    {
        public MetaDataRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<MetaDataRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }
    }
}
