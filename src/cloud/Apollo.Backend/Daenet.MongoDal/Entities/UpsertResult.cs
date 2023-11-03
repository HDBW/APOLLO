using System;
using System.Collections.Generic;
using System.Text;

namespace Daenet.MongoDal.Entitties
{
    /// <summary>
    /// The result of the upsert operation.
    /// </summary>
    public class UpsertResult
    {
        /// <summary>
        /// Number of inserted records.
        /// </summary>
        public int Inserted { get; set; }

        /// <summary>
        /// Number of updated records.
        /// </summary>
        public int Updated { get; set; }
    }
}
