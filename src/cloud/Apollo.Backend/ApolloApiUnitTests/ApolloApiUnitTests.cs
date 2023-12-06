// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using System.Collections;
using Apollo.Api;
using Apollo.Common.Entities;
using ApolloApiUnitTests;
using Daenet.MongoDal;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace apolloapiunittests
{

    /// <summary>
    /// Unit tests for the ApolloApi class.
    /// </summary>
    [TestClass]
    public class ApolloApiTests
    {
        [TestMethod]
        public async Task InsertTraining()
        {
            var api = Helpers.GetApolloApi();

            var training = new Training
            {
                Id = "T01",
                TrainingName = "Test Training"
            };

            await api.InsertTraining(training);

            await api.DeleteTrainings(new string[] { training.Id });
        }
    }
}
