// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;
using Invite.Apollo.App.Graph.Common.Models.Lists;

namespace Invite.Apollo.App.Graph.Common.Backend.Api
{
    public class QueryListResponse
    {
        /// <summary>
        /// The set of list items.
        /// </summary>
        public List<ApolloListItem> Result { get; set; }
    }
}
