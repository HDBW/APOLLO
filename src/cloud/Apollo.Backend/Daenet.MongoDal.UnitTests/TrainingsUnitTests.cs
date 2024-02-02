// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Linq;
using Apollo.Api;
using Apollo.Common.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Daenet.MongoDal.UnitTests
{
    /// <summary>
    /// Provides unit tests for the Training data access layer.
    /// </summary>
    [TestCategory("MongoDal")]
    [TestClass]
    public class TrainingsUnitTests
    {
        Training[] _testTrainings = new Training[]
        {
            new Training(){  Id = "T01", ProviderId = "unittest", TrainingName = "Open AI",
                Loans = new List<Loans>(
                new Loans[]
                {
                    new Loans() { Id = "L01", Name = "Loan 1" },
                    new Loans() { Id = "L02", Name = "Loan 2" }
                }
                )
            },

            new Training(){  Id = "T02", ProviderId = "unittest", TrainingName = "Azure AI",
             Loans = new List<Loans>(
                new Loans[]
                {
                    new Loans() { Id = "L01", Name = "Loan 1" },
                    new Loans() { Id = "L02", Name = "Loan 2" },
                    new Loans() { Id = "L03", Name = "Loan 3" },
                    new Loans() { Id = "L04", Name = "Loan 4" }
                }
                )},

            new Training(){  Id = "T03" , ProviderId = "unittest" },
        };

        private async Task CleanTestDocuments()
        {
            var dal = Helpers.GetDal();

            await dal.DeleteManyAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => t.Id).ToArray(), false);
        }


        /// <summary>
        /// Cleansup all test data.
        /// This method is called after each test to ensure clean state for subsequent tests.
        /// </summary>
        /// <returns></returns>
        [TestCleanup]
        public async Task CleanupTest()
        {
            await CleanTestDocuments();
        }


        /// <summary>
        /// Cleansup all test data.
        /// This method is called before each test to prepare necessary data for testing.
        /// </summary>
        /// <returns></returns>
        [TestInitialize]
        public async Task InitTest()
        {
            await CleanTestDocuments();
        }


        /// <summary>
        /// Tests the behavior of the Delete method when attempting to delete a non-existing document.
        /// Expects an ApplicationException to be thrown.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        [ExpectedException(typeof(ApplicationException))]
        public async Task ThrowOnDeleteNonExistingDocTest()
        {
            var dal = Helpers.GetDal();

            // Deletes not existing record with expected exception.
            await dal.DeleteAsync(Helpers.GetCollectionName<Training>(), int.MaxValue.ToString(), true);

            // Deletes not existing record without exception.            
            await dal.DeleteAsync(Helpers.GetCollectionName<Training>(), int.MaxValue.ToString(), false);
        }


        /// <summary>
        /// Tests the behavior of the Delete method when attempting to delete a non-existing document without throwing an exception.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task DoNotThrowDeleteNonExistingDocTest()
        {
            var dal = Helpers.GetDal();

            // Deletes not existing record without exception.            
            await dal.DeleteAsync(Helpers.GetCollectionName<Training>(), int.MaxValue.ToString(), false);
        }


        /// <summary>
        /// Insert and deletes many trainings.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task InsertDeleteManyTrainingsTest()
        {
            var dal = Helpers.GetDal();

            await dal.InsertManyAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => Convertor.Convert(t)).ToArray());

            var res = await dal.DeleteManyAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => t.Id).ToArray(), false);

            if (res != _testTrainings.Length)
            {

            }

            Assert.IsTrue(res == _testTrainings.Length);
        }


        /// <summary>
        /// Tests the insertion and deletion of a single Training instance.
        /// Accepts a DataRow attribute to test with different Training instances.
        /// </summary>
        /// <param name="idx">Index of the Training instance to test.</param>
        [TestMethod]
        [TestCategory("Prod")]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public async Task InsertDeleteTrainingTest(int idx)
        {
            var dal = Helpers.GetDal();

            await dal.InsertAsync(Helpers.GetCollectionName<Training>(), Convertor.Convert(_testTrainings[idx]));

            await dal.DeleteAsync(Helpers.GetCollectionName<Training>(), _testTrainings[idx].Id);
        }


        /// <summary>
        /// Tests querying for non-existing Training instances.
        /// Expects the query result to be empty.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task QueryNonExsistingTrainingTest()
        {
            var dal = Helpers.GetDal();

            var res = await dal.ExecuteQuery(Helpers.GetCollectionName<Training>(), null, new Entitties.Query()
            {
                Fields = new List<Daenet.MongoDal.Entitties.FieldExpression>()
                 {
                     new Daenet.MongoDal.Entitties.FieldExpression()
                     {
                         FieldName = "TrainingName",
                         Operator = Daenet.MongoDal.Entitties.QueryOperator.Contains,
                         Argument = new List<object>(){"sdlvkflsfsdlfkhe"},
                      }
                 }

            }, 100, 0);

            Assert.IsTrue(res?.Count == 0);
        }


        /// <summary>
        /// Tests querying for Training instances without any projection field and any filter criteria.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task QueryTrainingsBasicTest()
        {
            var dal = Helpers.GetDal();

            await dal.InsertManyAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => Convertor.Convert(t)).ToArray());

            var insertedTrainings = await dal.ExecuteQuery(Helpers.GetCollectionName<Training>(), new List<string> { "TrainingName" }, null, 100, 0);

            Console.WriteLine($"Inserted Trainings: {JsonConvert.SerializeObject(insertedTrainings)}");

            var res = await dal.ExecuteQuery(Helpers.GetCollectionName<Training>(), null, new Entitties.Query()
            {
                IsOrOperator = false,

                // Not equal to anything (means all documents) AND (because IsOrOperator = false) starts with T01 OR T02 Or T03
                Fields = new List<Daenet.MongoDal.Entitties.FieldExpression>()
                 {
                    
                 }

            }, 100, 0);

            Assert.IsTrue(res?.Count > 0, $"Expected bigger than 0, but got {res?.Count}");
        }


        /// <summary>
        /// Tests querying for Training instances based on specific criteria.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task QueryTrainingsTest()
        {
            var dal = Helpers.GetDal();

            await dal.InsertManyAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => Convertor.Convert(t)).ToArray());

            var insertedTrainings = await dal.ExecuteQuery(Helpers.GetCollectionName<Training>(), new List<string> { "TrainingName" }, null, 100, 0);

            Console.WriteLine($"Inserted Trainings: {JsonConvert.SerializeObject(insertedTrainings)}");

            var res = await dal.ExecuteQuery(Helpers.GetCollectionName<Training>(), null, new Entitties.Query()
            {
                IsOrOperator = false,

                // Not equal to anything (means all documents) AND (because IsOrOperator = false) starts with T01 OR T02 Or T03
                Fields = new List<Daenet.MongoDal.Entitties.FieldExpression>()
                 {
                     new Daenet.MongoDal.Entitties.FieldExpression()
                     {
                         FieldName = "TrainingName",
                         Operator = Daenet.MongoDal.Entitties.QueryOperator.NotEquals,
                         Argument = new List<object>(){"anything"},
                      },

                      new Daenet.MongoDal.Entitties.FieldExpression()
                     {
                         FieldName = "Id",
                         Operator = Daenet.MongoDal.Entitties.QueryOperator.StartsWith,
                         Argument = new List<object>(){"T01", "T02", "T03"},
                      }
                 }

            }, 100, 0);

            Assert.IsTrue(res?.Count > 0, $"Expected bigger than 0, but got {res?.Count}");
        }


        /// <summary>
        /// Tests querying for Training instances by name using a generic query.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task QueryTrainingsByNameGenericTest()
        {
            var dal = Helpers.GetDal();

            await dal.InsertManyAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => Convertor.Convert(t)).ToArray());

            var res = await dal.ExecuteQuery<Training>(Helpers.GetCollectionName<Training>(), null, new Entitties.Query()
            {
                Fields = new List<Daenet.MongoDal.Entitties.FieldExpression>()
                 {
                     new Daenet.MongoDal.Entitties.FieldExpression()
                     {
                         FieldName = "TrainingName",
                         Operator = Daenet.MongoDal.Entitties.QueryOperator.Contains,
                         Argument = new List<object>(){" AI", " dsd "},
                      }
                 }

            }, 100, 0);

            // Mongo Driver has the mapping issue form BsonDoc._id to T.Id.
            // We have fixed this and make here sure that ID is never NULL.
            foreach (var item in res)
            {
                Assert.IsNotNull(item.Id);
            }
          
            Assert.IsTrue(res?.Count == 2);
        }


        /// <summary>
        /// Tests querying for Training instances by name using a non-generic query.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task QueryTrainingsByNameNonGenericTest()
        {
            var dal = Helpers.GetDal();

            await dal.InsertManyAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => Convertor.Convert(t)).ToArray());

            var res = await dal.ExecuteQuery(Helpers.GetCollectionName<Training>(), null, new Entitties.Query()
            {
                Fields = new List<Daenet.MongoDal.Entitties.FieldExpression>()
                 {
                     new Daenet.MongoDal.Entitties.FieldExpression()
                     {
                         FieldName = "TrainingName",
                         Operator = Daenet.MongoDal.Entitties.QueryOperator.Contains,
                         Argument = new List<object>(){" AI", " dsd "},
                      }
                 }

            }, 100, 0);

            // Mongo Driver has the mapping issue form BsonDoc._id to T.Id.
            // We have fixed this and make here sure that ID is never NULL.
            foreach (var item in res)
            {
                Assert.IsNotNull(((IDictionary<string,object>)item!)["Id"]);
            }

            Assert.IsTrue(res?.Count == 2);
        }


        /// <summary>
        /// Tests the upsert functionality on Training instances.
        /// Verifies that the upsert operation updates the existing documents.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task UpsertTest()
        {
            var dal = Helpers.GetDal();

            await dal.InsertManyAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => Convertor.Convert(t)).ToArray());

            _testTrainings.ToList().ForEach(t =>
            {
                t.Description = nameof(UpsertTest);
            });

            await dal.UpsertAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => Convertor.Convert(t)).ToArray());

            var res = await dal.ExecuteQuery(Helpers.GetCollectionName<Training>(), null, new Entitties.Query()
            {
                Fields = new List<Daenet.MongoDal.Entitties.FieldExpression>()
                 {
                     new Daenet.MongoDal.Entitties.FieldExpression()
                     {
                         FieldName = "Description",
                         Operator = Daenet.MongoDal.Entitties.QueryOperator.Equals,
                         Argument = new List<object>(){nameof(UpsertTest)},
                      }
                 }

            }, 100, 0);

            Assert.IsTrue(res?.Count == 3);
        }


        /// <summary>
        /// Tests the count functionality by counting Training instances with certain criteria.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task CountTest()
        {
            var dal = Helpers.GetDal();

            await dal.InsertManyAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => Convertor.Convert(t)).ToArray());

            // Debug: Verify the inserted data
            var insertedTrainings = await dal.ExecuteQuery(Helpers.GetCollectionName<Training>(), new List<string> { "Loans" }, null, 100, 0);

            Console.WriteLine($"Inserted Trainings: {JsonConvert.SerializeObject(insertedTrainings)}");

            var res = await dal.ExecuteQuery(Helpers.GetCollectionName<Training>(), null, new Entitties.Query()
            {
                Fields = new List<Daenet.MongoDal.Entitties.FieldExpression>()
                 {
                     // Has some Loans
                     new Daenet.MongoDal.Entitties.FieldExpression()
                     {
                         FieldName = "Count(Loans)",
                         Operator = Daenet.MongoDal.Entitties.QueryOperator.GreaterThan,
                         Argument = new List<object>(){0},
                      },

                      new Daenet.MongoDal.Entitties.FieldExpression()
                     {
                         FieldName = "Id",
                         Operator = Daenet.MongoDal.Entitties.QueryOperator.StartsWith,
                         Argument = new List<object>(){"T"},
                      }
                 }

            }, 100, 0);

            // Filter for trainings with non-empty Loans array
            var filter = Builders<BsonDocument>.Filter.SizeGt("Loans", 0);

            // Count documents matching the filter
            var countWithLoans = await dal.CountDocumentsAsync(Helpers.GetCollectionName<Training>(), filter);


            Assert.IsTrue(res?.Count >= 2, $"Exptected >=2 trainings with loans, but got {countWithLoans}");
        }
    }
}
