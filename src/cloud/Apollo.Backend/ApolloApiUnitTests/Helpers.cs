// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Api;
using Daenet.MongoDal;

namespace ApolloApiUnitTests
{
    public class Helpers
    {
        internal static MongoDataAccessLayer GetDal()
        {
            MongoDataAccessLayer dal = new MongoDataAccessLayer(new Daenet.MongoDal.Entitties.MongoDalConfig()
            {
                MongoConnStr = "mongodb://apollodb-cosmos-dev:TV9uJP68Tr07LalP2dVazch7AVBXfsVqB4HIzkGMNbBQtKWe7aTT42iKdZt8IuCuH0UHLjbgmwwRACDbIv9d2A==@apollodb-cosmos-dev.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@apollodb-cosmos-dev@",
                MongoDatabase = "apollodb"
            });

            return dal;
        }

        internal static string GetCollectionName<T>()
        {
            return ApolloApi.GetCollectionName<T>();
        }
    }
}
