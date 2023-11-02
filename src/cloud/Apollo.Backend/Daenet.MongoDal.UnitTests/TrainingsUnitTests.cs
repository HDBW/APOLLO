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
            new Training(){  Id = "T01" },
            new Training(){  Id = "T02" },
            new Training(){  Id = "T03" },
        };

        [TestMethod]
        public async Task InsertTrainingTest()
        {
            var dal = Helpers.GetDal();

            await dal.InsertAsync(Helpers.GetCollectionName<Training>(), Convertor.Convert(_testTrainings[0]));
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
