// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.RestService.Messages
{
    /// <summary>
    /// Defines the requeste message of the GetTrainings operation.
    /// </summary>
    public class QueryTrainingsRequest
    {
        public string Contains { get; set; }

        public string Id { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
    }
}
