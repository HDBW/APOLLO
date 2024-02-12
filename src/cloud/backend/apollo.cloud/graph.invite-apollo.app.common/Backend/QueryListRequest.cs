// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Backend.Api
{
    public class QueryListRequest
    {
        /// <summary>
        /// Lookups only list items tha contains the specified string.
        /// </summary>
        public string? Contains { get; set; }

        /// <summary>
        /// Lookups only items of the given type.
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// The ISO code of the language.
        /// </summary>
        public string? Language { get; set; }
    }
}
