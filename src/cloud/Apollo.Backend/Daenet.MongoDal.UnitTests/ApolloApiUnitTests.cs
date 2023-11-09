// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Api;
using Apollo.Common.Entities;

namespace Daenet.MongoDal.UnitTests
{
    /// <summary>
    /// Unit tests for the ApolloApi class.
    /// </summary>
    [TestClass]
    public class ApolloApiUnitTests
    {
        private ApolloApi _api;

        /// <summary>
        /// Initialize the test environment and create an instance of ApolloApi for testing.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            var dal = Helpers.GetDal();
            _api = new ApolloApi(dal, null); // You can provide a mock logger here.
        }

        /// <summary>
        /// Test method to verify the InsertTrainings functionality.
        /// </summary>
        [TestMethod]
        public async Task InsertTrainingsTest()
        {
            // Create test data (trainings).
            var trainings = new List<Training>
            {
                // Add test training objects here.
            };

            // Test the InsertTrainings method.
            await _api.InsertTrainings(trainings);

            // Add assertions to check if the data is inserted correctly.
        }

        /// <summary>
        /// Test method to verify the InsertUsers functionality.
        /// </summary>
        [TestMethod]
        public async Task InsertUsersTest()
        {
            // Create test data (users).
            var users = new List<User>
            {
                // Add test user objects here.
            };

            // Test the InsertUsers method.
            await _api.InsertUsers(users);

            // Add assertions to check if the data is inserted correctly.
        }
    }
}
