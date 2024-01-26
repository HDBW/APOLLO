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
        public async Task CreateQualificationListTest()
        {
            var api = Helpers.GetApolloApi();

            /// Create a new qualification
            var qualificationlListItems = new List<ListItem>
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

            /// Create a new List and add the qualification to the Items property
            var qualificationList = new List
            {
                ItemType = "Qualification",
                Items = qualificationlListItems
            };

            /// Insert the new qualification
            var qualificationIds = await api.CreateOrUpdateQualificationAsync(new List<List> { qualificationList });
            Assert.IsNotNull(qualificationIds);
            Assert.AreEqual(1, qualificationIds.Count);
        }

        /// <summary>
        /// Tests querying Qualification List objects that match with the language.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task QueryQualificationListTest()
        {
            var api = Helpers.GetApolloApi();
            var language = "124";
            var itemType = "Qualification";
            List<string> namesToFilter = new List<string> { "Python" };

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

            // Perform the query
            var result = await api.QueryItemsListAsync(language, filter);
            /// Assert
            Assert.IsNotNull(result);
       

        }




    }
}
