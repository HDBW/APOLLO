// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Invite.Apollo.App.Graph.Common.Models.Lists
{
    /// <summary>
    /// Used as a general entity to store a list of items of some type.
    /// </summary>
    public class ApolloList
    {
        /// <summary>
        /// List identifier. Example: <see cref="ApolloApi.CreateListId"/>"/>
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Specifies the type  of the item like Qualification, Skill, etc.
        /// Also used as a partition key
        /// </summary>
        public string ItemType { get; set; } = string.Empty;

        /// <summary>
        /// Description of the item, optional.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// List of items in all languages.
        /// </summary>
        public List<ApolloListItem> Items{ get; set; } = new List<ApolloListItem>();
    }
}
