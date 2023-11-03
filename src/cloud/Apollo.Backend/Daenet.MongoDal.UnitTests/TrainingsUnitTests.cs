// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

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
            new Training(){  Id = "T01", ProviderId = "unittest" },
            new Training(){  Id = "T02", ProviderId = "unittest" },
            new Training(){  Id = "T03" , ProviderId = "unittest" },
        };


        /// <summary>
        /// Insert and delete training instances.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [TestMethod]
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
        public async Task QueryTrainingnTest()
        {
            var dal = Helpers.GetDal();

            var res = await dal.ExecuteQuery(Helpers.GetCollectionName<Training>(), null, new Entitties.Query()
            {
                Fields = new List<FieldExpression>()
                 {
                     new FieldExpression(){ FieldName = "Name",
                         Operator = QueryOperator.Contains,
                          Argument = new List<object>(){"test"}
                      }
                 }

            }, 100, 0);
        }

    }
}
