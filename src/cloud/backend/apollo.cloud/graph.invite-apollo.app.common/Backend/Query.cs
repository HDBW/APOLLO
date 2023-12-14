// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Apollo.Common.Entities
{
    public class Query
    {
        /// <summary>
        /// Specify which properties of the Entity should be returned in the response message, if Fields is empty, all fields in the database are returned
        /// </summary>
        [Newtonsoft.Json.JsonProperty("fields")]
        public List<string>? Fields { get; set; }

        [Newtonsoft.Json.JsonProperty("filter")]
        public Filter Filter { get; set; }

        /// <summary>
        /// If set on true, then the response will contain the number of pages and records (items).
        /// If set on false, the number of pages is set on -1. We use this argument, to avoid a double query inside of backend when number of pages is required.
        /// To calculate the number of pages, the backend first executes the query and then execute the request to get the number of available items for the given query,
        /// which is finally recalculated in the number of pages.
        /// In a case of FALSE (default), the second query is not executed.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("requestCount")]
        public bool? RequestCount { get; set; }

        /// <summary>
        /// Number of items return e.g., pageSize
        /// </summary>
        [Newtonsoft.Json.JsonProperty("top")]
        public int Top { get; set; } = 100;

        /// <summary>
        /// Use for paging indicated by (page - 1) * pageSize
        /// </summary>
        [Newtonsoft.Json.JsonProperty("skip")]
        public int Skip { get; set; } = 0;

        /// <summary>
        /// Specify the order of the return items by the specified field.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("sortExpression")]
        public SortExpression? SortExpression { get; set; } 
    }
}
