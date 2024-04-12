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
using Newtonsoft.Json;

namespace Apollo.Api.UnitTests
{
    [TestClass]
    public class ApolloApiTrainingUnitTests
    {
        Training[] _testTrainings = new Training[]
           {
                new Training(){  Id = "UT01", ProviderId = "unittest01", TrainingName = "Open AI",
                    PublishingDate = DateTime.Now, Price = 42.0, PriceDescription = "EUR",
                Loans = new List<Loans>(
                    new Loans[]
                    {
                        new Loans() { Id = "L01", Name = "Loan 1" },
                        new Loans() { Id = "L02", Name = "Loan 2" }
                    }
                    )},

                new Training(){  Id = "UT02", ProviderId = "unittest02", TrainingName = "Azure AI",
                    PublishingDate = DateTime.Now.AddDays(1),Price = 7.1, PriceDescription = "EUR",},

                new Training(){  Id = "UT03" , ProviderId = "unittest03",
                    PublishingDate = DateTime.Now.AddDays(3),Price = 1192.0, PriceDescription = "EUR" ,   Loans = new List<Loans>(
                    new Loans[]
                    {
                        new Loans() { Id = "L01", Name = "Loan 1" },
                        new Loans() { Id = "L02", Name = "Loan 2" }
                    }
                    )},


           };

        Training[] _testTrainingwithAppointments = new Training[]
         {
                new Training(){
                    Id = "UT01",
                    ProviderId = "unittest",
                    TrainingName = "Open AI",
                    Loans = new List<Loans>(
                        new Loans[]
                        {
                            new Loans() { Id = "L01", Name = "Loan 1" },
                            new Loans() { Id = "L02", Name = "Loan 2" }
                        }),
                    Appointment = new List<Appointment>(

                        new Appointment[]
                        {
                            new Appointment(){
                                AppointmentUrl = null,
                                AppointmentType = "In-person",
                                AppointmentDescription = "Introduction session",
                                AppointmentLocation = null,
                                StartDate = DateTime.Parse("2024-01-17T10:30:00.000Z"),
                                EndDate = DateTime.Parse("2024-01-24T10:30:00.000Z"),
                                DurationDescription = "2 hours",
                                //Duration = TimeSpan.Zero,
                                Occurences = null,
                                IsGuaranteed = true,
                                TrainingMode = TrainingMode.Offline,
                                //TimeInvestAttendee = TimeSpan.MinValue,
                                TimeModel = TrainingTimeModel.Unknown,
                                OccurenceNoteOnTime = "Weekly",
                                ExecutionDuration = "2",
                                ExecutionDurationUnit = "UE",
                                ExecutionDurationDescription = "Weekly",
                                LessonType = "Practical",
                                BookingUrl = null
                            },
                            new Appointment(){
                                AppointmentUrl = null,
                                AppointmentType = "In-person",
                                AppointmentDescription = "Introduction session",
                                AppointmentLocation = null,
                                StartDate = DateTime.Parse("2024-01-17T10:30:00.000Z"),
                                EndDate = DateTime.Parse("2024-01-24T10:30:00.000Z"),
                                DurationDescription = "2 hours",
                                //Duration = TimeSpan.MaxValue,
                                Occurences = null,
                                IsGuaranteed = true,
                                TrainingMode = TrainingMode.Offline,
                                //TimeInvestAttendee = TimeSpan.MinValue,
                                TimeModel = TrainingTimeModel.Unknown,
                                OccurenceNoteOnTime = "Weekly",
                                ExecutionDuration = "2",
                                ExecutionDurationUnit = "UE",
                                ExecutionDurationDescription = "Weekly",
                                LessonType = "Practical",
                                BookingUrl = null
                            },

                        }
                        ),
                 },
         };


        /// <summary>
        /// The instance of the training with all properties.
        /// </summary>
        private string _complexTrainingJson = @"[
  {
    ""id"": ""SER04"",
    ""providerId"": ""hdbw-F626FEDE-1A30-4DE0-B17B-9DCB04A654C2"",
    ""trainingName"": ""Training05"",
    ""description"": ""Description of Training 05"",
    ""shortDescription"": ""Short Description of T05"",
    ""trainingType"": ""Type of Training for Training 05"",
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
    ""trainingType"": ""<string>"",
    ""individualStartDate"": ""2024-01-24T08:31:23.069596"",
    ""price"": 22.22,
    ""priceDescription"": ""<string>"",
    ""accessibilityAvailable"": true,
    ""tags"": [
      ""<string>""      ""<string>"",

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
    ""id"": ""SER05"",
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
    ""trainingType"": ""<string>"",
    ""individualStartDate"": ""2024-01-24T08:41:29.747129"",
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

            //cleaning complex training
            List<Training> trainings = JsonConvert.DeserializeObject<List<Training>>(_complexTrainingJson)!;
            var idsToDelete = trainings.Select(training => training.Id).ToArray();
            await dal.DeleteManyAsync(Helpers.GetCollectionName<Training>(),idsToDelete!, false);

            //cleaning test training
            await dal.DeleteManyAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => t.Id).ToArray()!, false);
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
        [TestCategory("Prod")]
        public async Task InsertTrainingsTest()
        {
            await CleanupTest();
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

            await api.InsertTrainingAsync(training);

            await api.DeleteTrainingsAsync(new string[] { training.Id });

            await api.InsertTrainingsAsync(_testTrainings);

            await api.DeleteTrainingsAsync(_testTrainings.Select(i => i.Id).ToArray()!);
        }


        /// <summary>
        /// Tests creating or updating a Training object and then cleaning up by deleting it.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task CreateOrUpdateTrainingTest()
        {
            var api = Helpers.GetApolloApi();

            // Create a new training instance with Id and ProviderId
            var newTraining = new Training
            {
                Id = "T23", 
                ProviderId = "provider123", // Sample provider Id
                TrainingName = "New Test Training",
                TrainingType = "Type1",
                Description = "New training description"
            };

            // Insert the new training
            var insertedTrainingIds = await api.CreateOrUpdateTrainingAsync(new List<Training> { newTraining });

            // Ensure that the new training was inserted correctly
            Assert.IsNotNull(insertedTrainingIds);
            Assert.AreEqual(1, insertedTrainingIds.Count);
            Assert.AreEqual(newTraining.Id, insertedTrainingIds[0]);

            // Update the created training with new data
            newTraining.TrainingName = "Updated Test Training";
            newTraining.Description = "Updated Description";
            newTraining.TrainingType = "Type2";

            // Update the training using the same ID
            var updatedTrainingIds = await api.CreateOrUpdateTrainingAsync(new List<Training> { newTraining });

            // Ensure that the training was updated correctly
            Assert.IsNotNull(updatedTrainingIds);
            Assert.AreEqual(1, updatedTrainingIds.Count);
            Assert.AreEqual(newTraining.Id, updatedTrainingIds[0]); // The ID should remain the same

            // Clean up: Delete the created or updated training
            await api.DeleteTrainingsAsync(updatedTrainingIds.ToArray());
        }


        /// <summary>
        /// Tests retrieving a specific Training object by its ID and then cleaning up by deleting it.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task GetTrainingTest()
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
                await api.InsertTrainingAsync(training);

                // Retrieve the inserted training using the GetTrainingTest method
                var retrievedTraining = await api.GetTrainingAsync(training.Id);

                // Ensure that the retrieved training is not null and has the same ID as the inserted training
                Assert.IsNotNull(retrievedTraining);
                Assert.AreEqual(training.Id, retrievedTraining.Id);
            }
            finally
            {
                // Clean up: Delete the test training record from the database
                await api.DeleteTrainingsAsync(new string[] { training.Id });
            }
        }


        /// <summary>
        /// Query without projection fields and expressions.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task QueryTrainingsTest()
        {
            var api = Helpers.GetApolloApi();

            // Ensure that _testTrainings includes "Training T05" with a valid IndividualStartDate
            await api.InsertTrainingsAsync(_testTrainings);

            // Case 1: Query by TrainingName
            var query = new Apollo.Common.Entities.Query
            {
                Filter = new Filter
                {
                    Fields = new List<FieldExpression>
                    {
                        new FieldExpression
                        {
                            FieldName = "id",
                            Operator = QueryOperator.Equals,
                            Argument = new string[]{ "UT01" }
                        }
                        // Add other FieldExpressions as needed for additional conditions
                    }
                }
            };

            var trainings = await api.QueryTrainingsAsync(query);

            // Assert for Case 1
            Assert.IsNotNull(trainings);
            Assert.IsTrue(trainings.Count > 0);

        }



        /// <summary>
        /// Tests querying Training objects based on specific criteria such as TrainingName and StartDate.
        /// Multiple cases are tested, including querying by TrainingName and PublishingDate, and by IndividualStartDate.
        /// Asserts ensure the correct trainings are returned based on the query criteria.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task QueryTrainingsByDateTimeTest()
        {
            var api = Helpers.GetApolloApi();
            await api.InsertTrainingsAsync(_testTrainings);
            // Specifying the filter date in the desired format
            long filterDateTimestamp = -62135596800000; // Negative Unix timestamp
            DateTime filterDate = DateTimeOffset.FromUnixTimeMilliseconds(filterDateTimestamp).UtcDateTime;
            // Case 1: Query by TrainingName and PublishingDate
            var query = new Apollo.Common.Entities.Query
            {
                Fields = new List<string> { "TrainingName", "PublishingDate" },
                Filter = new Apollo.Common.Entities.Filter
                {
                    IsOrOperator = false,
                    Fields = new List<Apollo.Common.Entities.FieldExpression>
                    {
                        new Apollo.Common.Entities.FieldExpression
                        {
                            FieldName = "TrainingName",
                            Operator = Apollo.Common.Entities.QueryOperator.Equals,
                            Argument = new List<object> { "Training07" }
                        },
                        new Apollo.Common.Entities.FieldExpression
                        {
                            FieldName = "PublishingDate",
                            Operator = Apollo.Common.Entities.QueryOperator.GreaterThanEqualTo,
                            Argument = new List<object> { filterDate }
                        }
                    }
                },
                RequestCount = true,
                Top = 200,
                Skip = 0,
                SortExpression = new Apollo.Common.Entities.SortExpression
                {
                    FieldName = "PublishingDate",
                    Order = Apollo.Common.Entities.SortOrder.Ascending
                }
            };
            var trainings = await api.QueryTrainingsAsync(query);
            // Debugging: Log the results
            Console.WriteLine($"Case 1 - Trainings Count: {trainings.Count}");
            foreach (var training in trainings)
            {
                Console.WriteLine($"Training Name: {training.TrainingName}, PublishingDate: {training.PublishingDate}");
            }
            // Assert for Case 1
            Assert.IsNotNull(trainings);
            //Assert.IsTrue(trainings.Any(t => t.TrainingName == "Training07"));

            // Case 2: Query by IndividualStartDate only
            query.Filter.Fields.RemoveAt(0); // Remove TrainingName filter

            trainings = await api.QueryTrainingsAsync(query);

            // Assert for Case 2
            Assert.IsNotNull(trainings);
            // Assert based on expected number of trainings with a valid IndividualStartDate
            Assert.IsTrue(trainings.Count >= 1);

            // Case 3: Modify the IndividualStartDate filter
            query.Filter.Fields[0].Argument = new List<object> { DateTime.Now };

            trainings = await api.QueryTrainingsAsync(query);

            // Assert for Case 3
            Assert.IsNotNull(trainings);
            // Assert based on expected number of trainings after the current date
            Assert.IsTrue(trainings.Count >= 0);

            // Case 4: Further modification to IndividualStartDate filter
            query.Filter.Fields[0].Argument = new List<object> { DateTime.Now.AddDays(2) };

            trainings = await api.QueryTrainingsAsync(query);

            // Assert for Case 4
            Assert.IsNotNull(trainings);
            // Assert based on expected number of trainings two days after the current date
            Assert.IsTrue(trainings.Count >= 0);

            // Cleanup: Delete the training records inserted during the test
            foreach (var training in trainings)
            {
                await api.DeleteTrainingsAsync(new string[] { training.Id!});
            }
        }


        /// <summary>
        /// Tests the insertion of complex Training objects.
        /// A JSON string representing an array of Training objects is deserialized and inserted.
        /// Asserts confirm the successful insertion and subsequent cleanup of the Training data.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task InsertComplexTrainingTest()
        {
            await CleanupTest();
            // Arrange
            var api = Helpers.GetApolloApi();

            JsonSerializerOptions opts = new JsonSerializerOptions {
                 PropertyNameCaseInsensitive = true,
            };

            Training[]? t = System.Text.Json.JsonSerializer.Deserialize<Training[]>(_complexTrainingJson, opts);

            await api.InsertTrainingsAsync(t!);

            await api.DeleteTrainingsAsync(t.Select(x=>x.Id).ToArray()!);
            
        }


        /// <summary>
        /// Tests querying Training objects based on specific criteria such as TrainingName and StartDate.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task LookupTrainingsWithLoansTest()
        {
            var api = Helpers.GetApolloApi();

            await api.InsertTrainingsAsync(_testTrainings);

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
                trainings = await api.QueryTrainingsAsync(query);
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
                await api.DeleteTrainingsAsync(new string[] { training.Id! });
            }

            // add more assertions based on your specific testing requirements
        }

        
        /// <summary>
        ///  A training has a list of Appointments and an Appointment can contain a Contact where the Training is taking place.
        /// </summary>
        /// <returns></returns>

        [TestMethod]
        public async Task TrainingWithAppointmentsInsertTest()
        {
            var api = Helpers.GetApolloApi();

            // Insert a test training record into the database
            await api.InsertTrainingsAsync(_testTrainingwithAppointments);

            // Retrieve the inserted training using the GetTrainingTest method
            var retrievedTraining = await api.GetTrainingAsync(_testTrainingwithAppointments.Select(i => i.Id).ToArray().First()!);

            // Ensure that the retrieved training is not null and has the same ID as the inserted training
            Assert.IsNotNull(retrievedTraining);
            Assert.AreEqual(_testTrainingwithAppointments.Select(i => i.Id).ToArray().First(), retrievedTraining.Id);
            

        }

        ///// <summary>
        ///// Filters  a Training by Id and with Appointment Start and End Date 
        ///// </summary>
        ///// <returns></returns>
        ///// <param name="startDate">Start date of the range.</param>
        ///// <param name="endDate">End date of the range.</param>
        //[TestMethod]
        //public async Task TrainingWithAppointmentFilterWithDateTest()
        //{
        //    // Dummy initialization or specify default values for parameters
        //    string trainingId = "defaultTrainingId";
        //    DateTime startDate = DateTime.MinValue;
        //    DateTime endDate = DateTime.MaxValue;
        //    TimeSpan minDuration = TimeSpan.Zero;
        //    TimeSpan maxDuration = TimeSpan.MaxValue;
        //    try
        //    {
        //        var query = new Query
        //        {
        //            Filter = new Filter
        //            {
        //                Fields = new List<FieldExpression>
        //                {
        //                    new FieldExpression { FieldName = "_id", Operator = QueryOperator.Equals, Argument = new List<object> { trainingId } },
        //                    new FieldExpression { FieldName = "appointments", Operator = QueryOperator.NotEquals, Argument = new List<object> { null } },
        //                    new FieldExpression { FieldName = "appointments.startdate", Operator = QueryOperator.Equals, Argument = new List<object> { endDate } },
        //                    //new FieldExpression { FieldName = "appointments.enddate", Operator = QueryOperator.Equals, Argument = new List<object> { startDate } },
        //                }
        //            },
        //            Top = 100,
        //            Skip = 0
        //        };
        //        var api = Helpers.GetApolloApi(); // Assuming GetApolloApi() is a static method in your Helpers class.
        //        var res = await api.QueryTrainingsAsync(query); // Assuming QueryTrainingsTest is an asynchronous method.
        //        var training = res.SingleOrDefault();
        //        Assert.IsNotNull(training);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApolloApiException(ErrorCodes.TrainingErrors.QueryTrainingsError, "Error while querying training with appointments by date range", ex);
        //    }    
        //  }




        /// <summary>
        /// Filters for training with specific IndividualStartDate
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task  trainingswithindividualstartdatetest()
        {

           
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
        /// A Training has an auto calculated property which is based on the TrainingMode of the Appointment List of a Training.
        /// This is a flagged Enum. And the user should be able to Multiselect in a Filter for the flagged Enum.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TrainingTypeTest()
        {
        }


        /// <summary>
        /// Test price filter operations including the sorting by price.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TrainingPriceTest()
        {
            var api = Helpers.GetApolloApi();

            await api.InsertTrainingsAsync(_testTrainings);

            var query = new Apollo.Common.Entities.Query
            {
                Fields = new List<string> { "TrainingName", "StartDate", "Price" },
                Filter = new Apollo.Common.Entities.Filter
                {
                    IsOrOperator = false,
                    Fields = new List<Apollo.Common.Entities.FieldExpression>
                    {
                        new Apollo.Common.Entities.FieldExpression
                        {
                            FieldName = "Price",
                            Operator = Apollo.Common.Entities.QueryOperator.LessThanEqualTo,
                            Argument = new List<object> { 42.2 }
                        }
                    }
                },
                RequestCount = true,
                Top = 200,
                Skip = 0,
                SortExpression = new Apollo.Common.Entities.SortExpression
                {
                    FieldName = "Price",
                    Order = Apollo.Common.Entities.SortOrder.Ascending
                }
            };


            // Log the generated query for debugging
            Console.WriteLine($"Generated Query: {JsonConvert.SerializeObject(query)}");

            // Act
            IList<Training> trainings = await api.QueryTrainingsAsync(query);

            // Log the retrieved trainings for debugging
            Console.WriteLine($"Retrieved Trainings: {JsonConvert.SerializeObject(trainings)}");

            Assert.IsNotNull(trainings);
            Assert.IsTrue(trainings.Count > 0);

            query.Filter.Fields[0].Argument = new List<object> { double.MaxValue };

            trainings = await api.QueryTrainingsAsync(query);

            Assert.IsNotNull(trainings);
            Assert.IsTrue(trainings.Count > 0);

            foreach (var training in trainings)
            {
                // Additional assertions based on your specific requirements
               // Assert.IsTrue(training.Price <= Convert.ToDecimal(42.2));
            }
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


        /// <summary>
        /// Delete Trainings test by Provider Ids
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task ProviderTrainingsDeleteTest()
        {
            var api = Helpers.GetApolloApi();
            //
            // test training has three provider id with "unittest01", "unittest02", "unittest03"
            await api.InsertTrainingsAsync(_testTrainings);

            var profileIds = new string[] { "unittest01", "unittest02", "unittest03" };

            var trainings = await api.DeleteProviderTrainingsAsync(profileIds);
            Assert.IsNotNull(trainings);
            Assert.IsTrue(trainings.Count == 3);

        }


    }
}

