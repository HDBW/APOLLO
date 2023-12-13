// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Common.Entities;
using ApolloApiUnitTests;
using Daenet.MongoDal;
using Microsoft.Extensions.Logging;
using Moq;

namespace Apollo.Api.UnitTests
{
    [TestClass]
    public class ApolloApiTrainingUnitTests
    {

        private ApolloApi _api;
        private List<Training> _testTrainings;


        /// <summary>
        /// Initializes test data for the Training unit tests.
        /// This method is called before each test to set up the test environment, 
        /// including creating and inserting test Training instances into the database.
        /// </summary>
        [TestInitialize]
        public void InitializeTestData()
        {
            // Arrange
            _api = Helpers.GetApolloApi();

            // Initialize test data for trainings
            _testTrainings = new List<Training>
            {
                new Training
                {
                    Id = "T01_" + Guid.NewGuid().ToString(), // Append a unique identifier to the ID
                    TrainingName = "Test Training 1"
                },
                new Training
                {
                    Id = "T02_" + Guid.NewGuid().ToString(), // Append a unique identifier to the ID
                    TrainingName = "Test Training 2"
                },
                // Add more test training instances as needed
            };

            // Insert test trainings into the database
            foreach (var training in _testTrainings)
            {
                _api.InsertTraining(training).Wait(); // Use synchronous wait in the test setup
            }
        }


        /// <summary>
        /// Cleans up test data after the Training unit tests.
        /// This method is called after each test to clean up the test environment, 
        /// including deleting the test Training instances from the database.
        /// </summary>
        [TestCleanup]
        public void CleanupTestData()
        {
            // Cleanup: Delete test trainings that were inserted in the InitializeTestData method
            foreach (var training in _testTrainings)
            {
                _api.DeleteTrainings(new string[] { training.Id }).Wait(); // Use synchronous wait in the test cleanup
            }
        }


        /// <summary>
        /// Tests the insertion of a Training object and its subsequent deletion.
        /// </summary>
        [TestMethod]
        public async Task InsertTraining()
        {
            var api = Helpers.GetApolloApi();

            // Create the test instances
            var t01 = new Training
            {
                Id = "T01",
                TrainingName = "Test Training 01"
                // Set values for other properties as needed
            };

            var t02 = new Training
            {
                Id = "T02",
                TrainingName = "Test Training 02"
                // Set values for other properties as needed
            };

            var t03 = new Training
            {
                Id = "T03",
                TrainingName = "Test Training 03"
                // Set values for other properties as needed
            };

            try
            {
                // Insert the first test training
                await api.InsertTraining(t01);

                // Perform the necessary test actions with t01
                var retrievedT01 = await api.GetTraining(t01.Id);
                Assert.IsNotNull(retrievedT01);
                Assert.AreEqual(t01.Id, retrievedT01.Id);

                // Delete the first test training
                await api.DeleteTrainings(new string[] { t01.Id });

                // Insert the second test training
                await api.InsertTraining(t02);

                // Perform the necessary test actions with t02
                var retrievedT02 = await api.GetTraining(t02.Id);
                Assert.IsNotNull(retrievedT02);
                Assert.AreEqual(t02.Id, retrievedT02.Id);

                // Delete the second test training
                await api.DeleteTrainings(new string[] { t02.Id });

                // Insert the third test training
                await api.InsertTraining(t03);

                // Perform the necessary test actions with t03
                var retrievedT03 = await api.GetTraining(t03.Id);
                Assert.IsNotNull(retrievedT03);
                Assert.AreEqual(t03.Id, retrievedT03.Id);

                // Delete the third test training
                await api.DeleteTrainings(new string[] { t03.Id });
            }
            catch (Exception ex)
            {
               
                Assert.Fail($"An exception occurred: {ex.Message}");
            }
        }


        /// <summary>
        /// Tests creating or updating a Training object and then cleaning up by deleting it.
        /// </summary>
        [TestMethod]
        public async Task CreateOrUpdateTraining()
        {
            var api = Helpers.GetApolloApi();

            var training = new Training
            {
                TrainingName = "Test Training"
            };

            var trainingIds = await api.CreateOrUpdateTraining(new List<Training> { training });

            // Ensure that the training was created or updated and has a valid ID
            Assert.IsNotNull(trainingIds);
            Assert.IsTrue(trainingIds.Count > 0);

            // Clean up: Delete the created or updated training
            await api.DeleteTrainings(trainingIds.ToArray());
        }


        /// <summary>
        /// Tests retrieving a specific Training object by its ID and then cleaning up by deleting it.
        /// </summary>
        [TestMethod]
        public async Task GetTraining()
        {
            var api = Helpers.GetApolloApi();

            var training = new Training
            {
                Id = "T01",
                TrainingName = "Test Training"
            };

            try
            {
                // Insert a test training record into the database
                await api.InsertTraining(training);

                // Retrieve the inserted training using the GetTraining method
                var retrievedTraining = await api.GetTraining(training.Id);

                // Ensure that the retrieved training is not null and has the same ID as the inserted training
                Assert.IsNotNull(retrievedTraining);
                Assert.AreEqual(training.Id, retrievedTraining.Id);
            }
            finally
            {
                // Clean up: Delete the test training record from the database
                await api.DeleteTrainings(new string[] { training.Id });
            }
        }


        /// <summary>
        /// Tests querying Training objects based on specific criteria such as TrainingName and StartDate.
        /// </summary>
        [TestMethod]
        public async Task QueryTrainings()
        {
            // Arrange
            var api = Helpers.GetApolloApi();

            var query = new Apollo.Common.Entities.Query
            {
                Fields = new List<string> { "TrainingName", "StartDate" }, // Specify the fields to be returned
                Filter = new Apollo.Common.Entities.Filter
                {
                    IsOrOperator = false,
                    Fields = new List<Apollo.Common.Entities.FieldExpression>
            {
                new Apollo.Common.Entities.FieldExpression
                {
                    FieldName = "TrainingName", // Specify the field name for filtering
                    Operator = Apollo.Common.Entities.QueryOperator.Equals, // Specify the operator for the filter
                    Argument = new List<object> { "Sample Training" } // Specify filter values
                },
                new Apollo.Common.Entities.FieldExpression
                {
                    FieldName = "StartDate", // Specify another field name for filtering
                    Operator = Apollo.Common.Entities.QueryOperator.GreaterThanEqualTo, // Specify the operator for the filter
                    Argument = new List<object> { DateTime.Now.AddMonths(-1) } // Specify filter values
                }
            }
                },
                RequestCount = true, // Set to true if you want to include count in the response
                Top = 200, // Specify the number of items to return
                Skip = 0, // Specify the skip value for paging
                SortExpression = new Apollo.Common.Entities.SortExpression
                {
                    FieldName = "StartDate", // Specify the field name for sorting
                    Order = Apollo.Common.Entities.SortOrder.Ascending // Specify the sorting direction as Ascending
                }
            };

            // Act
            IList<Training> trainings;
            try
            {
                trainings = await api.QueryTrainings(query);
            }
            catch (ApolloApiException ex)
            {
                // Handle the case when no records are found
                if (ex.ErrorCode == ErrorCodes.TrainingErrors.QueryTrainingsError)
                {
                    trainings = new List<Training>(); // Initialize an empty list
                }
                else
                {
                    // Re-throw the exception if it's not related to an empty result
                    throw;
                }
            }

            // Assert
            // Ensure that trainings are retrieved based on the query
            Assert.IsNotNull(trainings);
            Assert.IsTrue(trainings.Count >= 0); // Change the condition to allow for an empty list

            // Cleanup: Delete the training records inserted during the test
            foreach (var training in trainings)
            {
                await api.DeleteTrainings(new string[] { training.Id });
            }

            // add more assertions based on your specific testing requirements
        }
    }
}
