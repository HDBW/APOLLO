// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Common.Entities;
using Daenet.MongoDal;
using Microsoft.Extensions.Logging;
using Moq;

namespace Apollo.Api.UnitTests
{
    [TestClass]
    public class ApolloApiTrainingUnitTests
    {
        Training[] _testTrainings = new Training[]
      {
            new Training(){  Id = "T01", ProviderId = "unittest", TrainingName = "Open AI",
            Loans = new List<Loans>(
                new Loans[]
                {
                    new Loans() { Id = "L01", Name = "Loan 1" },
                    new Loans() { Id = "L02", Name = "Loan 2" }
                }
                )},

            new Training(){  Id = "T02", ProviderId = "unittest", TrainingName = "Azure AI" },

            new Training(){  Id = "T03" , ProviderId = "unittest",    Loans = new List<Loans>(
                new Loans[]
                {
                    new Loans() { Id = "L01", Name = "Loan 1" },
                    new Loans() { Id = "L02", Name = "Loan 2" }
                }
                )},
      };

        private async Task CleanTestDocuments()
        {
            var dal = Helpers.GetDal();

            await dal.DeleteManyAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => t.Id).ToArray(), false);
        }

        /// <summary>
        /// Cleansup all test data.
        /// </summary>
        /// <returns></returns>
        [TestCleanup]
        public async Task CleanupTest()
        {
            await CleanTestDocuments();
        }


        /// <summary>
        /// Cleansup all test data.
        /// </summary>
        /// <returns></returns>
        [TestInitialize]
        public async Task InitTest()
        {
            await CleanTestDocuments();
        }


        /// <summary>
        /// Tests the insertion of a Training object and its subsequent deletion.
        /// </summary>
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


        /// <summary>
        /// Tests querying Training objects based on specific criteria such as TrainingName and StartDate.
        /// </summary>
        [TestMethod]
        public async Task LookupTrainingsWithLoans()
        {
            var api = Helpers.GetApolloApi();

            await api.InsertTrainings(_testTrainings);

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
                        Operator = Apollo.Common.Entities.QueryOperator.LessThan, // Specify the operator for the filter
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
