// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Apollo.Common.Entities
{
    public class QueryResponse
    {
        [Newtonsoft.Json.JsonProperty("query")]
        public Filter Query { get; set; }

        [Newtonsoft.Json.JsonProperty("trainings")]
        public IEnumerable<Training> Trainings { get; set; }
    }
}
