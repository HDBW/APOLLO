// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;
using Apollo.Service.Controllers;

namespace Apollo.RestService.Messages
{
    /// <summary>
    /// Defines the request message of the <see cref="nameof(ListController.GetListAsync)"/> operation.
    /// </summary>
    public class GetListResponse
    {
        /// <summary>
        /// The set of list items.
        /// </summary>
        public ApolloList Result { get; set; }
    }
}
