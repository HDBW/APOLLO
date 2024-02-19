// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;
using Apollo.Service.Controllers;

namespace Apollo.RestService.Messages
{
    /// <summary>
    /// Defines the request message of the <see cref="ListController.QueryListItemsAsync(QueryListRequest)"/> operation.
    /// </summary>
    public class QueryListRequest
    {
        /// <summary>
        /// Lookups only list items tha contains the specified string.
        /// </summary>
        public string? Contains { get; set; }

        /// <summary>
        /// Lookups only items of the given type.
        /// </summary>
        public required string ItemType { get; set; }

        /// <summary>
        /// The ISO code of the language.
        /// </summary>
        public string? Language { get; set; }
    }
}
