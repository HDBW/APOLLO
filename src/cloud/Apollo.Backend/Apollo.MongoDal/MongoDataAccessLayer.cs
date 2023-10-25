// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.MongoDal.Entitties;

namespace Apollo.MongoDal
{
    /// <summary>
    /// Implements the Data Access Layer for communication with the Mongo Database.
    /// </summary>
    public class MongoDataAccessLayer
    {
        private readonly MongoDalConfig _cfg;

        public MongoDataAccessLayer(MongoDalConfig cfg)
        {
            _cfg = cfg;
        }
    }
}
