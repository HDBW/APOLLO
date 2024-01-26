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
        /// <summary>
        /// Tests creating or updating a Qualification List object and then cleaning up by deleting it.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task CreateOrUpdateQualificationListTest()
        {
            var api = Helpers.GetApolloApi();

            /// Create a new qualification
              var qualificationListItems = new List<ListItem>
                    {
                        new ListItem
                        {
                            Name = "C# Programmer",
                            Description = "C# Programming Language",
                            Lng = "123"
                        },
                        new ListItem
                        {
                            Name = "Python Programmer",
                            Description = "Python Programming Language",
                            Lng = "124"
                        }
                    };

            /// Create a new List object and add the qualification to the Items property
            var qualificationList = new List
            {
                ItemType = "Qualification",
                Items = qualificationListItems
            };

            /// Insert the new qualification
            var qualificationIds = await api.CreateOrUpdateQualificationAsync(new List<List> { qualificationList });
            Assert.IsNotNull(qualificationIds);
            Assert.AreEqual(1, qualificationIds.Count);

            /// Update an existing qualification (assuming there is an existing qualification with the same ItemType and language)
            var updatedQualificationListItems = new List<ListItem>
            {
                new ListItem
                {
                    Name = "Updated C# Programmer",
                    Description = "Updated C# Programming Language",
                    Lng = "123"
                },
                new ListItem
                {
                    Name = "New Language Programmer",
                    Description = "New Language Programming Language",
                    Lng = "125"
                }
            };

            /// Create an updated List with the updated qualification and a new qualification with same ID
            var updatedQualificationList = new List
            {
                Id = qualificationIds[0],
                ItemType = "Qualification",
                Items = updatedQualificationListItems
            };

            /// Update the existing qualification and add a new qualification
            var updatedQualificationIds = await api.CreateOrUpdateQualificationAsync(new List<List> { updatedQualificationList });
            Assert.IsNotNull(updatedQualificationIds);
            Assert.AreEqual(1, updatedQualificationIds.Count);

            /// Additional cleanup if needed (delete the qualification)
            //var deleteResult = await api.DeleteQualificationAsync(new string[] { qualificationIds[0] });
            // Assert.AreEqual(1, deleteResult); 

        }


        /// <summary>
        /// Tests querying Qualification List objects that match with the language.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task QueryQualificationListTest()
        {
            var api = Helpers.GetApolloApi();
            var language = "125";
            var itemType = "Qualification";
            List<string> namesToFilter = new List<string> { "New Language Programmer" };

            var filter = new Common.Entities.Query
            {
                Filter = new Filter
                {
                    Fields = new List<Common.Entities.FieldExpression>
            {
                new Common.Entities.FieldExpression
                {
                    Operator = Common.Entities.QueryOperator.Equals,
                    Argument = new List<object> { itemType },
                    FieldName = nameof(List.ItemType)
                },
                new Common.Entities.FieldExpression
                {
                    Operator = Common.Entities.QueryOperator.Contains,
                    Argument = namesToFilter.Cast<object>().ToList(),
                    FieldName = "Items.Name"
                }
            }
                }
            };

            /// Perform the query
            var results = await api.QueryItemsListAsync(language, filter);

            /// Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);
            /// Check that all items have names that match the specified names
            var matchingNames = results.SelectMany(result => result.Items.Select(item => item.Name)).ToList();
            CollectionAssert.AreEquivalent(namesToFilter, matchingNames);

            /// Extract qualification IDs from the results
            var qualificationIdsToDelete = results.Select(result => result.Id).ToArray();

            /// Delete qualifications by passing the array of IDs
            var deleteResult = await api.DeleteQualificationAsync(qualificationIdsToDelete);
            Assert.AreEqual(qualificationIdsToDelete.Length, deleteResult); 


        }

    }
}
