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
        private List<ApolloList> _tesList = new List<ApolloList>()
        {
             new ApolloList()
             {
                 Id = "UT01",
                 ItemType = _cTestListType1,
                 Items = new List<ApolloListItem>()
                 {
                     new ApolloListItem()
                     {
                         Lng = "DE",
                         Value = "C# Entwickler"
                     },
                     new ApolloListItem()
                     {
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
                         Lng = "DE",
                         Value = "Python Entwickler"
                     },
                     new ApolloListItem()
                     {
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
                         Lng = "DE",
                         Value = "C++ Entwickler"
                     },
                     new ApolloListItem()
                     {
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
                     {
                         Lng = "DE",
                         Value = "JAVA Entwickler"
                     },
                     new ApolloListItem()
                     {
                         Lng = "EN",
                         Value = "C++ Developer"
                     }
                 }
             },
        };

        /// <summary>
        /// Makes sure that all test records are deleted after each test execution.
        /// </summary>
        /// <returns></returns>
        [TestCleanup]
        private async Task Cleanup()
        {
            var api = Helpers.GetApolloApi();

            var deleteResult = await api.DeleteListInternalAsync(_tesList.Select(l=>l.Id).ToArray());
            Assert.AreEqual(_tesList.Count, deleteResult);
        }

        /// <summary>
        /// Creates the test lists in the database.
        /// </summary>
        /// <returns></returns>
        private async Task InsertTestLists()
        {
            var api = Helpers.GetApolloApi();
            
        }

        ///// <summary>
        ///// Tests creating or updating a Qualification List object and then cleaning up by deleting it.
        ///// </summary>
        //[TestMethod]
        //[TestCategory("Prod")]
        //public async Task CreateOrUpdateQualificationListTest()
        //{
        //    var api = Helpers.GetApolloApi();

        //    /// Create a new qualification
        //      var qualificationListItems = new List<ListItem>
        //            {
        //                new ListItem
        //                {
        //                    Name = "C# Programmer",
        //                    Description = "C# Programming Language",
        //                    Lng = "123"
        //                },
        //                new ListItem
        //                {
        //                    Name = "Python Programmer",
        //                    Description = "Python Programming Language",
        //                    Lng = "124"
        //                }
        //            };

        //    /// Create a new List object and add the qualification to the Items property
        //    var qualificationList = new ApolloList
        //    {
        //        ItemType = "Qualification",
        //        Items = qualificationListItems
        //    };

        //    /// Insert the new qualification
        //    var qualificationIds = await api.CreateOrUpdateQualificationAsync(new List<ApolloList> { qualificationList });
        //    Assert.IsNotNull(qualificationIds);
        //    Assert.AreEqual(1, qualificationIds.Count);

        //    /// Update an existing qualification (assuming there is an existing qualification with the same ItemType and language)
        //    var updatedQualificationListItems = new List<ListItem>
        //    {
        //        new ListItem
        //        {
        //            Name = "Updated C# Programmer",
        //            Description = "Updated C# Programming Language",
        //            Lng = "123"
        //        },
        //        new ListItem
        //        {
        //            Name = "New Language Programmer",
        //            Description = "New Language Programming Language",
        //            Lng = "125"
        //        }
        //    };

        //    /// Create an updated List with the updated qualification and a new qualification with same ID
        //    var updatedQualificationList = new ApolloList
        //    {
        //        Id = qualificationIds[0],
        //        ItemType = "Qualification",
        //        Items = updatedQualificationListItems
        //    };

        //    /// Update the existing qualification and add a new qualification
        //    var updatedQualificationIds = await api.CreateOrUpdateQualificationAsync(new List<ApolloList> { updatedQualificationList });
        //    Assert.IsNotNull(updatedQualificationIds);
        //    Assert.AreEqual(1, updatedQualificationIds.Count);

        //    /// Additional cleanup if needed (delete the qualification)
        //    //var deleteResult = await api.DeleteQualificationAsync(new string[] { qualificationIds[0] });
        //    // Assert.AreEqual(1, deleteResult); 

        //}


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public Task GetListInternalAsyncTest()
        {
            throw new NotImplementedException();
            //TODO.
            // Create Test ApolloListItem. Make it as a field.
        }

        /// <summary>
        /// Tests querying Qualification List objects that match with the language.
        /// </summary>
        [TestMethod]
        [TestCategory("It would be nice to be Prod")]
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
