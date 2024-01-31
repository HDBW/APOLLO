// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using Apollo.Common.Entities;
using Daenet.MongoDal;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;

namespace Apollo.Api.UnitTests
{

    /// <summary>
    /// Unit tests for the ApolloApi class.
    /// </summary>
    [TestClass]
    public class ApolloApiListUnitTests
    {
        private const string _cTestListType1 = "TestListType1";
        private const string _cTestListType2 = "TestListType2";

        /// <summary>
        /// The list of ApolloList objects used for testing.
        /// </summary>
        private List<ApolloList> _testList = new List<ApolloList>()
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
                 ItemType = _cTestListType1,
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
                 ItemType = _cTestListType2,
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
                 ItemType = _cTestListType2,
                 Items = new List<ApolloListItem>()
                 {
                     new ApolloListItem()
                     {   ListItemId = 1,
                         Lng = "DE",
                         Value = "JAVA Entwickler"
                     },
                     new ApolloListItem()
                     {   ListItemId= 2,
                         Lng = "EN",
                         Value = "C++ Developer"
                     }
                 }
             },
        };

      

        /// <summary>
        /// Creates the test lists in the database.
        /// </summary>
        [TestMethod]
        public async Task InsertTestLists()
        {
 
            var api = Helpers.GetApolloApi();

            // Create a list to store qualification IDs for all inserted lists
            List<string> allIdsInApolloList = new List<string>();

            // Insert each test list and accumulate qualification IDs
            foreach (var apolloList in _testList)
            {
                var qualificationIds = await api.CreateOrUpdateListAsync(apolloList);
                Assert.IsNotNull(qualificationIds);
                allIdsInApolloList.AddRange(qualificationIds);
            }

            // Check if the total count of qualificationIds matches the expected count
            Assert.AreEqual(_testList.SelectMany(list => list.Items).Count(), allIdsInApolloList.Count);


        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetListInternalAsyncTest()
        {
            var api = Helpers.GetApolloApi();
            ;
            var result = await api.GetListInternalAsync("DE", "_cTestListType1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("_cTestListType1", result.ItemType);
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual("DE", result.Items[0].Lng);
            Assert.AreEqual("C# Entwickler", result.Items[0].Value);
        }


        /// <summary>
        /// Tests creating or updating a Qualification List object and then cleaning up by deleting it.
        /// </summary>
        [TestMethod]
        public async Task CreateOrUpdateListsTest()
        {

            var api = Helpers.GetApolloApi();

            // Update an existing Apollo list
            var existingApolloList = _testList.FirstOrDefault();
            Assert.IsNotNull(existingApolloList, "Empty item _testList.");

            // Update the existing Apollo list
            existingApolloList.Items[1].Lng = "EN Updated";
            existingApolloList.Items[1].Value = "C# Developer Updated";

            var existingApolloListId = await api.CreateOrUpdateListAsync(existingApolloList);
            Assert.IsNotNull(existingApolloListId);

            // Retrieve the updated list
            var updatedList = await api.GetListInternalAsync("DE", _cTestListType1); 

            // Assert that the existing list is updated
            Assert.IsNotNull(updatedList);
            Assert.AreEqual("EN Updated", updatedList.Items[1].Lng);
            Assert.AreEqual("C# Developer Updated", updatedList.Items[1].Value);

            // Add a new Item to the existing List
            var newQualificationList = new ApolloList
            {
                Id = "UT01",
                ItemType = _cTestListType1,
                Items = new List<ApolloListItem>
                {
                    new ApolloListItem
                    {
                        ListItemId = 3,
                        Lng = "DE",
                        Value = "New Value Item"
                    }
                }
            };

            var newQualificationIds = await api.CreateOrUpdateQualificationAsync(newQualificationList);
            Assert.IsNotNull(newQualificationIds);

            // Retrieve the list after adding the new item
            var newListWithNewItem = await api.GetListInternalAsync("DE", _cTestListType1);

            // Assert that the new item is present in the list
            Assert.IsNotNull(newListWithNewItem);
            Assert.IsTrue(newListWithNewItem.Items.Any(item => item.ListItemId == 3 ));
       }

    


     


        /// <summary>
        /// Tests querying Qualification List objects that match with the language.
        /// </summary>
        [TestMethod]
        public async Task QueryListAsyncTest()
        {
            var api = Helpers.GetApolloApi();
            var language = "DE";
        
            // Perform the query
            var results = await api.QueryListInternalAsync(language, "TestListType1", null);

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);

        }


        /// <summary>
        /// Makes sure that all test records are deleted after each test execution.
        /// </summary>
        /// <returns></returns>
        [TestCleanup]
        private async Task Cleanup()
        {
            var api = Helpers.GetApolloApi();

            var deleteResult = await api.DeleteListInternalAsync(_testList.Select(l => l.Id).ToArray());
            Assert.AreEqual(_testList.Count, deleteResult);
        }


        /// <summary>
        /// Tests querying Qualification List objects that match with the language.
        /// </summary>
        [TestMethod]
        public async Task QueryQualificationListTest()
        {
            var api = Helpers.GetApolloApi();
            var language = "DE";

            string containsFilter = "New Language Programmer";

            /// Perform the query
            var results = await api.QueryQualificationsListAsync(language, containsFilter);

            /// Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);
            /// Check Name filter working properly
            /// Check that all items contains the filter text.
            foreach (var item in results)
            {
                Assert.IsTrue(item.Contains(containsFilter));
            }

            /// Extract qualification IDs from the results
            //var qualificationIdsToDelete = results.Select(result => result.Id).ToArray();

            /// Delete qualifications by passing the array of IDs
            //var deleteResult = await api.DeleteQualificationAsync(qualificationIdsToDelete);
            //Assert.AreEqual(qualificationIdsToDelete.Length, deleteResult);


        }

    }
}

