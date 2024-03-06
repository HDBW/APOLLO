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

    /// <summary>
    /// Integration tests for the TrainingController class.
    /// </summary>
    [TestClass]
    public class TrainingControllerIntegrationTests 
    {
        private const string _cTrainingController = "Training";

        Training[] _testTrainings = new Training[]
        {
        new Training()
        {
        Id = "UT01",
        ProviderId = "IntegrationTestProvider",
        TrainingName = "Advanced C# Programming",
        TrainingType = "Online",
        Description = "An advanced course in C# programming covering topics such as async programming, LINQ, and more.",
        ShortDescription = "Advanced C# course",
        Content = new List<string>() { "Async programming", "LINQ", "Memory management" },
        BenefitList = new List<string>() { "Deep understanding of advanced C# features", "Preparation for professional certification" },
        Certificate = new List<string>() { "Advanced C# Programmer" },
        Prerequisites = new List<string>() { "Basic knowledge of C#", "Experience with .NET framework" },
        Price = 299.99,
        PriceDescription = "Includes course materials and certification exam",
        ProductUrl = new Uri("https://example.com/csharp-advanced"),
        PublishingDate = DateTime.Now,
        UnpublishingDate = DateTime.Now.AddYears(1),

        Loans = new List<Loans>(
            new Loans[]
            {
                new Loans { Id = "L01", Name = "Loan 1" },
                new Loans { Id = "L02", Name = "Loan 2" }
            }),

        TrainingProvider = new EduProvider()
        {
            Id = "EduProv01",
            Name = "Tech Education Online",
            Description = "An online platform offering tech-focused education.",
            Url = new Uri("https://tech-education.com")
        },

       Appointment = new List<Appointment>

        {
            new Appointment
        {

        Id = "A01",
        AppointmentUrl = new Uri("http://example.com/appointment1"),
        AppointmentType = "In-person",
        AppointmentDescription = "Introduction session",
        AppointmentLocation = new Contact

        {
            Id = "C01",
            Firstname = "John",
            Surname = "Doe",
            Mail = "john.doe@example.com",
            Phone = "123456789",
            Organization = "Example Org",
            Address = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Region = "Anyregion",
            Country = "Anycountry",
            EAppointmentUrl = new Uri("http://example.com/eappointment"),
        },
            StartDate = DateTime.Parse("2024-01-17T10:30:00.000Z"),
            EndDate = DateTime.Parse("2024-01-24T10:30:00.000Z"),
            DurationDescription = "2 hours",
            Duration = 120, // This might need to be calculated or set based on the duration description
            Occurences = new List<Occurence>
          {
            new Occurence
            {
                Id = "O01",
                CorrelationId = "Corr01",
                StartDate = DateTime.Parse("2024-01-17T10:30:00.000Z"),
                EndDate = DateTime.Parse("2024-01-17T12:30:00.000Z"),
                Description = "First occurrence of the appointment",
                Location = new Contact
                {
                    Id = "Location01",
                    Firstname = "John",
                    Surname = "Doe",
                    Mail = "johndoe@example.com",
                    Phone = "123-456-7890",
                    Organization = "Test Venue",
                    Address = "123 Test Street",
                    City = "Test City",
                    ZipCode = "12345",
                    Region = "Test Region",
                    Country = "Testland",
                    EAppointmentUrl = new Uri("http://example.com/appointment-booking"),
                }
            }
        },

        IsGuaranteed = true,
        TrainingMode = TrainingMode.Offline,
        TimeInvestAttendee = 120,
        TimeModel = TrainingTimeModel.Unknown,
        OccurenceNoteOnTime = "Weekly",
        ExecutionDuration = "2",
        ExecutionDurationUnit = "UE",
        ExecutionDurationDescription = "Weekly",
        LessonType = "Practical",
        BookingUrl = new Uri("http://example.com/booking"),
        DurationUnit = "Hours",
        Comment = "This is a sample appointment for testing."
         }
    },
        Contacts = new List<Contact>()
        {
            new Contact()
            {
                Firstname = "Jane",
                Surname = "Smith",
                Mail = "jane.smith@conferencecenter.com",
                Phone = "+491234567890",
                Organization = "City Conference Center",
                Address = "456 Conference Blvd",
                City = "Metropolis",
                ZipCode = "10101",
                Region = "Metro Region",
                Country = "Germany",
                EAppointmentUrl = new Uri("https://www.conferencecenter.com/bookings"),
            }
        },
        Tags = new List<string>() { "C#", ".NET", "Advanced" },
        Categories = new List<string>() { "Programming", "Software Development" },
        AccessibilityAvailable = true
        }
};



        /// <summary>
        /// The instance of the training with all properties.
        /// </summary>
        private string _complexTrainingJson = @"[
  {
    ""id"": ""IT01"",
    ""providerId"": ""intergrationtest"",
    ""trainingName"": ""Training01"",
    ""description"": ""Description of Training 01"",
    ""shortDescription"": ""Short Description of 01"",
    ""trainingType"": ""Type of Training for Training 01"",
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


        /// <summary>
        /// Cleanup method to delete test data after each test.
        /// </summary>
        [TestCleanup]
        public async Task CleanUp()
        {
            var httpClient = Helpers.GetHttpClient();

            foreach (var testTraining in _testTrainings)
            {
                await httpClient.DeleteAsync($"{_cTrainingController}/{testTraining.Id}");
            }            
        }


        /// <summary>
        /// Initialization method to insert test data before each test.
        /// </summary>
        [TestInitialize]
        public async Task InitTest()
        {
            await CleanUp();
            await InsertTestTrainings();
        }


        /// <summary>
        /// Inserts a predefined list of training objects into the database for testing purposes.
        /// This method serializes the list of training objects to JSON, sends it to the training insert API endpoint,
        /// and verifies the success of the operation by checking the response status code and the content.
        /// Before and after insertion, it ensures the clean-up of test data to maintain test environment integrity.
        /// </summary>
        private async Task InsertTestTrainings()
        {
            await CleanUp();

            var httpClient = Helpers.GetHttpClient();

            var json = JsonSerializer.Serialize(_testTrainings);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await httpClient.PostAsync($"{_cTrainingController}/insert", content);

            Assert.IsTrue(res.IsSuccessStatusCode);

            var resjson = await res.Content.ReadAsStringAsync();
            var insertedIds = JsonSerializer.Deserialize<List<string>>(resjson);
            Assert.IsNotNull(insertedIds, "The response should include IDs of inserted trainings.");
            Assert.AreEqual(_testTrainings.Length, insertedIds.Count, "The number of inserted trainings should match the input.");

            await CleanUp();
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
                // GET the training by ID and verify the response is successful
                var response = await httpClient.GetAsync($"{_cTrainingController}/{testTraining.Id}");
                Assert.IsTrue(response.IsSuccessStatusCode);

                // Deserialize the response content to a Training object
                var trainingJson = await response.Content.ReadAsStringAsync();
                var training = JsonSerializer.Deserialize<Training>(trainingJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Perform necessary assertions on the retrieved training object
                Assert.IsNotNull(training);
                Assert.AreEqual(testTraining.Id, training.Id);
                // Add more assertions as necessary to verify the training details
            }

            // Assert content as needed
        }


        /// <summary>
        /// Queries the trainings based on specified criteria and asserts the results.
        /// </summary>
        [TestMethod]
        public async Task QueryTrainingsTest()
        {
            var httpClient = Helpers.GetHttpClient();

            // Construct the query
            var query = new Apollo.Common.Entities.Query
            {
                Fields = new List<string> { "TrainingName" },
                Filter = new Apollo.Common.Entities.Filter
                {
                    IsOrOperator = false,
                    Fields = new List<Apollo.Common.Entities.FieldExpression>
                {
                    new Apollo.Common.Entities.FieldExpression
                    {
                        FieldName = "TrainingName",
                        Operator = Apollo.Common.Entities.QueryOperator.Equals,
                        Argument = new List<object> { "Business English A2/B1" } // The name we are querying for
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

            // Deserialize the response
            var responseJson = await queryResponse.Content.ReadAsStringAsync();
            var trainingsResponse = JsonSerializer.Deserialize<Apollo.RestService.Messages.QueryTrainingsResponse>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Perform assertions 
            Assert.IsNotNull(trainingsResponse);
            Assert.IsTrue(trainingsResponse.Trainings.Any(t => t.TrainingName == "Business English A2/B1"));
        }


        /// <summary>
        /// Creates or updates training instances based on the provided test data and verifies the responses.
        /// </summary>
        [TestMethod]
        public async Task CreateOrUpdateTrainingTest()
        {
            var httpClient = Helpers.GetHttpClient();

            foreach (var testTraining in _testTrainings)
            {
                // Serialize the individual training object to JSON
                var trainingJson = JsonSerializer.Serialize(testTraining);
                HttpContent content = new StringContent(trainingJson, Encoding.UTF8, "application/json");

                // Send the create or update request
                HttpResponseMessage response;

                // Check if the training already has an ID to determine if it should be an update or insert
                if (string.IsNullOrEmpty(testTraining.Id))
                {
                    // No ID means it's a new training, so use the POST endpoint
                    response = await httpClient.PostAsync($"{_cTrainingController}", content);
                }
                else
                {
                    // An ID is present, use the PUT endpoint to update
                    response = await httpClient.PutAsync($"{_cTrainingController}/{testTraining.Id}", content);
                }

                // Assert that the response is successful
                Assert.IsTrue(response.IsSuccessStatusCode, "The response should be successful.");

                // Deserialize the response content to get the ID of the created or updated training
                var responseContent = await response.Content.ReadAsStringAsync();
                var createdOrUpdatedId = JsonSerializer.Deserialize<string>(responseContent);
                Assert.IsNotNull(createdOrUpdatedId, "The response should contain the ID of the created or updated training.");

                // Additional assertions to check the response content can be added here
            }
        }
    }
}
