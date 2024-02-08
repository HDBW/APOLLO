// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Lists;

namespace Invite.Apollo.App.Graph.Common.Backend.Api
{
    public class GetListResponse
    {
        /// <summary>
        /// The set of list items.
        /// </summary>
        public ApolloList Result { get; set; }
    }
}
