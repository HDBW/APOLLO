// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Daenet.MongoDal.Entitties;

namespace Daenet.MongoDal.UnitTests
{
    [TestClass]
    public class UnitTests
    {

        [TestMethod]
        public async Task TestMethod1()
        {
            var dal = Helpers.GetDal();

            var res = await dal.ExecuteQuery("users", null, new Entitties.Query()
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
