// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Apollo.Common.Entities;
using Apollo.RestService.IntergrationTests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;


namespace TrainingControllerIntegrationTests
{
    [TestClass]
    public class TrainingControllerIntegrationTests 
    {
        private readonly HttpClient _httpClient;
        private readonly IMongoDatabase _database;

        private const string _cTrainingController = "Training";


        Training[] _testTrainings = new Training[]
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
                                TimeModel = TrainingTimeModel.Unkonwn,
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
                                TimeModel = TrainingTimeModel.Unkonwn,
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


        public TrainingControllerIntegrationTests()
        {
        
        }

        [TestCleanup]
        public async Task CleanUp()
        {
            var httpClient = Helpers.GetHttpClient();

            foreach (var testTraining in _testTrainings)
            {
                await httpClient.DeleteAsync($"{_cTrainingController}/{testTraining.Id}");
            }            
        }

        [TestInitialize]
        private async Task InsertTestTrainings()
        {
            await CleanUp();

            var httpClient = Helpers.GetHttpClient();

            var json = JsonSerializer.Serialize(_testTrainings);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await httpClient.PostAsync($"{_cTrainingController}/insert", content);

            Assert.IsTrue(res.IsSuccessStatusCode);

            var resjson = await res.Content.ReadAsStringAsync();            
        }


        /// <summary>
        /// Makes sure that the GetTraining endpoint returns a correct training instance.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetTrainingTest()
        {
            var httpClient = Helpers.GetHttpClient();

            foreach (var testTraining in _testTrainings)
            {
                var res = await httpClient.GetAsync($"{_cTrainingController}/{testTraining.Id}");

                Assert.IsTrue(res.IsSuccessStatusCode);
            }

            // Assert content as needed
        }
    }
}
