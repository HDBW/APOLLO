// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Api;

namespace Daenet.MongoDal.UnitTests
{
    internal class Helpers
    {
        internal static MongoDataAccessLayer GetDal()
        {
            MongoDataAccessLayer dal = new MongoDataAccessLayer(new Entitties.MongoDalConfig()
            {
                MongoConnStr = "mongodb://apollobackend-cosmos-dev:ixHaRynO7TgcqeES2Ho6eq3PYXWYTwRHgnai316AvC69TJkecHbKwPr7CW47ReSdGgLIk3f0FD8FACDbgUNQxQ==@apollobackend-cosmos-dev.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@apollobackend-cosmos-dev@",
                MongoDatabase = "apollobackend-cosmos-dev"
            });

            return dal;
        }

        internal static string GetCollectionName<T>()
        {
            return ApolloApi.GetCollectionName<T>();
        }
    }
}
