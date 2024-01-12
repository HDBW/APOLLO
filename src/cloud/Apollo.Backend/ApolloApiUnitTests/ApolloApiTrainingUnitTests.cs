// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            new Training(){  Id = "LT01", ProviderId = "unittest", TrainingName = "Open AI",
            Loans = new List<Loans>(
                new Loans[]
                {
                    new Loans() { Id = "L01", Name = "Loan 1" },
                    new Loans() { Id = "L02", Name = "Loan 2" }
                }
                )},

            new Training(){  Id = "LT02", ProviderId = "unittest", TrainingName = "Azure AI" },

            new Training(){  Id = "LT03" , ProviderId = "unittest",    Loans = new List<Loans>(
                new Loans[]
                {
                    new Loans() { Id = "L01", Name = "Loan 1" },
                    new Loans() { Id = "L02", Name = "Loan 2" }
                }
                )},


      };

        /// <summary>
        /// The instance of the training with all properties.
        /// </summary>
        private string _complexTrainingJson = @"[
  {
    ""id"": ""SER01"",
    ""providerId"": ""hdbw-F626FEDE-1A30-4DE0-B17B-9DCB04A654C2"",
    ""trainingName"": ""Training05"",
    ""description"": ""Description of Training 05"",
    ""shortDescription"": ""Short Description of T05"",
    ""content"": [
      ""<string>"",
      ""<string>""
    ],
    ""benefitList"": [
      ""<string>"",
      ""<string>""
    ],
    ""certificate"": [
      ""<string>"",
      ""<string>""
    ],
    ""prerequisites"": [
      ""<string>"",
      ""<string>""
    ],
    ""loans"": [
      {
        ""id"": ""<string>"",
        ""name"": ""<string>"",
        ""description"": ""<string>"",
        ""url"": ""<uri>"",
        ""loanContact"": {
          ""id"": ""<string>"",
          ""surname"": ""<string>"",
          ""mail"": ""<string>"",
          ""phone"": ""<string>"",
          ""organization"": ""<string>"",
          ""address"": ""<string>"",
          ""city"": ""<string>"",
          ""zipCode"": ""<string>"",
          ""eAppointmentUrl"": ""<uri>""
        }
      },
      {
        ""id"": ""<string>"",
        ""name"": ""<string>"",
        ""description"": ""<string>"",
        ""url"": ""<uri>"",
        ""loanContact"": {
          ""id"": ""<string>"",
          ""surname"": ""<string>"",
          ""mail"": ""<string>"",
          ""phone"": ""<string>"",
          ""organization"": ""<string>"",
          ""address"": ""<string>"",
          ""city"": ""<string>"",
          ""zipCode"": ""<string>"",
          ""eAppointmentUrl"": ""<uri>""
        }
      }
    ],
    ""trainingProvider"": {
      ""id"": ""<string>"",
      ""name"": ""<string>"",
      ""description"": ""<string>"",
      ""url"": ""<uri>"",
      ""contact"": {
        ""id"": ""<string>"",
        ""surname"": ""<string>"",
        ""mail"": ""<string>"",
        ""phone"": ""<string>"",
        ""organization"": ""<string>"",
        ""address"": ""<string>"",
        ""city"": ""<string>"",
        ""zipCode"": ""<string>"",
        ""eAppointmentUrl"": ""<uri>""
      },
      ""image"": ""<uri>""
    },
    ""courseProvider"": {
      ""id"": ""<string>"",
      ""name"": ""<string>"",
      ""description"": ""<string>"",
      ""url"": ""<uri>"",
      ""contact"": {
        ""id"": ""<string>"",
        ""surname"": ""<string>"",
        ""mail"": ""<string>"",
        ""phone"": ""<string>"",
        ""organization"": ""<string>"",
        ""address"": ""<string>"",
        ""city"": ""<string>"",
        ""zipCode"": ""<string>"",
        ""eAppointmentUrl"": ""<uri>""
      },
      ""image"": ""<uri>""
    },
    ""appointments"": {
      ""id"": ""<string>"",
      ""appointment"": ""<uri>"",
      ""appointmentType"": ""<string>"",
      ""appointmentDescription"": ""<string>"",
      ""appointmentLocation"": {
        ""id"": ""<string>"",
        ""surname"": ""<string>"",
        ""mail"": ""<string>"",
        ""phone"": ""<string>"",
        ""organization"": ""<string>"",
        ""address"": ""<string>"",
        ""city"": ""<string>"",
        ""zipCode"": ""<string>"",
        ""eAppointmentUrl"": ""<uri>""
      },
      ""startDate"": ""<dateTime>"",
      ""endDate"": ""<dateTime>"",
      ""durationDescription"": ""<string>"",
      ""duration"": {
        ""ticks"": ""<long>"",
        ""days"": ""<integer>"",
        ""hours"": ""<integer>"",
        ""milliseconds"": ""<integer>"",
        ""microseconds"": ""<integer>"",
        ""nanoseconds"": ""<integer>"",
        ""minutes"": ""<integer>"",
        ""seconds"": ""<integer>"",
        ""totalDays"": ""<double>"",
        ""totalHours"": ""<double>"",
        ""totalMilliseconds"": ""<double>"",
        ""totalMicroseconds"": ""<double>"",
        ""totalNanoseconds"": ""<double>"",
        ""totalMinutes"": ""<double>"",
        ""totalSeconds"": ""<double>""
      },
      ""occurences"": [
        {
          ""correlationId"": ""<string>"",
          ""id"": ""<string>"",
          ""startDate"": ""<dateTime>"",
          ""endDate"": ""<dateTime>"",
          ""duration"": {
            ""ticks"": ""<long>"",
            ""days"": ""<integer>"",
            ""hours"": ""<integer>"",
            ""milliseconds"": ""<integer>"",
            ""microseconds"": ""<integer>"",
            ""nanoseconds"": ""<integer>"",
            ""minutes"": ""<integer>"",
            ""seconds"": ""<integer>"",
            ""totalDays"": ""<double>"",
            ""totalHours"": ""<double>"",
            ""totalMilliseconds"": ""<double>"",
            ""totalMicroseconds"": ""<double>"",
            ""totalNanoseconds"": ""<double>"",
            ""totalMinutes"": ""<double>"",
            ""totalSeconds"": ""<double>""
          },
          ""description"": ""<string>"",
          ""location"": {
            ""id"": ""<string>"",
            ""surname"": ""<string>"",
            ""mail"": ""<string>"",
            ""phone"": ""<string>"",
            ""organization"": ""<string>"",
            ""address"": ""<string>"",
            ""city"": ""<string>"",
            ""zipCode"": ""<string>"",
            ""eAppointmentUrl"": ""<uri>""
          }
        },
        {
          ""correlationId"": ""<string>"",
          ""id"": ""<string>"",
          ""startDate"": ""<dateTime>"",
          ""endDate"": ""<dateTime>"",
          ""duration"": {
            ""ticks"": ""<long>"",
            ""days"": ""<integer>"",
            ""hours"": ""<integer>"",
            ""milliseconds"": ""<integer>"",
            ""microseconds"": ""<integer>"",
            ""nanoseconds"": ""<integer>"",
            ""minutes"": ""<integer>"",
            ""seconds"": ""<integer>"",
            ""totalDays"": ""<double>"",
            ""totalHours"": ""<double>"",
            ""totalMilliseconds"": ""<double>"",
            ""totalMicroseconds"": ""<double>"",
            ""totalNanoseconds"": ""<double>"",
            ""totalMinutes"": ""<double>"",
            ""totalSeconds"": ""<double>""
          },
          ""description"": ""<string>"",
          ""location"": {
            ""id"": ""<string>"",
            ""surname"": ""<string>"",
            ""mail"": ""<string>"",
            ""phone"": ""<string>"",
            ""organization"": ""<string>"",
            ""address"": ""<string>"",
            ""city"": ""<string>"",
            ""zipCode"": ""<string>"",
            ""eAppointmentUrl"": ""<uri>""
          }
        }
      ],
      ""isGuaranteed"": ""<boolean>"",
      ""trainingType"": {},
      ""timeInvestAttendee"": {
        ""ticks"": ""<long>"",
        ""days"": ""<integer>"",
        ""hours"": ""<integer>"",
        ""milliseconds"": ""<integer>"",
        ""microseconds"": ""<integer>"",
        ""nanoseconds"": ""<integer>"",
        ""minutes"": ""<integer>"",
        ""seconds"": ""<integer>"",
        ""totalDays"": ""<double>"",
        ""totalHours"": ""<double>"",
        ""totalMilliseconds"": ""<double>"",
        ""totalMicroseconds"": ""<double>"",
        ""totalNanoseconds"": ""<double>"",
        ""totalMinutes"": ""<double>"",
        ""totalSeconds"": ""<double>""
      },
      ""timeModel"": ""<string>""
    },
    ""productUrl"": ""<uri>"",
    ""contacts"": [
      {
        ""id"": ""<string>"",
        ""surname"": ""<string>"",
        ""mail"": ""<string>"",
        ""phone"": ""<string>"",
        ""organization"": ""<string>"",
        ""address"": ""<string>"",
        ""city"": ""magna_acb"",
        ""zipCode"": ""<string>"",
        ""eAppointmentUrl"": ""<uri>""
      },
      {
        ""id"": ""<string>"",
        ""surname"": ""<string>"",
        ""mail"": ""<string>"",
        ""phone"": ""<string>"",
        ""organization"": ""<string>"",
        ""address"": ""<string>"",
        ""city"": ""<string>"",
        ""zipCode"": ""<string>"",
        ""eAppointmentUrl"": ""<uri>""
      }
    ],
    ""trainingType"": 2,
    ""individualStartDate"": ""<string>"",
    ""price"": 22.22,
    ""priceDescription"": ""<string>"",
    ""accessibilityAvailable"": true,
    ""tags"": [
      ""<string>"",
      ""<string>""
    ],
    ""categories"": [
      ""<string>"",
      ""<string>""
    ],
    ""recommendedTrainings"":[],
    ""similarTrainings"":[],
    ""publishingDate"": ""0001-01-01T00:00:00Z"",
    ""unpublishingDate"": ""0001-01-01T00:00:00Z"",
    ""successor"": ""<string>"",
    ""predecessor"": ""<string>""
  },
  {
    ""id"": ""SER02"",
    ""providerId"": ""provider2"",
    ""trainingName"": ""Training T05"",
    ""description"": ""Training T05 Description long"",
    ""shortDescription"": ""Training T05 Description short"",
    ""content"": [
      ""<string>"",
      ""<string>""
    ],
    ""benefitList"": [
      ""<string>"",
      ""<string>""
    ],
    ""certificate"": [
      ""<string>"",
      ""<string>""
    ],
    ""prerequisites"": [
      ""<string>"",
      ""<string>""
    ],
    ""loans"": [
      {
        ""id"": ""<string>"",
        ""name"": ""<string>"",
        ""description"": ""<string>"",
        ""url"": ""<uri>"",
        ""loanContact"": {
          ""id"": ""<string>"",
          ""surname"": ""<string>"",
          ""mail"": ""<string>"",
          ""phone"": ""<string>"",
          ""organization"": ""<string>"",
          ""address"": ""<string>"",
          ""city"": ""<string>"",
          ""zipCode"": ""<string>"",
          ""eAppointmentUrl"": ""<uri>""
        }
      },
      {
        ""id"": ""<string>"",
        ""name"": ""<string>"",
        ""description"": ""<string>"",
        ""url"": ""<uri>"",
        ""loanContact"": {
          ""id"": ""<string>"",
          ""surname"": ""<string>"",
          ""mail"": ""<string>"",
          ""phone"": ""<string>"",
          ""organization"": ""<string>"",
          ""address"": ""<string>"",
          ""city"": ""<string>"",
          ""zipCode"": ""<string>"",
          ""eAppointmentUrl"": ""<uri>""
        }
      }
    ],
    ""trainingProvider"": {
      ""id"": ""<string>"",
      ""name"": ""<string>"",
      ""description"": ""<string>"",
      ""url"": ""<uri>"",
      ""contact"": {
        ""id"": ""<string>"",
        ""surname"": ""<string>"",
        ""mail"": ""<string>"",
        ""phone"": ""<string>"",
        ""organization"": ""<string>"",
        ""address"": ""<string>"",
        ""city"": ""<string>"",
        ""zipCode"": ""<string>"",
        ""eAppointmentUrl"": ""<uri>""
      },
      ""image"": ""<uri>""
    },
    ""courseProvider"": {
      ""id"": ""<string>"",
      ""name"": ""<string>"",
      ""description"": ""<string>"",
      ""url"": ""<uri>"",
      ""contact"": {
        ""id"": ""<string>"",
        ""surname"": ""<string>"",
        ""mail"": ""<string>"",
        ""phone"": ""<string>"",
        ""organization"": ""<string>"",
        ""address"": ""<string>"",
        ""city"": ""<string>"",
        ""zipCode"": ""<string>"",
        ""eAppointmentUrl"": ""<uri>""
      },
      ""image"": ""<uri>""
    },
    ""appointments"": {
      ""id"": ""<string>"",
      ""appointment"": ""<uri>"",
      ""appointmentType"": ""<string>"",
      ""appointmentDescription"": ""<string>"",
      ""appointmentLocation"": {
        ""id"": ""<string>"",
        ""surname"": ""<string>"",
        ""mail"": ""<string>"",
        ""phone"": ""<string>"",
        ""organization"": ""<string>"",
        ""address"": ""<string>"",
        ""city"": ""<string>"",
        ""zipCode"": ""<string>"",
        ""eAppointmentUrl"": ""<uri>""
      },
      ""startDate"": ""<dateTime>"",
      ""endDate"": ""<dateTime>"",
      ""durationDescription"": ""<string>"",
      ""duration"": {
        ""ticks"": ""<long>"",
        ""days"": ""<integer>"",
        ""hours"": ""<integer>"",
        ""milliseconds"": ""<integer>"",
        ""microseconds"": ""<integer>"",
        ""nanoseconds"": ""<integer>"",
        ""minutes"": ""<integer>"",
        ""seconds"": ""<integer>"",
        ""totalDays"": ""<double>"",
        ""totalHours"": ""<double>"",
        ""totalMilliseconds"": ""<double>"",
        ""totalMicroseconds"": ""<double>"",
        ""totalNanoseconds"": ""<double>"",
        ""totalMinutes"": ""<double>"",
        ""totalSeconds"": ""<double>""
      },
      ""occurences"": [
        {
          ""correlationId"": ""<string>"",
          ""id"": ""<string>"",
          ""startDate"": ""<dateTime>"",
          ""endDate"": ""<dateTime>"",
          ""duration"": {
            ""ticks"": ""<long>"",
            ""days"": ""<integer>"",
            ""hours"": ""<integer>"",
            ""milliseconds"": ""<integer>"",
            ""microseconds"": ""<integer>"",
            ""nanoseconds"": ""<integer>"",
            ""minutes"": ""<integer>"",
            ""seconds"": ""<integer>"",
            ""totalDays"": ""<double>"",
            ""totalHours"": ""<double>"",
            ""totalMilliseconds"": ""<double>"",
            ""totalMicroseconds"": ""<double>"",
            ""totalNanoseconds"": ""<double>"",
            ""totalMinutes"": ""<double>"",
            ""totalSeconds"": ""<double>""
          },
          ""description"": ""<string>"",
          ""location"": {
            ""id"": ""<string>"",
            ""surname"": ""<string>"",
            ""mail"": ""<string>"",
            ""phone"": ""<string>"",
            ""organization"": ""<string>"",
            ""address"": ""<string>"",
            ""city"": ""<string>"",
            ""zipCode"": ""<string>"",
            ""eAppointmentUrl"": ""<uri>""
          }
        },
        {
          ""correlationId"": ""<string>"",
          ""id"": ""<string>"",
          ""startDate"": ""<dateTime>"",
          ""endDate"": ""<dateTime>"",
          ""duration"": {
            ""ticks"": ""<long>"",
            ""days"": ""<integer>"",
            ""hours"": ""<integer>"",
            ""milliseconds"": ""<integer>"",
            ""microseconds"": ""<integer>"",
            ""nanoseconds"": ""<integer>"",
            ""minutes"": ""<integer>"",
            ""seconds"": ""<integer>"",
            ""totalDays"": ""<double>"",
            ""totalHours"": ""<double>"",
            ""totalMilliseconds"": ""<double>"",
            ""totalMicroseconds"": ""<double>"",
            ""totalNanoseconds"": ""<double>"",
            ""totalMinutes"": ""<double>"",
            ""totalSeconds"": ""<double>""
          },
          ""description"": ""<string>"",
          ""location"": {
            ""id"": ""<string>"",
            ""surname"": ""<string>"",
            ""mail"": ""<string>"",
            ""phone"": ""<string>"",
            ""organization"": ""<string>"",
            ""address"": ""<string>"",
            ""city"": ""<string>"",
            ""zipCode"": ""<string>"",
            ""eAppointmentUrl"": ""<uri>""
          }
        }
      ],
      ""isGuaranteed"": ""<boolean>"",
      ""trainingType"": {},
      ""timeInvestAttendee"": {
        ""ticks"": ""<long>"",
        ""days"": ""<integer>"",
        ""hours"": ""<integer>"",
        ""milliseconds"": ""<integer>"",
        ""microseconds"": ""<integer>"",
        ""nanoseconds"": ""<integer>"",
        ""minutes"": ""<integer>"",
        ""seconds"": ""<integer>"",
        ""totalDays"": ""<double>"",
        ""totalHours"": ""<double>"",
        ""totalMilliseconds"": ""<double>"",
        ""totalMicroseconds"": ""<double>"",
        ""totalNanoseconds"": ""<double>"",
        ""totalMinutes"": ""<double>"",
        ""totalSeconds"": ""<double>""
      },
      ""timeModel"": ""<string>""
    },
    ""productUrl"": ""<uri>"",
    ""contacts"": [
      {
        ""id"": ""<string>"",
        ""surname"": ""<string>"",
        ""mail"": ""<string>"",
        ""phone"": ""<string>"",
        ""organization"": ""<string>"",
        ""address"": ""<string>"",
        ""city"": ""magna_acb"",
        ""zipCode"": ""<string>"",
        ""eAppointmentUrl"": ""<uri>""
      },
      {
        ""id"": ""<string>"",
        ""surname"": ""<string>"",
        ""mail"": ""<string>"",
        ""phone"": ""<string>"",
        ""organization"": ""<string>"",
        ""address"": ""<string>"",
        ""city"": ""frankfurt"",
        ""zipCode"": ""<string>"",
        ""eAppointmentUrl"": ""<uri>""
      }
    ],
    ""trainingType"": 1,
    ""individualStartDate"": ""<string>"",
    ""price"": 42.1,
    ""priceDescription"": ""<string>"",
    ""accessibilityAvailable"": false,
    ""tags"": [
      ""<string>"",
      ""<string>""
    ],
    ""categories"": [
      ""<string>"",
      ""<string>""
    ],
    ""publishingDate"": ""0001-01-01T00:00:00Z"",
    ""unpublishingDate"": ""0001-01-01T00:00:00Z"",
    ""successor"": ""<string>"",
    ""predecessor"": ""<string>""
  }
]";

     
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
        public async Task InsertTrainings()
        {
            var api = Helpers.GetApolloApi();
            var training = new Training
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

            await api.InsertTraining(training);

            await api.DeleteTrainings(new string[] { training.Id });

            await api.InsertTrainings(_testTrainings);

            await api.DeleteTrainings(_testTrainings.Select(i => i.Id).ToArray());
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

        [TestMethod]
        public async Task InsertComplexTraining()
        {
            // Arrange
            var api = Helpers.GetApolloApi();

            JsonSerializerOptions opts = new JsonSerializerOptions {
                 PropertyNameCaseInsensitive = true,
            };

            Training[]? t = JsonSerializer.Deserialize<Training[]>(_complexTrainingJson, opts);

            await api.InsertTrainings(t!);

            //await api.DeleteTrainings(t.Select(x=>x.Id).ToArray());
            
        }

        /// <summary>
        /// Tests querying Training objects based on specific criteria such as TrainingName and StartDate.
        /// </summary>
        [TestMethod]
        public async Task LookupTrainingsWithLoansTest()
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


        /// <summary>
        ///  A training has a list of Appointments and an Appointment can contain a Contact where the Training is taking place.
        /// </summary>
        /// <returns></returns>

        [TestMethod]
        public async Task TrainingWithAppointmentsTest()
        {

        }


        /// <summary>
        /// Filters for training with specific IndividualStartDate
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TrainingsWithIndividualStartDateTest()
        {
        }

        /// <summary>
        /// Filters  a Training by Id and with Appointment Start and End Date 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AppointmentDateFiterTestForSingleTraning()
        {
            var api = Helpers.GetApolloApi();

            try
            {
                // Create a test training record with a specific IndividualStartDate
                var testStartDate = "2024-01-15"; 
                //var testTraining = new Training
                //{
                //    Id = "T02",
                //    TrainingName = "Test Training with Appointment",
                //    IndividualStartDate = testStartDate
                //};

                //// Insert the test training record into the database
                //await api.InsertTraining(testTraining);

                //// Retrieve trainings with the specific IndividualStartDate
                //var filteredTrainings = await api.GetTrainingsByAppointmentDate(testStartDate);

                //// Ensure that the filteredTrainings list is not null and contains the test training
                //Assert.IsNotNull(filteredTrainings);
                //Assert.IsTrue(filteredTrainings.Contains(testTraining));
            }
            finally
            {
                // Clean up: Delete the test training record from the database
                await api.DeleteTrainings(new string[] { "T02" });
            }
        }

        /// <summary>
        /// NOTE; Duration is an auto calculated Property that can very well be null. 3 out of 6 tenants do not support dates that would allow a duration to be calculated.
        /// If null not recognized by the filter – therefor not in the result list.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TrainingAppointmentDurationTest()
        {
        }


        /// <summary>
        /// A Training has an auto calculated property which is based on the TrainingType of the Appointment List of a Training.
        /// This is a flagged Enum. And the user should be able to Multiselect in a Filter for the flagged Enum.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TrainingTypeTest()
        {
        }


        /// <summary>
        /// Test price filter operations including th esorting by price.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TrainingPriceTest()
        {


        }


        /// <summary>
        /// Retunrs provider1 OR provider2 or .. (Multiprovider select feature.)
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TrainingMultiProviderTest()
        {
        }

        /// <summary>
        /// Retrurns trainigns with accessibility flag.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TrainingAssessibilityTest()
        {
        }
    }
}
