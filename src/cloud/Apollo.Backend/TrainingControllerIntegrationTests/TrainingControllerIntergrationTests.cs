// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Apollo.Common.Entities;
using Apollo.RestService.IntergrationTests;
using Apollo.RestService.Messages;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Driver.Core.Authentication;


namespace Apollo.RestService.IntergrationTests
{

    /// <summary>
    /// Integration tests for the TrainingController class.
    /// </summary>
    [TestClass]
    public class TrainingControllerIntegrationTests
    {
        private const string _cTrainingController = "Training";

        Training[] _testTrainings = new Training[]
   {
        new Training(){
                    Id = "IT01",
                    ProviderId = "intergrationtest",
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




        ///// <summary>
        ///// The instance of the training with all properties.
        ///// </summary>
        //private string _complexTrainingJson = @"[
        //  {
        //    ""id"": ""IT01"",
        //    ""providerId"": ""intergrationtest"",
        //    ""trainingName"": ""Training01"",
        //    ""description"": ""Description of Training 01"",
        //    ""shortDescription"": ""Short Description of 01"",
        //    ""trainingType"": ""Type of Training for Training 01"",
        //    ""content"": [
        //      ""<string>"",
        //      ""<string>""
        //    ],
        //    ""benefitList"": [
        //      ""<string>"",
        //      ""<string>""
        //    ],
        //    ""certificate"": [
        //      ""<string>"",
        //      ""<string>""
        //    ],
        //    ""prerequisites"": [
        //      ""<string>"",
        //      ""<string>""
        //    ],
        //    ""loans"": [
        //      {
        //        ""id"": ""<string>"",
        //        ""name"": ""<string>"",
        //        ""description"": ""<string>"",
        //        ""url"": ""<uri>"",
        //        ""loanContact"": {
        //          ""id"": ""<string>"",
        //          ""surname"": ""<string>"",
        //          ""mail"": ""<string>"",
        //          ""phone"": ""<string>"",
        //          ""organization"": ""<string>"",
        //          ""address"": ""<string>"",
        //          ""city"": ""<string>"",
        //          ""zipCode"": ""<string>"",
        //          ""eAppointmentUrl"": ""<uri>""
        //        }
        //      },
        //      {
        //        ""id"": ""<string>"",
        //        ""name"": ""<string>"",
        //        ""description"": ""<string>"",
        //        ""url"": ""<uri>"",
        //        ""loanContact"": {
        //          ""id"": ""<string>"",
        //          ""surname"": ""<string>"",
        //          ""mail"": ""<string>"",
        //          ""phone"": ""<string>"",
        //          ""organization"": ""<string>"",
        //          ""address"": ""<string>"",
        //          ""city"": ""<string>"",
        //          ""zipCode"": ""<string>"",
        //          ""eAppointmentUrl"": ""<uri>""
        //        }
        //      }
        //    ],
        //    ""trainingProvider"": {
        //      ""id"": ""<string>"",
        //      ""name"": ""<string>"",
        //      ""description"": ""<string>"",
        //      ""url"": ""<uri>"",
        //      ""contact"": {
        //        ""id"": ""<string>"",
        //        ""surname"": ""<string>"",
        //        ""mail"": ""<string>"",
        //        ""phone"": ""<string>"",
        //        ""organization"": ""<string>"",
        //        ""address"": ""<string>"",
        //        ""city"": ""<string>"",
        //        ""zipCode"": ""<string>"",
        //        ""eAppointmentUrl"": ""<uri>""
        //      },
        //      ""image"": ""<uri>""
        //    },
        //    ""courseProvider"": {
        //      ""id"": ""<string>"",
        //      ""name"": ""<string>"",
        //      ""description"": ""<string>"",
        //      ""url"": ""<uri>"",
        //      ""contact"": {
        //        ""id"": ""<string>"",
        //        ""surname"": ""<string>"",
        //        ""mail"": ""<string>"",
        //        ""phone"": ""<string>"",
        //        ""organization"": ""<string>"",
        //        ""address"": ""<string>"",
        //        ""city"": ""<string>"",
        //        ""zipCode"": ""<string>"",
        //        ""eAppointmentUrl"": ""<uri>""
        //      },
        //      ""image"": ""<uri>""
        //    },
        //    ""appointments"": {
        //      ""id"": ""<string>"",
        //      ""appointment"": ""<uri>"",
        //      ""appointmentType"": ""<string>"",
        //      ""appointmentDescription"": ""<string>"",
        //      ""appointmentLocation"": {
        //        ""id"": ""<string>"",
        //        ""surname"": ""<string>"",
        //        ""mail"": ""<string>"",
        //        ""phone"": ""<string>"",
        //        ""organization"": ""<string>"",
        //        ""address"": ""<string>"",
        //        ""city"": ""<string>"",
        //        ""zipCode"": ""<string>"",
        //        ""eAppointmentUrl"": ""<uri>""
        //      },
        //      ""startDate"": ""<dateTime>"",
        //      ""endDate"": ""<dateTime>"",
        //      ""durationDescription"": ""<string>"",
        //      ""duration"": {
        //        ""ticks"": ""<long>"",
        //        ""days"": ""<integer>"",
        //        ""hours"": ""<integer>"",
        //        ""milliseconds"": ""<integer>"",
        //        ""microseconds"": ""<integer>"",
        //        ""nanoseconds"": ""<integer>"",
        //        ""minutes"": ""<integer>"",
        //        ""seconds"": ""<integer>"",
        //        ""totalDays"": ""<double>"",
        //        ""totalHours"": ""<double>"",
        //        ""totalMilliseconds"": ""<double>"",
        //        ""totalMicroseconds"": ""<double>"",
        //        ""totalNanoseconds"": ""<double>"",
        //        ""totalMinutes"": ""<double>"",
        //        ""totalSeconds"": ""<double>""
        //      },
        //      ""occurences"": [
        //        {
        //          ""correlationId"": ""<string>"",
        //          ""id"": ""<string>"",
        //          ""startDate"": ""<dateTime>"",
        //          ""endDate"": ""<dateTime>"",
        //          ""duration"": {
        //            ""ticks"": ""<long>"",
        //            ""days"": ""<integer>"",
        //            ""hours"": ""<integer>"",
        //            ""milliseconds"": ""<integer>"",
        //            ""microseconds"": ""<integer>"",
        //            ""nanoseconds"": ""<integer>"",
        //            ""minutes"": ""<integer>"",
        //            ""seconds"": ""<integer>"",
        //            ""totalDays"": ""<double>"",
        //            ""totalHours"": ""<double>"",
        //            ""totalMilliseconds"": ""<double>"",
        //            ""totalMicroseconds"": ""<double>"",
        //            ""totalNanoseconds"": ""<double>"",
        //            ""totalMinutes"": ""<double>"",
        //            ""totalSeconds"": ""<double>""
        //          },
        //          ""description"": ""<string>"",
        //          ""location"": {
        //            ""id"": ""<string>"",
        //            ""surname"": ""<string>"",
        //            ""mail"": ""<string>"",
        //            ""phone"": ""<string>"",
        //            ""organization"": ""<string>"",
        //            ""address"": ""<string>"",
        //            ""city"": ""<string>"",
        //            ""zipCode"": ""<string>"",
        //            ""eAppointmentUrl"": ""<uri>""
        //          }
        //        },
        //        {
        //          ""correlationId"": ""<string>"",
        //          ""id"": ""<string>"",
        //          ""startDate"": ""<dateTime>"",
        //          ""endDate"": ""<dateTime>"",
        //          ""duration"": {
        //            ""ticks"": ""<long>"",
        //            ""days"": ""<integer>"",
        //            ""hours"": ""<integer>"",
        //            ""milliseconds"": ""<integer>"",
        //            ""microseconds"": ""<integer>"",
        //            ""nanoseconds"": ""<integer>"",
        //            ""minutes"": ""<integer>"",
        //            ""seconds"": ""<integer>"",
        //            ""totalDays"": ""<double>"",
        //            ""totalHours"": ""<double>"",
        //            ""totalMilliseconds"": ""<double>"",
        //            ""totalMicroseconds"": ""<double>"",
        //            ""totalNanoseconds"": ""<double>"",
        //            ""totalMinutes"": ""<double>"",
        //            ""totalSeconds"": ""<double>""
        //          },
        //          ""description"": ""<string>"",
        //          ""location"": {
        //            ""id"": ""<string>"",
        //            ""surname"": ""<string>"",
        //            ""mail"": ""<string>"",
        //            ""phone"": ""<string>"",
        //            ""organization"": ""<string>"",
        //            ""address"": ""<string>"",
        //            ""city"": ""<string>"",
        //            ""zipCode"": ""<string>"",
        //            ""eAppointmentUrl"": ""<uri>""
        //          }
        //        }
        //      ],
        //      ""isGuaranteed"": ""<boolean>"",
        //      ""trainingType"": {},
        //      ""timeInvestAttendee"": {
        //        ""ticks"": ""<long>"",
        //        ""days"": ""<integer>"",
        //        ""hours"": ""<integer>"",
        //        ""milliseconds"": ""<integer>"",
        //        ""microseconds"": ""<integer>"",
        //        ""nanoseconds"": ""<integer>"",
        //        ""minutes"": ""<integer>"",
        //        ""seconds"": ""<integer>"",
        //        ""totalDays"": ""<double>"",
        //        ""totalHours"": ""<double>"",
        //        ""totalMilliseconds"": ""<double>"",
        //        ""totalMicroseconds"": ""<double>"",
        //        ""totalNanoseconds"": ""<double>"",
        //        ""totalMinutes"": ""<double>"",
        //        ""totalSeconds"": ""<double>""
        //      },
        //      ""timeModel"": ""<string>""
        //    },
        //    ""productUrl"": ""<uri>"",
        //    ""contacts"": [
        //      {
        //        ""id"": ""<string>"",
        //        ""surname"": ""<string>"",
        //        ""mail"": ""<string>"",
        //        ""phone"": ""<string>"",
        //        ""organization"": ""<string>"",
        //        ""address"": ""<string>"",
        //        ""city"": ""magna_acb"",
        //        ""zipCode"": ""<string>"",
        //        ""eAppointmentUrl"": ""<uri>""
        //      },
        //      {
        //        ""id"": ""<string>"",
        //        ""surname"": ""<string>"",
        //        ""mail"": ""<string>"",
        //        ""phone"": ""<string>"",
        //        ""organization"": ""<string>"",
        //        ""address"": ""<string>"",
        //        ""city"": ""<string>"",
        //        ""zipCode"": ""<string>"",
        //        ""eAppointmentUrl"": ""<uri>""
        //      }
        //    ],
        //    ""trainingType"": ""<string>"",
        //    ""individualStartDate"": ""2024-01-24T08:31:23.069596"",
        //    ""price"": 22.22,
        //    ""priceDescription"": ""<string>"",
        //    ""accessibilityAvailable"": true,
        //    ""tags"": [
        //      ""<string>"",
        //      ""<string>""
        //    ],
        //    ""categories"": [
        //      ""<string>"",
        //      ""<string>""
        //    ],
        //    ""recommendedTrainings"":[],
        //    ""similarTrainings"":[],
        //    ""publishingDate"": ""0001-01-01T00:00:00Z"",
        //    ""unpublishingDate"": ""0001-01-01T00:00:00Z"",
        //    ""successor"": ""<string>"",
        //    ""predecessor"": ""<string>""
        //  },
        //  {
        //    ""id"": ""SER05"",
        //    ""providerId"": ""provider2"",
        //    ""trainingName"": ""Training T05"",
        //    ""description"": ""Training T05 Description long"",
        //    ""shortDescription"": ""Training T05 Description short"",
        //    ""content"": [
        //      ""<string>"",
        //      ""<string>""
        //    ],
        //    ""benefitList"": [
        //      ""<string>"",
        //      ""<string>""
        //    ],
        //    ""certificate"": [
        //      ""<string>"",
        //      ""<string>""
        //    ],
        //    ""prerequisites"": [
        //      ""<string>"",
        //      ""<string>""
        //    ],
        //    ""loans"": [
        //      {
        //        ""id"": ""<string>"",
        //        ""name"": ""<string>"",
        //        ""description"": ""<string>"",
        //        ""url"": ""<uri>"",
        //        ""loanContact"": {
        //          ""id"": ""<string>"",
        //          ""surname"": ""<string>"",
        //          ""mail"": ""<string>"",
        //          ""phone"": ""<string>"",
        //          ""organization"": ""<string>"",
        //          ""address"": ""<string>"",
        //          ""city"": ""<string>"",
        //          ""zipCode"": ""<string>"",
        //          ""eAppointmentUrl"": ""<uri>""
        //        }
        //      },
        //      {
        //        ""id"": ""<string>"",
        //        ""name"": ""<string>"",
        //        ""description"": ""<string>"",
        //        ""url"": ""<uri>"",
        //        ""loanContact"": {
        //          ""id"": ""<string>"",
        //          ""surname"": ""<string>"",
        //          ""mail"": ""<string>"",
        //          ""phone"": ""<string>"",
        //          ""organization"": ""<string>"",
        //          ""address"": ""<string>"",
        //          ""city"": ""<string>"",
        //          ""zipCode"": ""<string>"",
        //          ""eAppointmentUrl"": ""<uri>""
        //        }
        //      }
        //    ],
        //    ""trainingProvider"": {
        //      ""id"": ""<string>"",
        //      ""name"": ""<string>"",
        //      ""description"": ""<string>"",
        //      ""url"": ""<uri>"",
        //      ""contact"": {
        //        ""id"": ""<string>"",
        //        ""surname"": ""<string>"",
        //        ""mail"": ""<string>"",
        //        ""phone"": ""<string>"",
        //        ""organization"": ""<string>"",
        //        ""address"": ""<string>"",
        //        ""city"": ""<string>"",
        //        ""zipCode"": ""<string>"",
        //        ""eAppointmentUrl"": ""<uri>""
        //      },
        //      ""image"": ""<uri>""
        //    },
        //    ""courseProvider"": {
        //      ""id"": ""<string>"",
        //      ""name"": ""<string>"",
        //      ""description"": ""<string>"",
        //      ""url"": ""<uri>"",
        //      ""contact"": {
        //        ""id"": ""<string>"",
        //        ""surname"": ""<string>"",
        //        ""mail"": ""<string>"",
        //        ""phone"": ""<string>"",
        //        ""organization"": ""<string>"",
        //        ""address"": ""<string>"",
        //        ""city"": ""<string>"",
        //        ""zipCode"": ""<string>"",
        //        ""eAppointmentUrl"": ""<uri>""
        //      },
        //      ""image"": ""<uri>""
        //    },
        //    ""appointments"": {
        //      ""id"": ""<string>"",
        //      ""appointment"": ""<uri>"",
        //      ""appointmentType"": ""<string>"",
        //      ""appointmentDescription"": ""<string>"",
        //      ""appointmentLocation"": {
        //        ""id"": ""<string>"",
        //        ""surname"": ""<string>"",
        //        ""mail"": ""<string>"",
        //        ""phone"": ""<string>"",
        //        ""organization"": ""<string>"",
        //        ""address"": ""<string>"",
        //        ""city"": ""<string>"",
        //        ""zipCode"": ""<string>"",
        //        ""eAppointmentUrl"": ""<uri>""
        //      },
        //      ""startDate"": ""<dateTime>"",
        //      ""endDate"": ""<dateTime>"",
        //      ""durationDescription"": ""<string>"",
        //      ""duration"": {
        //        ""ticks"": ""<long>"",
        //        ""days"": ""<integer>"",
        //        ""hours"": ""<integer>"",
        //        ""milliseconds"": ""<integer>"",
        //        ""microseconds"": ""<integer>"",
        //        ""nanoseconds"": ""<integer>"",
        //        ""minutes"": ""<integer>"",
        //        ""seconds"": ""<integer>"",
        //        ""totalDays"": ""<double>"",
        //        ""totalHours"": ""<double>"",
        //        ""totalMilliseconds"": ""<double>"",
        //        ""totalMicroseconds"": ""<double>"",
        //        ""totalNanoseconds"": ""<double>"",
        //        ""totalMinutes"": ""<double>"",
        //        ""totalSeconds"": ""<double>""
        //      },
        //      ""occurences"": [
        //        {
        //          ""correlationId"": ""<string>"",
        //          ""id"": ""<string>"",
        //          ""startDate"": ""<dateTime>"",
        //          ""endDate"": ""<dateTime>"",
        //          ""duration"": {
        //            ""ticks"": ""<long>"",
        //            ""days"": ""<integer>"",
        //            ""hours"": ""<integer>"",
        //            ""milliseconds"": ""<integer>"",
        //            ""microseconds"": ""<integer>"",
        //            ""nanoseconds"": ""<integer>"",
        //            ""minutes"": ""<integer>"",
        //            ""seconds"": ""<integer>"",
        //            ""totalDays"": ""<double>"",
        //            ""totalHours"": ""<double>"",
        //            ""totalMilliseconds"": ""<double>"",
        //            ""totalMicroseconds"": ""<double>"",
        //            ""totalNanoseconds"": ""<double>"",
        //            ""totalMinutes"": ""<double>"",
        //            ""totalSeconds"": ""<double>""
        //          },
        //          ""description"": ""<string>"",
        //          ""location"": {
        //            ""id"": ""<string>"",
        //            ""surname"": ""<string>"",
        //            ""mail"": ""<string>"",
        //            ""phone"": ""<string>"",
        //            ""organization"": ""<string>"",
        //            ""address"": ""<string>"",
        //            ""city"": ""<string>"",
        //            ""zipCode"": ""<string>"",
        //            ""eAppointmentUrl"": ""<uri>""
        //          }
        //        },
        //        {
        //          ""correlationId"": ""<string>"",
        //          ""id"": ""<string>"",
        //          ""startDate"": ""<dateTime>"",
        //          ""endDate"": ""<dateTime>"",
        //          ""duration"": {
        //            ""ticks"": ""<long>"",
        //            ""days"": ""<integer>"",
        //            ""hours"": ""<integer>"",
        //            ""milliseconds"": ""<integer>"",
        //            ""microseconds"": ""<integer>"",
        //            ""nanoseconds"": ""<integer>"",
        //            ""minutes"": ""<integer>"",
        //            ""seconds"": ""<integer>"",
        //            ""totalDays"": ""<double>"",
        //            ""totalHours"": ""<double>"",
        //            ""totalMilliseconds"": ""<double>"",
        //            ""totalMicroseconds"": ""<double>"",
        //            ""totalNanoseconds"": ""<double>"",
        //            ""totalMinutes"": ""<double>"",
        //            ""totalSeconds"": ""<double>""
        //          },
        //          ""description"": ""<string>"",
        //          ""location"": {
        //            ""id"": ""<string>"",
        //            ""surname"": ""<string>"",
        //            ""mail"": ""<string>"",
        //            ""phone"": ""<string>"",
        //            ""organization"": ""<string>"",
        //            ""address"": ""<string>"",
        //            ""city"": ""<string>"",
        //            ""zipCode"": ""<string>"",
        //            ""eAppointmentUrl"": ""<uri>""
        //          }
        //        }
        //      ],
        //      ""isGuaranteed"": ""<boolean>"",
        //      ""trainingType"": {},
        //      ""timeInvestAttendee"": {
        //        ""ticks"": ""<long>"",
        //        ""days"": ""<integer>"",
        //        ""hours"": ""<integer>"",
        //        ""milliseconds"": ""<integer>"",
        //        ""microseconds"": ""<integer>"",
        //        ""nanoseconds"": ""<integer>"",
        //        ""minutes"": ""<integer>"",
        //        ""seconds"": ""<integer>"",
        //        ""totalDays"": ""<double>"",
        //        ""totalHours"": ""<double>"",
        //        ""totalMilliseconds"": ""<double>"",
        //        ""totalMicroseconds"": ""<double>"",
        //        ""totalNanoseconds"": ""<double>"",
        //        ""totalMinutes"": ""<double>"",
        //        ""totalSeconds"": ""<double>""
        //      },
        //      ""timeModel"": ""<string>""
        //    },
        //    ""productUrl"": ""<uri>"",
        //    ""contacts"": [
        //      {
        //        ""id"": ""<string>"",
        //        ""surname"": ""<string>"",
        //        ""mail"": ""<string>"",
        //        ""phone"": ""<string>"",
        //        ""organization"": ""<string>"",
        //        ""address"": ""<string>"",
        //        ""city"": ""magna_acb"",
        //        ""zipCode"": ""<string>"",
        //        ""eAppointmentUrl"": ""<uri>""
        //      },
        //      {
        //        ""id"": ""<string>"",
        //        ""surname"": ""<string>"",
        //        ""mail"": ""<string>"",
        //        ""phone"": ""<string>"",
        //        ""organization"": ""<string>"",
        //        ""address"": ""<string>"",
        //        ""city"": ""frankfurt"",
        //        ""zipCode"": ""<string>"",
        //        ""eAppointmentUrl"": ""<uri>""
        //      }
        //    ],
        //    ""trainingType"": ""<string>"",
        //    ""individualStartDate"": ""2024-01-24T08:41:29.747129"",
        //    ""price"": 42.1,
        //    ""priceDescription"": ""<string>"",
        //    ""accessibilityAvailable"": false,
        //    ""tags"": [
        //      ""<string>"",
        //      ""<string>""
        //    ],
        //    ""categories"": [
        //      ""<string>"",
        //      ""<string>""
        //    ],
        //    ""publishingDate"": ""0001-01-01T00:00:00Z"",
        //    ""unpublishingDate"": ""0001-01-01T00:00:00Z"",
        //    ""successor"": ""<string>"",
        //    ""predecessor"": ""<string>""
        //  }
        //]";


        public TrainingControllerIntegrationTests()
        {

        }


        ///// <summary>
        ///// Initialization method to insert test data before each test.
        ///// </summary>
        [TestInitialize]
        public async Task InitTest()
        {
         
        }


        /// <summary>
        /// Inserts a predefined list of training objects into the database for testing purposes.
        /// This method serializes the list of training objects to JSON, sends it to the training insert API endpoint,
        /// and verifies the success of the operation by checking the response status code and the content.
        /// Before and after insertion, it ensures the clean-up of test data to maintain test environment integrity.
        /// </summary>
        public async Task InsertTestTrainings()
        {

            var httpClient = Helpers.GetHttpClient();

            var json = JsonSerializer.Serialize(_testTrainings);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await httpClient.PostAsync($"{_cTrainingController}/insert", content);

            Assert.IsTrue(res.IsSuccessStatusCode);

            var resjson = await res.Content.ReadAsStringAsync();
            var insertedIds = JsonSerializer.Deserialize<List<string>>(resjson);
            Assert.IsNotNull(insertedIds, "The response should include IDs of inserted trainings.");
            Assert.AreEqual(_testTrainings.Length, insertedIds.Count, "The number of inserted trainings should match the input.");

        }


        /// <summary>
        /// Makes sure that the GetTraining endpoint returns a correct training instance.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetTrainingTest()
        {
            await InsertTestTrainings();

            var httpClient = Helpers.GetHttpClient();

            try
            {
                foreach (var testTraining in _testTrainings)
                {
                    // GET the training by ID and verify the response is successful
                    var response = await httpClient.GetAsync($"{_cTrainingController}/{testTraining.Id}");
                    Assert.IsTrue(response.IsSuccessStatusCode);

                    // Deserialize the response content to a Training object
                    var trainingJson = await response.Content.ReadAsStringAsync();

                    var trainingWrapper = JsonSerializer.Deserialize<TrainingWrapper>(trainingJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // Perform necessary assertions on the retrieved training object
                    Assert.IsNotNull(trainingWrapper);
                    Assert.AreEqual(testTraining.Id, trainingWrapper.training.Id);
                    // Add more assertions as necessary to verify the training details
                }
            }
            finally
            {
                // Cleanup: Delete the test trainings
                foreach (var testTraining in _testTrainings)
                {
                    await httpClient.DeleteAsync($"{_cTrainingController}/{testTraining.Id}");
                }
            }
        }


        /// <summary>
        /// Queries the trainings based on specified criteria and asserts the results.
        /// </summary>
        [TestMethod]
        public async Task QueryTrainingsTest()
        {
            var httpClient = Helpers.GetHttpClient();

            await InsertTestTrainings();

            var query = new Query
            {
                Fields = new List<string> { "TrainingName" },
                Filter = new Filter
                {
                    IsOrOperator = false,
                    Fields = new List<FieldExpression>
                {
                    new FieldExpression
                    {
                        FieldName = "TrainingName",
                        Operator = QueryOperator.Contains,
                        Argument = new List<object> { "Open AI" } // The name we are querying for
                    }
                }
                },
                RequestCount = true,
                Top = 200,
                Skip = 0,
            };

            var jsonQuery = JsonSerializer.Serialize(query);
            var queryContent = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

           // Execute the query against Api
           var queryResponse = await httpClient.PostAsync($"{_cTrainingController}", queryContent);
            Assert.IsTrue(queryResponse.IsSuccessStatusCode);

            //Deserialize the response
           var responseJson = await queryResponse.Content.ReadAsStringAsync();
            var trainingsResponse = JsonSerializer.Deserialize<QueryTrainingsResponse>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            //Perform assertions
            Assert.IsNotNull(trainingsResponse);
            Assert.IsTrue(trainingsResponse.Trainings.Any(t => t.TrainingName == "Open AI"));

            var trainingIds = trainingsResponse.Trainings.Select(t => t.Id).ToList();

            // Cleanup: Delete the inserted trainings
            foreach (var id in trainingIds)
            {
                var deleteResponse = await httpClient.DeleteAsync($"{_cTrainingController}/{id}");
                Assert.IsTrue(deleteResponse.IsSuccessStatusCode, $"Cleanup failed for training ID {id}");
            }

        }


        /// <summary>
        /// Creates or updates training instances based on the provided test data and verifies the responses.
        /// </summary>
        [TestMethod]
        public async Task CreateOrUpdateTrainingTest()
        {
            var httpClient = Helpers.GetHttpClient();

            // Insert test trainings first to ensure they are in the system.
            await InsertTestTrainings();

            // Assuming we're updating the first training from the inserted test trainings for simplicity.
            var trainingToUpdate = _testTrainings.First();

            // Update details of the training for demonstration purposes.
            trainingToUpdate.TrainingName = "Updated Training Name";
            trainingToUpdate.Description = "Updated Description";

            // Wrap the training object for the API request.
            var updateRequestObj = new
            {
                Training = trainingToUpdate,
                Filter = new { } 
            };

            // Serializing the request object to JSON for update.
            var updateRequestJson = JsonSerializer.Serialize(updateRequestObj);
            HttpContent updateContent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");

            // Use PUT to update the training, assuming the endpoint differentiates create vs update based on the request method.
            var updateResponse = await httpClient.PutAsync($"{_cTrainingController}", updateContent);
            Assert.IsTrue(updateResponse.IsSuccessStatusCode, "Update of the training failed.");

            // Logging response for debugging.
            var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Update Response Content: {updateResponseContent}");

            // Deserialize the response to check the update was successful, if applicable.
            // Example: var updatedTraining = JsonSerializer.Deserialize<Training>(updateResponseContent);

            // Optionally, confirm the training was updated by querying or directly fetching it, and perform assertions as necessary.

            // Cleanup: Delete the inserted/updated training at the end of the test.
            foreach (var training in _testTrainings)
            {
                await httpClient.DeleteAsync($"{_cTrainingController}/{training.Id}");
            }
        }

        private class TrainingWrapper
        {
            public Training training { get; set; }
        }
    }

   
}
