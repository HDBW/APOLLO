// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Daenet.MongoDal.UnitTests
{
    [TestClass]
    public class UnitTests
    {

        [TestMethod]
        public async Task TestMethod1()
        {
            var dal = Helpers.GetDal();

            await dal.ExecuteQuery("users", null, new Entitties.Query()
            {

            }, 100, 0);
        }

    }
}
