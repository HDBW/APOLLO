// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;
using Apollo.Service.Controllers;

namespace Apollo.RestService.Messages
{
    /// <summary>
    /// Defines the response message of the <see cref="ListController.QueryListItemsAsync"/> operation.
    /// </summary>
    public class QueryListResponse
    {
        /// <summary>
        /// The set of list items.
        /// </summary>
        public required List<ApolloListItem> Result { get; set; }
    }
}
