// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Linq;
using Apollo.Api;
using Apollo.Common.Entities;
using Daenet.MongoDal.Entitties;

namespace Daenet.MongoDal.UnitTests
{
    [TestCategory("MongoDal")]
    [TestClass]
    public class TrainingsUnitTests
    {
        Training[] _testTrainings = new Training[]
        {
            new Training(){  Id = "T01", ExternalTrainingId = "unittest", TrainingName = "Open AI" },
            new Training(){  Id = "T02", ExternalTrainingId = "unittest", TrainingName = "Azure AI" },
            new Training(){  Id = "T03" , ExternalTrainingId = "unittest" },
        };

        private async Task CleanTestDocuments()
        {
            var dal = Helpers.GetDal();

            await dal.DeleteManyAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => t.Id).ToArray(), false);
        }

        /// <summary>
        /// Cleansup all test data.
        /// </summary>
        /// <returns></returns>
        [TestCleanup]
        public async Task CleanupTest()
        {
            await CleanTestDocuments();
        }


        /// <summary>
        /// Cleansup all test data.
        /// </summary>
        /// <returns></returns>
        [TestInitialize]
        public async Task InitTest()
        {
            await CleanTestDocuments();
        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public async Task ThrowOnDeleteNonExistingDocTest()
        {
            var dal = Helpers.GetDal();

            // Deletes not existing record with expected exception.
            await dal.DeleteAsync(Helpers.GetCollectionName<Training>(), int.MaxValue.ToString(), true);

            // Deletes not existing record without exception.            
            await dal.DeleteAsync(Helpers.GetCollectionName<Training>(), int.MaxValue.ToString(), false);
        }

        [TestMethod]
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

            Assert.IsTrue(res == _testTrainings.Length);
        }

        /// <summary>
        /// Insert and delete training instances.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [TestMethod]
        //[TestCategory("Prod")]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public async Task InsertDeleteTrainingTest(int idx)
        {
            var dal = Helpers.GetDal();

            await dal.InsertAsync(Helpers.GetCollectionName<Training>(), Convertor.Convert(_testTrainings[idx]));

            await dal.DeleteAsync(Helpers.GetCollectionName<Training>(), _testTrainings[idx].Id);
        }



        [TestMethod]
        public async Task QueryNonExsistingTrainingTest()
        {
            var dal = Helpers.GetDal();

            var res = await dal.ExecuteQuery(Helpers.GetCollectionName<Training>(), null, new Entitties.Query()
            {
                Fields = new List<Daenet.MongoDal.Entitties.FieldExpression>()
                 {
                     new Daenet.MongoDal.Entitties.FieldExpression()
                     {
                         FieldName = "Name",
                         Operator = Daenet.MongoDal.Entitties.QueryOperator.Contains,
                         Argument = new List<object>(){"test"},
                      }
                 }

            }, 100, 0);

            Assert.IsTrue(res?.Count == 0);
        }

        [TestMethod]
        public async Task QueryTrainingsTest()
        {
            var dal = Helpers.GetDal();

            await dal.InsertManyAsync(Helpers.GetCollectionName<Training>(), _testTrainings.Select(t => Convertor.Convert(t)).ToArray());

            var res = await dal.ExecuteQuery(Helpers.GetCollectionName<Training>(), null, new Entitties.Query()
            {
                Fields = new List<Daenet.MongoDal.Entitties.FieldExpression>()
                 {
                     new Daenet.MongoDal.Entitties.FieldExpression()
                     {
                         FieldName = "Name",
                         Operator = Daenet.MongoDal.Entitties.QueryOperator.NotEquals,
                         Argument = new List<object>(){"anything"},
                      }
                 }

            }, 100, 0);

            Assert.IsTrue(res?.Count == 3);
        }

        [TestMethod]
        public async Task QueryTrainingsByNameTest()
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
                         Argument = new List<object>(){"AI"},
                      }
                 }

            }, 100, 0);

            Assert.IsTrue(res?.Count == 2);
        }

        [TestMethod]
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
    }
}
