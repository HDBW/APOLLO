// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Apollo.Common.Entities;
using Apollo.Service.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace Apollo.RestService.IntergrationTests
{
    /// <summary>
    /// Integration tests for the UserController class.
    /// </summary>
    internal class ProfileControllerIntergrationTests
    {

        private readonly HttpClient _httpClient;
        private readonly IMongoDatabase _database;

        private const string _cUserController = "User";

        User[] _testUsers = new User[]
             {
            new User
            {
                ObjectId = "obj-12345",
                IdentityProvicer = "AADB2C",
                Upn = "user1@domain.com",
                Email = "user1@domain.com",
                Name = "Test User One",
                ContactInfos = new List<Contact>
                {
                    new Contact
                    {
                        Firstname = "Contact",
                        Surname = "One",
                        Mail = "contact1@domain.com",
                        Phone = "123-456-7890",
                        Organization = "Company One",
                        Address = "123 First St",
                        City = "CityOne",
                        ZipCode = "12345",
                        Country = "CountryOne",
                        EAppointmentUrl = new Uri("http://appointments.companyone.com"),
                        ContactType = new ContactType
                        {
                               ListItemId = 1,
                               Value="TestValue"   
                        }
                    }
                },
                Birthdate = new DateTime(1990, 1, 1),
                Disabilities = false
            },
            new User
            {
                ObjectId = "obj-67890",
                IdentityProvicer = "AADB2C",
                Upn = "user2@domain.com",
                Email = "user2@domain.com",
                Name = "Test User Two",
                ContactInfos = new List<Contact>
                {
                    new Contact
                    {
                        Firstname = "Contact",
                        Surname = "Two",
                        Mail = "contact2@domain.com",
                        Phone = "987-654-3210",
                        Organization = "Company Two",
                        Address = "456 Second St",
                        City = "CityTwo",
                        ZipCode = "67890",
                        Country = "CountryTwo",
                        EAppointmentUrl = new Uri("http://appointments.companytwo.com"),
                        ContactType = new ContactType
                        {
                            ListItemId = 1,
                            Value="TestValue"
                        }
                    }
                },
                Birthdate = new DateTime(1992, 2, 2),
                Disabilities = true
            }
            // ... Other users if necessary
        };


        /// <summary>
        /// Constructor for UserControllerIntergrationTests.
        /// Initializes the HTTP client for testing.
        /// </summary>
        public ProfileControllerIntergrationTests()
        {
            
        }


        [TestInitialize]
        public async Task InitTest()
        {
            await CleanUp();
            await InsertTestUsers();
        }


        [TestCleanup]
        public async Task CleanUp()
        {
            var httpClient = Helpers.GetHttpClient();

            // Loop through each test user and send a DELETE request to the user controller's endpoint.
            foreach (var testUser in _testUsers)
            {
                await httpClient.DeleteAsync($"{_cUserController}/{testUser.Id}");
            }
        }


        /// <summary>
        /// Inserts test users into the database for testing.
        /// </summary>
        [TestMethod]
        public async Task InsertTestUsers()
        {
            await CleanUp(); // Ensure any previous data is cleaned up

            var json = JsonSerializer.Serialize(_testUsers);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/User/insert", content);

            Assert.IsTrue(response.IsSuccessStatusCode);

            var responseJson = await response.Content.ReadAsStringAsync();
            var insertedIds = JsonSerializer.Deserialize<List<string>>(responseJson);
            Assert.IsNotNull(insertedIds, "The response should include IDs of inserted users.");
            Assert.AreEqual(_testUsers.Length, insertedIds.Count, "The number of inserted users should match the input.");

            // Should i leave it ?
            // Optionally, retrieve and assert some details about the inserted users...
            foreach (var id in insertedIds)
            {
                var getUserResponse = await _httpClient.GetAsync($"api/User/{id}");
                Assert.IsTrue(getUserResponse.IsSuccessStatusCode);

                var userDataJson = await getUserResponse.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<User>(userDataJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Assert.IsNotNull(user);
                // Additional assertions as needed...
            }

            // Clean up after test
            await CleanUp();
        }


        /// <summary>
        /// Retrieves and asserts details of a user.
        /// </summary>
        [TestMethod]
        public async Task GetUserTest()
        {
            foreach (var testUser in _testUsers)
            {

                var response = await _httpClient.GetAsync($"{_cUserController}/{testUser.Id}");
                Assert.IsTrue(response.IsSuccessStatusCode);

                var userJson = await response.Content.ReadAsStringAsync();
                var retrievedUser = JsonSerializer.Deserialize<User>(userJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Assert.IsNotNull(retrievedUser);
                Assert.AreEqual(testUser.Name, retrievedUser.Name); // Validate the retrieved user data
                // Add more assertions as necessary
            }
        }


        /// <summary>
        /// Queries users based on specific criteria and performs assertions.
        /// </summary>
        [TestMethod]
        public async Task QueryUsersTest()
        {
         
            var query = new Query
            {
                Fields = new List<string> { "Name", "Email" },
                Filter = new Filter
                {
                    IsOrOperator = false,
                    Fields = new List<FieldExpression>
            {
                new FieldExpression
                {
                    FieldName = "Name",
                    Operator = QueryOperator.Contains,
                    Argument = new List<object> { "John Doe" }
                },
                new FieldExpression
                {
                    FieldName = "Email",
                    Operator = QueryOperator.Contains,
                    Argument = new List<object> { "johndoe@example.com" }
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
            var queryResponse = await _httpClient.PostAsync($"{_cUserController}", queryContent);
            Assert.IsTrue(queryResponse.IsSuccessStatusCode);

            // Deserialize the response
            var responseJson = await queryResponse.Content.ReadAsStringAsync();
            var usersResponse = JsonSerializer.Deserialize<QueryUsersResponse>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Perform assertions 
            Assert.IsNotNull(usersResponse);
      
        }


        /// <summary>
        /// Creates or updates a user and asserts the response.
        /// </summary>
        [TestMethod]
        public async Task CreateOrUpdateUserTest()
        {
            foreach (var testUser in _testUsers)
            {
                // Serialize the individual user object to JSON
                var userJson = JsonSerializer.Serialize(testUser);
                HttpContent content = new StringContent(userJson, Encoding.UTF8, "application/json");

                // Send the create or update request
                HttpResponseMessage response;

                // Check if the user already has an ID to determine if it should be an update or insert
                if (string.IsNullOrEmpty(testUser.Id))
                {
                    // No ID means it's a new user, so use the POST endpoint
                    response = await _httpClient.PostAsync($"{_cUserController}", content);
                }
                else
                {
                    // An ID is present, use the PUT endpoint to update
                    response = await _httpClient.PutAsync($"{_cUserController}", content);
                }

                // Assert that the response is successful
                Assert.IsTrue(response.IsSuccessStatusCode, "The response should be successful.");

                // Deserialize the response content to get the result of the create or update operation
                var responseContent = await response.Content.ReadAsStringAsync();
                var createOrUpdateUserResponse = JsonSerializer.Deserialize<Messages.CreateOrUpdateUserResponse>(responseContent);
                Assert.IsNotNull(createOrUpdateUserResponse, "The response should not be null.");

                // Additional assertions to check the response content can be added here
            }
        }

    }
}
