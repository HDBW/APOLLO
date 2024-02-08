// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;

namespace Invite.Apollo.App.Graph.Common.Backend.Api
{
    public class GetListRequest
    {
        /// <summary>
        /// ISO Code of the language. If set, it returns only the list items of the given language and the given <see cref="ItemType"/>.
        /// </summary>
        public string? Lng { get; set; }

        /// <summary>
        /// Lookups only items of the given type. If set, it returns only the list items of the given language and the given <see cref="ItemType"/>.
        /// </summary>
        public string? ItemType { get; set; }

        /// <summary>
        /// If set, it returns only the list items with the given ids. In this case, the <see cref="ItemType"/> and <see cref="Lng"/> are ignored.
        /// </summary>
        public string[] Ids { get; set; } = Array.Empty<string>();
    }
}
