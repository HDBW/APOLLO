// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Daenet.MongoDal.Entitties;

namespace Apollo.RestService.Messages
{
    /// <summary>
    /// Defines the requeste message of the GetTrainings operation.
    /// </summary>
    public class QueryTrainingsRequest
    {
        QueryOperator Query { get; set; }

        [Obsolete]
        public string Contains { get; set; }

        [Obsolete]
        public DateTime? From { get; set; }

        [Obsolete]
        public DateTime? To { get; set; }
    }
}
