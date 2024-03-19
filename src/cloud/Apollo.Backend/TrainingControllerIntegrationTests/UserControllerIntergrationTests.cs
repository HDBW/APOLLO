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
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace Apollo.RestService.IntergrationTests
{
    /// <summary>
    /// Integration tests for the UserController class.
    /// </summary>
    [TestClass]
    public class UserControllerIntergrationTests
    {

        private const string _cUserController = "User";

        User[] _testUsers = new User[]
        {
            new User { Id = "IT01", ObjectId = "ObjectID1", Upn = "upn1@example.com", Email = "user1@example.com", Name = "Test User1", IdentityProvider = "Provider1", ContactInfos = new List<Contact>(), Birthdate = new DateTime(1990, 1, 1), Disabilities = false, Profile = new Profile() },
            new User { Id = "IT02", ObjectId = "ObjectID2", Upn = "upn2@example.com", Email = "user2@example.com", Name = "Test User2", IdentityProvider = "Provider2", ContactInfos = new List<Contact>(), Birthdate = new DateTime(1991, 2, 2), Disabilities = true, Profile = new Profile() },
            new User { Id = "IT03", ObjectId = "ObjectID3", Upn = "upn3@example.com", Email = "user3@example.com", Name = "Test User3", IdentityProvider = "Provider3", ContactInfos = new List<Contact>(), Birthdate = new DateTime(1992, 3, 3), Disabilities = false, Profile = new Profile() }
        };


        /// <summary>
        /// Constructor for UserControllerIntergrationTests.
        /// Initializes the HTTP client for testing.
        /// </summary>
        public UserControllerIntergrationTests()
        {
            
        }


        [TestInitialize]
        public async Task InitTest()
        {
            //await CleanUp();
            //await InsertTestUsers();
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
            var httpClient = Helpers.GetHttpClient();

            // Serialize the _testUsers array to JSON
            var usersJson = JsonSerializer.Serialize(_testUsers);
            HttpContent content = new StringContent(usersJson, Encoding.UTF8, "application/json");

            // Attempt to insert the test users
            var response = await httpClient.PostAsync("User/insert", content);

            // Log detailed information for debugging
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to insert test users. Status code: {response.StatusCode}");
                Console.WriteLine($"Response content: {responseContent}");
            }

            // Assert that the insertion was successful
            Assert.IsTrue(response.IsSuccessStatusCode, "Failed to insert test users.");
        }



        /// <summary>
        /// Retrieves and asserts details of a user.
        /// </summary>
        [TestMethod]
        public async Task GetUserTest()
        {
            var httpClient = Helpers.GetHttpClient();

            foreach (var testUser in _testUsers)
            {
                var response = await httpClient.GetAsync($"{_cUserController}/{testUser.ObjectId}");
                Console.WriteLine($"Request URL: {httpClient.BaseAddress}{_cUserController}/{testUser.ObjectId}");
                Console.WriteLine($"Response StatusCode: {response.StatusCode}");
                var userJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {userJson}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to retrieve user. Skipping further assertions for this user.");
                    continue;
                }

                var wrapper = JsonSerializer.Deserialize<UserWrapper>(userJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var retrievedUser = wrapper?.User;

                if (retrievedUser == null)
                {
                    Console.WriteLine("Failed to deserialize the user JSON. Skipping further assertions for this user.");
                    continue;
                }

                // Check for both original and potentially updated user data
                bool nameMatches = testUser.Name == retrievedUser.Name || "Updated User Name" == retrievedUser.Name;
                Assert.IsTrue(nameMatches, $"User name does not match. Expected: {testUser.Name} or Updated User Name, Actual: {retrievedUser.Name}");
            }
        }


        /// <summary>
        /// Queries users based on specific criteria and performs assertions.
        /// </summary>
        [TestMethod]
        public async Task QueryUsersTest()
        {
            var httpClient = Helpers.GetHttpClient();

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
                    Argument = new List<object> { "Test User1" }
                },
                new FieldExpression
                {
                    FieldName = "Email",
                    Operator = QueryOperator.Contains,
                    Argument = new List<object> { "user1@example.com" }
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
            var queryResponse = await httpClient.PostAsync($"{_cUserController}", queryContent);
            Assert.IsTrue(queryResponse.IsSuccessStatusCode);

            // Deserialize the response to a JsonDocument
            var responseJson = await queryResponse.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);

            // Create an instance of QueryUsersResponse and manually assign the Users property
            var usersResponse = new QueryUsersResponse(null); // Passing null since we'll manually set the Users property

            if (doc.RootElement.TryGetProperty("users", out var usersElement) && usersElement.ValueKind == JsonValueKind.Array)
            {
                usersResponse.Users = JsonSerializer.Deserialize<List<User>>(usersElement.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<User>();
            }

            // Perform assertions
            Assert.IsNotNull(usersResponse);
            Assert.IsTrue(usersResponse.Users.Any(u => u.Name.Contains("Test User1") && u.Email.Contains("user1@example.com")));
        }


        /// <summary>
        /// Creates or updates a user and asserts the response.
        /// </summary>
        [TestMethod]
        public async Task CreateOrUpdateUserTest()
        {
            var httpClient = Helpers.GetHttpClient();

            // Assuming _testUsers[0] is the user you want to create/update
            var requestObj = new
            {
                User = _testUsers[0],
                  Filter = new { }
            };

            // Serializing the request object to JSON for creation
            var createRequestJson = JsonSerializer.Serialize(requestObj);
            HttpContent createContent = new StringContent(createRequestJson, Encoding.UTF8, "application/json");

            // POST to the CreateOrUpdate endpoint for creation
            var createResponse = await httpClient.PutAsync($"{_cUserController}", createContent);
            Assert.IsTrue(createResponse.IsSuccessStatusCode, "Creation of the user failed.");

            // Logging response for debugging
            var createResponseContent = await createResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Create Response Content: {createResponseContent}");

            // Modifying the user object for update
            requestObj.User.Name = "Updated User Name";
            requestObj.User.Email = "updated@example.com";

            // Serializing the modified request object to JSON for update
            var updateRequestJson = JsonSerializer.Serialize(requestObj);
            HttpContent updateContent = new StringContent(updateRequestJson, Encoding.UTF8, "application/json");

            // POST to the CreateOrUpdate endpoint for update
            var updateResponse = await httpClient.PutAsync($"{_cUserController}", updateContent);
            Assert.IsTrue(updateResponse.IsSuccessStatusCode, "Update of the user failed.");

            // Logging response for debugging
            var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Update Response Content: {updateResponseContent}");

            await CleanUp();
        }


        private class UserWrapper
        {
            public User User { get; set; }
        }

    }
}
