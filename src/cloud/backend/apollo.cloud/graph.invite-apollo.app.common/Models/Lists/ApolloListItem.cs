// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Models.Lists
{
    /// <summary>
    ///  The item stored in the DB.
    ///  Describes the item in the list in the specific language.
    /// </summary>
    public class ApolloListItem
    {
        /// <summary>
        /// Identifier of the item in the list.
        /// </summary>
        public int ListItemId { get; set; }

        /// <summary>
        /// The ISO language code.
        /// </summary>
        public string? Lng { get; set; }

        /// <summary>
        /// Optionl description of the values.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string Value { get; set; }

    }
}
