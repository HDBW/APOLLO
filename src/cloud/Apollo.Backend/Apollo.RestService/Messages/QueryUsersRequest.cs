// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.RestService.Messages
{
    /// <summary>
    /// Defines the request message for querying users.
    /// </summary>
    public class QueryUsersRequest
    {
       

        public string Contains { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        // Add any other properties relevant to  user query here.
    }
}
