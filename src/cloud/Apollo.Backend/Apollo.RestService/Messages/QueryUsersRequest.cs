// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Daenet.MongoDal.Entitties;

namespace Apollo.RestService.Messages
{
    /// <summary>
    /// Defines the request message for querying users.
    /// </summary>
    public class QueryUsersRequest
    {
        QueryOperator  Query {  get; set; }

        [Obsolete]
        public string Contains { get; set; }
        [Obsolete]
        public DateTime? From { get; set; }
        [Obsolete]
        public DateTime? To { get; set; }
        // Add any other properties relevant to  user query here.
    }
}
