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
        /// Fetches all lists from the system and verifies that the returned lists match the expected criteria.
        /// </summary>
        [TestMethod]
        public async Task GetListsTest()
        {
            var httpClient = Helpers.GetHttpClient();

            // GET request to fetch all lists
            var response = await httpClient.GetAsync($"{_cListController}");
            Assert.IsTrue(response.IsSuccessStatusCode, "Failed to retrieve lists.");

            // Deserialize the response content to a ListsResponseWrapper object
            var listsJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Raw JSON response: {listsJson}");

            var wrapper = JsonSerializer.Deserialize<ListsResponseWrapper>(listsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Perform necessary assertions on the retrieved list objects within the Result property
            Assert.IsNotNull(wrapper?.Result, "Failed to deserialize lists.");
            Assert.IsTrue(wrapper.Result.Any(), "No lists were retrieved.");
        }


        public class ListsResponseWrapper
        {
            public List<ApolloList> Result { get; set; }
        }




        /// <summary>
        /// Queries list items based on specific criteria and verifies the query results.
        /// </summary>
        [TestMethod]
        public async Task QueryListItemsTest()
        {
            var httpClient = Helpers.GetHttpClient();
            var query = new { ItemType = "TestListType3" };
            var jsonQuery = JsonSerializer.Serialize(query);
            var queryContent = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            var queryResponse = await httpClient.PostAsync($"{_cListController}", queryContent);
            Assert.IsTrue(queryResponse.IsSuccessStatusCode, "QueryListItems should return a successful response.");

            var responseJson = await queryResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Raw JSON response: {responseJson}");

            var listResponse = JsonSerializer.Deserialize<QueryListResponse>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assertions to validate the deserialized data
            Assert.IsNotNull(listResponse?.Result, "The response content should not be null.");
            Assert.IsNotNull(listResponse.Result.Items, "Items should not be null.");
            Assert.IsTrue(listResponse.Result.Items.Any(), "The response should contain at least one list item.");
        }



        public class QueryListResponse
        {
            public ApolloList Result { get; set; }
        }



        /// <summary>
        /// Creates or updates lists in the system and verifies the operation was successful.
        /// </summary>
        [TestMethod]
        public async Task CreateOrUpdateListTest()
        {
            var httpClient = Helpers.GetHttpClient();

            foreach (var testList in _testLists)
            {
                // If Description is null, set it to a test description
                if (string.IsNullOrEmpty(testList.Description))
                {
                    testList.Description = "Default Test Description";
                }

                var requestObj = new
                {
                    List = testList,
                    Filter = new { }  
                };

                var listRequestJson = JsonSerializer.Serialize(requestObj);
                HttpContent content = new StringContent(listRequestJson, Encoding.UTF8, "application/json");

                // Assuming PUT is correct for both creating and updating in your API
                HttpResponseMessage response = await httpClient.PutAsync($"{_cListController}", content);
                Assert.IsTrue(response.IsSuccessStatusCode, "The response should be successful.");

                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize using the wrapper class
                var listResponse = JsonSerializer.Deserialize<CreateOrUpdateListResponseWrapper>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Assert.IsNotNull(listResponse?.Result, "The response should contain a list.");
                Assert.IsFalse(string.IsNullOrEmpty(listResponse.Result.Id), "The list should have an ID.");
                Assert.IsFalse(string.IsNullOrEmpty(listResponse.Result.ItemType), "The ItemType should not be empty.");

                // Additional validation of the returned list object as needed
            }
        }

        //[TestMethod]
        //public async Task UpdateSpecificListTest()
        //{
        //    var httpClient = Helpers.GetHttpClient();

        //    var specificList = _testLists.FirstOrDefault(list => list.Id == "UT03");

        //    if (specificList != null)
        //    {
        //        var requestObj = new
        //        {
        //            List = specificList,
        //            Filter = new { }
        //        };

        //        var listRequestJson = JsonSerializer.Serialize(requestObj);
        //        HttpContent content = new StringContent(listRequestJson, Encoding.UTF8, "application/json");

        //        // Update the URL to include the list ID
        //        HttpResponseMessage response = await httpClient.PutAsync($"{_cListController}/{specificList.Id}", content);
        //        Assert.IsTrue(response.IsSuccessStatusCode, "The response should be successful.");

        //        var responseContent = await response.Content.ReadAsStringAsync();

        //        // Deserialize using the wrapper class
        //        var listResponse = JsonSerializer.Deserialize<CreateOrUpdateListResponseWrapper>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //        Assert.IsNotNull(listResponse?.Result, "The response should contain a list.");
        //        Assert.IsFalse(string.IsNullOrEmpty(listResponse.Result.Id), "The list should have an ID.");
        //        Assert.IsFalse(string.IsNullOrEmpty(listResponse.Result.ItemType), "The ItemType should not be empty.");

        //        // Additional validation of the returned list object as needed
        //    }
        //    else
        //    {
        //        Assert.Fail("The specific list was not found.");
        //    }
        //}

        public class CreateOrUpdateListResponseWrapper
        {
            public ApolloList Result { get; set; }
        }

    }
}
