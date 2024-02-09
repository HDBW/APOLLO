// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;
using Apollo.Service.Controllers;

namespace Apollo.RestService.Messages
{
    /// <summary>
    /// Defines the response message of the <see cref="nameof(ListController.GetAllListsAsync)"/> operation.
    /// </summary>
    public class GetAllListResponse
    {
        /// <summary>
        /// The set of list of ItemType and Id.
        /// </summary>
        public List<ApolloList>? Result { get; set; }
    }
}
