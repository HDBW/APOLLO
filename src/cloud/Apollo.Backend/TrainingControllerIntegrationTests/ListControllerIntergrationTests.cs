// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Apollo.Common.Entities;
using Apollo.RestService.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Apollo.RestService.IntergrationTests
{
    [TestClass]
    public class ListControllerIntergrationTests
    {
        private const string _cListController = "List";
        private const string _cTestListType1 = "TestListType1";
        private const string _cTestListType2 = "TestListType2";
        private const string _cTestListType3 = "TestListType3";
        private const string _cTestListType4 = "TestListType4";



        private List<ApolloList> _testLists = new List<ApolloList>()
        {
             new ApolloList()
             {
                 Id = "UT01",
                 ItemType = _cTestListType1,
                 Items = new List<ApolloListItem>()
                 {
                     new ApolloListItem()
                     {
                         ListItemId = 1,
                         Lng = "DE",
                         Value = "C# Entwickler"
                     },
                     new ApolloListItem()
                     {
                         ListItemId = 2,
                         Lng = "EN",
                         Value = "C# Developer"
                     }
                 }
             },
             new ApolloList()
             {
                 Id = "UT02",
                 ItemType = _cTestListType2,
                 Items = new List<ApolloListItem>()
                 {
                     new ApolloListItem()
                     {
                         ListItemId = 1,
                         Lng = "DE",
                         Value = "Python Entwickler"
                     },
                     new ApolloListItem()
                     {
                         ListItemId = 2,
                         Lng = "EN",
                         Value = "Python Developer"
                     }
                 }
             },
             new ApolloList()
             {
                 Id = "UT03",
                 ItemType = _cTestListType3,
                 Items = new List<ApolloListItem>()
                 {
                     new ApolloListItem()
                     {
                         ListItemId = 1,
                         Lng = "DE",
                         Value = "C++ Entwickler"
                     },
                     new ApolloListItem()
                     {
                         ListItemId= 2,
                         Lng = "EN",
                         Value = "C++ Developer"
                     }
                 }
             },
              new ApolloList()
             {
                 Id = "UT04",
                 ItemType = _cTestListType4,
                 Items = new List<ApolloListItem>()
                 {
                     new ApolloListItem()
                     {   ListItemId = 1,
                         Lng = "DE",
                         Value = "JAVA Entwickler"
                     },
                       new ApolloListItem()
                     {   ListItemId = 1,
                         Lng = "DE",
                         Value = "C# Entwickler"
                     },
                         new ApolloListItem()
                     {   ListItemId = 1,
                         Lng = "DE",
                         Value = "Python Hobbydeveloper"
                     },
                           new ApolloListItem()
                     {   ListItemId = 1,
                         Lng = "DE",
                         Value = "JAVA Script angelernter Programmierer"
                     },
                             new ApolloListItem()
                     {   ListItemId = 1,
                         Lng = "DE",
                         Value = "Indischer alleskönner Entwickler"
                     },
                     new ApolloListItem()
                     {   ListItemId= 2,
                         Lng = "EN",
                         Value = "C++ Developer"
                     }
                 }
             },
        };


        public ListControllerIntergrationTests()
        {
            
        }


        /// <summary>
        /// Ensures a clean state before each test by invoking the CleanUp method to delete existing test lists.
        /// </summary>
        [TestInitialize]
        public async Task Initialize()
        {
            await CleanUp(); 
        }


        /// <summary>
        /// Cleans up by deleting test lists created during tests to ensure a clean state.
        /// </summary>
        [TestCleanup]
        public async Task CleanUp()
        {
            var httpClient = Helpers.GetHttpClient();

            // Loop through each test list and send a DELETE request to the list controller's endpoint.
            foreach (var testList in _testLists)
            {
               await httpClient.DeleteAsync($"{_cListController}/{testList.Id}");
            }
        }


        /// <summary>
        /// Inserts test lists into the system and verifies that the operation was successful.
        /// </summary>
        [TestMethod]
        public async Task InsertTestLists()
        {
            await CleanUp(); 

            var httpClient = Helpers.GetHttpClient();

          
            var json = JsonSerializer.Serialize(_testLists);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            
            var response = await httpClient.PostAsync($"{_cListController}/insert", content);

            // Assert that the HTTP response is successful
            Assert.IsTrue(response.IsSuccessStatusCode, "Inserting test lists should return a successful response.");

            // Read the response content and deserialize it to get the IDs of the inserted lists
            var responseJson = await response.Content.ReadAsStringAsync();
            var insertedIds = JsonSerializer.Deserialize<List<string>>(responseJson);

            // Assert that the response contains IDs and matches the expected number of inserted lists
            Assert.IsNotNull(insertedIds, "The response should include IDs of inserted lists.");
            Assert.AreEqual(_testLists.Count, insertedIds.Count, "The number of inserted lists should match the input.");
        }


        /// <summary>
        /// Fetches all lists from the system and verifies that the returned lists match the expected criteria.
        /// </summary>
        [TestMethod]
        public async Task GetListsTest()
        {
            var httpClient = Helpers.GetHttpClient();

            // GET request to fetch all lists
            var response = await httpClient.GetAsync($"api/{_cListController}"); 
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Deserialize the response content to a List of ApolloList objects
            var listsJson = await response.Content.ReadAsStringAsync();
            var lists = JsonSerializer.Deserialize<List<ApolloList>>(listsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Perform necessary assertions on the retrieved list objects
            Assert.IsNotNull(lists);
            Assert.IsTrue(lists.Any()); // Asserting that we have received at least one list
        }


        /// <summary>
        /// Queries list items based on specific criteria and verifies the query results.
        /// </summary>
        [TestMethod]
        public async Task QueryListItemsTest()
        {
            var httpClient = Helpers.GetHttpClient();

            var query = new
            {
                ItemType = "EducationType"
            };

            var jsonQuery = JsonSerializer.Serialize(query);
            var queryContent = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            // Using the correct constant for the ListController
            var queryResponse = await httpClient.PostAsync($"api/{_cListController}", queryContent); 
            Assert.IsTrue(queryResponse.IsSuccessStatusCode, "QueryListItems should return a successful response.");

            // Deserialize the response to the expected type
            var responseJson = await queryResponse.Content.ReadAsStringAsync();
            var listResponse = JsonSerializer.Deserialize<QueryListResponse>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Perform assertions
            Assert.IsNotNull(listResponse, "The response content should not be null.");
            Assert.IsTrue(listResponse.Result.Any(), "The response should contain at least one list item.");
        }


        /// <summary>
        /// Creates or updates lists in the system and verifies the operation was successful.
        /// </summary>
        [TestMethod]
        public async Task CreateOrUpdateListTest()
        {
            var httpClient = Helpers.GetHttpClient();
            var testLists = _testLists; 

            foreach (var testList in testLists)
            {
                // Serialize the individual list object to JSON
                var listJson = JsonSerializer.Serialize(testList);
                HttpContent content = new StringContent(listJson, Encoding.UTF8, "application/json");

                // Send the create or update request
                HttpResponseMessage response;

                // Check if the list already has an ID to determine if it should be an update or insert
                if (string.IsNullOrEmpty(testList.Id))
                {
                    // No ID means it's a new list, so use the POST endpoint
                    response = await httpClient.PostAsync($"api/{_cListController}", content);
                }
                else
                {
                    // An ID is present, indicating an existing list, so use the PUT endpoint to update
                    response = await httpClient.PutAsync($"api/{_cListController}/{testList.Id}", content);
                }

                // Assert that the response is successful
                Assert.IsTrue(response.IsSuccessStatusCode, "The response should be successful.");

                // Deserialize the response content to get the ID of the created or updated list
                var responseContent = await response.Content.ReadAsStringAsync();
                var createdOrUpdatedId = JsonSerializer.Deserialize<string>(responseContent);
                Assert.IsNotNull(createdOrUpdatedId, "The response should contain the ID of the created or updated list.");
            }
        }
    }
}
