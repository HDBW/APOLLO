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



        public static Training[] _testTrainings = new Training[]
{
    new Training()
    {
        Id = "T07",  
        ProviderId = "hdbw-F626FEDE-1A30-4DE0-B17B-9DCB04A654C2",
        ExternalTrainingId = "EXT01",
        TrainingType = "Online",
        TrainingName = "Training07",
        Description = "Description of Training 07",
        ShortDescription = "Short Description of T07",
        Content = new List<string> { "Content 1", "Content 2" },
        BenefitList = new List<string> { "Benefit 1", "Benefit 2" },
        Certificate = new List<string> { "Certificate 1", "Certificate 2" },
        Prerequisites = new List<string> { "Prerequisite 1", "Prerequisite 2" },
        Loans = new List<Loans>
        {
            new Loans
            {
                Id = "Loan1",
                Name = "Loan Name 1",
                Description = "Description of Loan 1",
                Url = new Uri("http://loan1.example.com"),
                LoanContact = new Contact
                {
                    Id = "LC01",
                    Firstname = "John",
                    Surname = "Doe",
                    Mail = "john.doe@example.com",
                    Phone = "1234567890",
                    Organization = "Loan Org 1",
                    Address = "123 Loan Street",
                    City = "Loan City",
                    ZipCode = "12345",
                    EAppointmentUrl = new Uri("http://loancontact1.example.com")
                }
            }
        },
        TrainingProvider = new EduProvider
        {
            Id = "TP01",
            Name = "Training Provider 1",
            Description = "Description of Training Provider 1",
            Url = new Uri("http://trainingprovider1.com"),
            Contact = new Contact
            {
                Id = "TPC01",
                Firstname = "Alice",
                Surname = "Smith",
                Mail = "alice.smith@example.com",
                Phone = "0987654321",
                Organization = "Training Provider Org 1",
                Address = "123 Training Provider Street",
                City = "Provider City",
                ZipCode = "67890",
                EAppointmentUrl = new Uri("http://trainingprovidercontact1.example.com")
            },
            Image = new Uri("http://trainingprovider1.com/image.jpg")
        },
        CourseProvider = new EduProvider
        {
            Id = "CP01",
            Name = "Course Provider 1",
            Description = "Description of Course Provider 1",
            Url = new Uri("http://courseprovider1.com"),
            Contact = new Contact
            {
                Id = "CPC01",
                Firstname = "Bob",
                Surname = "Brown",
                Mail = "bob.brown@example.com",
                Phone = "1234567890",
                Organization = "Course Provider Org 1",
                Address = "123 Course Provider Street",
                City = "Provider City",
                ZipCode = "67890",
                EAppointmentUrl = new Uri("http://courseprovidercontact1.example.com")
            },
            Image = new Uri("http://courseprovider1.com/image.jpg")
        },
        Appointment = new List<Appointment>
        {
            new Appointment
            {
                Id = "A01",
                AppointmentType = "In-person",
                AppointmentDescription = "Introduction session",
                StartDate = DateTime.Parse("2024-01-17T10:30:00.000Z"),
                EndDate = DateTime.Parse("2024-01-24T10:30:00.000Z"),
                DurationDescription = "2 hours",
                Occurences = new List<Occurence>
                {
                    new Occurence
                    {
                        Id = "O01",
                        StartDate = DateTime.Parse("2024-01-17T10:30:00.000Z"),
                        EndDate = DateTime.Parse("2024-01-17T12:30:00.000Z"),
                        Description = "First occurrence"
                    }
                },
                IsGuaranteed = true,
                TrainingMode = TrainingMode.Offline
            }
        },
        ProductUrl = new Uri("http://product.example.com"),
        Contacts = new List<Contact>
        {
            new Contact
            {
                Id = "CT01",
                Firstname = "Charlie",
                Surname = "Day",
                Mail = "charlie.day@example.com",
                Phone = "1234567890",
                Organization = "Contact Org",
                Address = "123 Contact Street",
                City = "Contact City",
                ZipCode = "12345",
                EAppointmentUrl = new Uri("http://contact.example.com")
            }
        },
        Tags = new List<string> { "Tag1", "Tag2" },
        Categories = new List<string> { "Category1", "Category2" },
        PublishingDate = DateTime.Parse("2024-03-04T09:00:00Z"),
        UnpublishingDate = DateTime.Parse("2024-03-11T09:00:00Z"),
        Successor = "Next Training",
        Predecessor = "Previous Training"
    }
};

        //        Training[] _testTrainings = new Training[]
        //{
        //    new Training()
        //    {
        //        Id = "IT01",
        //        ProviderId = "integrationtest",
        //        ExternalTrainingId = "EXT01",
        //        TrainingType = "Online",
        //        TrainingName = "Open AI",
        //        Description = "Introduction to Open AI",
        //        ShortDescription = "Open AI course",
        //        Image = new Uri("http://example.com/image.jpg"),
        //        SubTitle = "A brief introduction",
        //        Content = new List<string>() { "Module 1", "Module 2" },
        //        BenefitList = new List<string>() { "Understanding AI", "Practical applications" },
        //        Certificate = new List<string>() { "Certificate of Completion" },
        //        Prerequisites = new List<string>() { "Basic programming knowledge" },
        //        Loans = new List<Loans>()
        //        {
        //            new Loans()
        //            {
        //                Id = "L01",
        //                Name = "Loan 1",
        //                Description = "Loan 1 description"
        //            },
        //            new Loans()
        //            {
        //                Id = "L02",
        //                Name = "Loan 2",
        //                Description = "Loan 2 description"
        //            }
        //        },
        //        Appointment = new List<Appointment>()
        //        {
        //            new Appointment()
        //            {
        //                Id = "A01",
        //                AppointmentType = "In-person",
        //                AppointmentDescription = "Introduction session",
        //                StartDate = DateTime.Parse("2024-01-17T10:30:00.000Z"),
        //                EndDate = DateTime.Parse("2024-01-24T10:30:00.000Z"),
        //                DurationDescription = "2 hours",
        //                Occurences = new List<Occurence>()
        //                {
        //                    new Occurence()
        //                    {
        //                        Id = "O01",
        //                        StartDate = DateTime.Parse("2024-01-17T10:30:00.000Z"),
        //                        EndDate = DateTime.Parse("2024-01-17T12:30:00.000Z"),
        //                        Description = "First occurrence"
        //                    }
        //                },
        //                IsGuaranteed = true,
        //                TrainingMode = TrainingMode.Offline
        //            }
        //        },
        //        // Fill in other required properties as needed
        //    }
        //};


        /// <summary>
        /// The instance of the training with all properties.
        /// </summary>
        private string _complexTrainingJson = @"[
          {
            ""id"": ""ITCX01"",
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
        /// </summary>
        public async Task InsertTestTrainings()
        {
            var httpClient = Helpers.GetHttpClient();

            var json = JsonSerializer.Serialize(_testTrainings);
            Console.WriteLine($"Sending InsertTestTrainings request with JSON: {json}");

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage res = await httpClient.PostAsync($"{_cTrainingController}/insert", content);

            Console.WriteLine($"InsertTestTrainings Response StatusCode: {res.StatusCode}");
            var resjson = await res.Content.ReadAsStringAsync();
            Console.WriteLine($"InsertTestTrainings Response Body: {resjson}");

            Assert.IsTrue(res.IsSuccessStatusCode, $"Insertion of test trainings failed. StatusCode: {res.StatusCode}, Response: {resjson}");

            var insertedIds = JsonSerializer.Deserialize<List<string>>(resjson);
            Console.WriteLine($"Inserted IDs: {string.Join(", ", insertedIds)}");
            Assert.IsNotNull(insertedIds, "The response should include IDs of inserted trainings.");
            Assert.AreEqual(_testTrainings.Length, insertedIds.Count, "The number of inserted trainings should match the input.");
        }



        /// <summary>
        /// Inserts a list of complex training objects into the system.
        /// Verifies the successful insertion by checking the response contains the IDs of the inserted trainings.
        /// </summary>
        /// <returns>A list of IDs for the inserted training objects.</returns>
        [TestMethod]
        public async Task<List<string>> InsertComplexTrainingTest()
        {
            var httpClient = Helpers.GetHttpClient();
            HttpContent content = new StringContent(_complexTrainingJson, Encoding.UTF8, "application/json");
            var res = await httpClient.PostAsync($"{_cTrainingController}/insert", content);
            Assert.IsTrue(res.IsSuccessStatusCode);

            var resJson = await res.Content.ReadAsStringAsync();
            var insertedIds = JsonSerializer.Deserialize<List<string>>(resJson);
            Assert.IsNotNull(insertedIds, "The response should include IDs of inserted trainings.");

            return insertedIds; 
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
        /// Tests the retrieval of complex training objects by their IDs.
        /// Verifies each training can be fetched successfully and matches the expected data.
        /// </summary>
        [TestMethod]
        public async Task GetComplexTrainingTest()
        {
            var complexTrainingIds = await InsertComplexTrainingTest(); 
            var httpClient = Helpers.GetHttpClient();

            foreach (var complexTrainingId in complexTrainingIds)
            {
                var response = await httpClient.GetAsync($"{_cTrainingController}/{complexTrainingId}");
                Assert.IsTrue(response.IsSuccessStatusCode);

                var trainingJson = await response.Content.ReadAsStringAsync();
                var trainingWrapper = JsonSerializer.Deserialize<TrainingWrapper>(trainingJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Assert.IsNotNull(trainingWrapper);
                Assert.AreEqual(complexTrainingId, trainingWrapper.training.Id);
            }

            // Cleanup
            foreach (var complexTrainingId in complexTrainingIds)
            {
                await httpClient.DeleteAsync($"{_cTrainingController}/{complexTrainingId}");
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
                Fields = new List<string> { "ShortDescription" },
                Filter = new Filter
                {
                    IsOrOperator = false,
                    Fields = new List<FieldExpression>
                {
                    new FieldExpression
                    {
                        FieldName = "ShortDescription",
                        Operator = QueryOperator.Contains,
                        Argument = new List<object> { "Short Description of T07" }
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
            Assert.IsTrue(trainingsResponse.Trainings.Any(t => t.ShortDescription == "Short Description of T07"));

            var trainingIds = trainingsResponse.Trainings.Select(t => t.Id).ToList();

            // Cleanup: Delete the inserted trainings
            foreach (var id in trainingIds)
            {
                var deleteResponse = await httpClient.DeleteAsync($"{_cTrainingController}/{id}");
                Assert.IsTrue(deleteResponse.IsSuccessStatusCode, $"Cleanup failed for training ID {id}");
            }

        }


        /// <summary>
        /// Queries the database for complex training objects that match specific criteria.
        /// First, complex trainings are inserted to ensure data is present for querying.
        /// Then, a query is executed to find trainings with a specific name.
        /// Verifies that the query response includes the expected trainings.
        /// </summary>
        [TestMethod]
        public async Task QueryComplexTrainingsTest()
        {
            var httpClient = Helpers.GetHttpClient();

            // First, insert complex trainings into the database
            await InsertComplexTrainingTest();

            // Define a query to find trainings with a specific name
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
                    Argument = new List<object> { "Training T05" } // The name we are querying for
                }
            }
                },
                RequestCount = true,
                Top = 200,
                Skip = 0,
            };

            var jsonQuery = JsonSerializer.Serialize(query);
            var queryContent = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            // Execute the query against the API
            var queryResponse = await httpClient.PostAsync($"{_cTrainingController}", queryContent);
            Assert.IsTrue(queryResponse.IsSuccessStatusCode);

            // Deserialize the response
            var responseJson = await queryResponse.Content.ReadAsStringAsync();
            var trainingsResponse = JsonSerializer.Deserialize<QueryTrainingsResponse>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Perform assertions
            Assert.IsNotNull(trainingsResponse);
            Assert.IsTrue(trainingsResponse.Trainings.Any(t => t.TrainingName.Contains("Training T05")));

            // Extract the IDs of trainings matched by the query for cleanup
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

            // Assuming we're updating the first training .
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

            // Use PUT to update the training
            var updateResponse = await httpClient.PutAsync($"{_cTrainingController}", updateContent);
            Assert.IsTrue(updateResponse.IsSuccessStatusCode, "Update of the training failed.");

            // Logging response for debugging.
            var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Update Response Content: {updateResponseContent}");

            // Cleanup: Delete the inserted/updated training at the end of the test.
            foreach (var training in _testTrainings)
            {
                await httpClient.DeleteAsync($"{_cTrainingController}/{training.Id}");
            }
        }


        /// <summary>
        /// Demonstrates creating or updating a complex training object.
        /// A complex training is first deserialized from JSON, then updated, and finally,
        /// the updated training is submitted via an API request.
        /// Verifies the update operation was successful.
        /// </summary>
        [TestMethod]
        public async Task CreateOrUpdateComplexTrainingTest()
        {
            var httpClient = Helpers.GetHttpClient();

            // Step 1: Insert initial complex training data
            var insertedTrainingIds = await InsertComplexTrainingTest();

            // Step 2: Prepare the training update
            var trainingToUpdate = new Training
            {
                Id = "ITCX01",
                ProviderId = "intergrationtest",
                TrainingName = "Updated Training Name",
                Description = "Updated Description",
                TrainingType = "Online", 
                ShortDescription = "A concise description of the updated training"
            };

            // Step 3: Serialize and send the update request
            var updateRequestJson = JsonSerializer.Serialize(new { Training = trainingToUpdate });
            HttpContent updateContent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");
            var updateResponse = await httpClient.PutAsync($"{_cTrainingController}", updateContent);

            // Step 4: Assert the update was successful
            Assert.IsTrue(updateResponse.IsSuccessStatusCode, "Failed to update the training.");


            // Step 5: Cleanup - Delete the inserted/updated trainings at the end of the test
            // This includes both the initially inserted trainings and the one updated by this test
            foreach (var id in insertedTrainingIds)
            {
                var deleteResponse = await httpClient.DeleteAsync($"{_cTrainingController}/{id}");
                Assert.IsTrue(deleteResponse.IsSuccessStatusCode, $"Cleanup failed for training ID {id}");
            }
        }


        private class TrainingWrapper
        {
            public Training training { get; set; }
        }
    }

   
}
