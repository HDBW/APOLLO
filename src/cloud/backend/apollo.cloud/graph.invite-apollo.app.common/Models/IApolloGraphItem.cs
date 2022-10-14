using System;

namespace Invite.Apollo.App.Graph.Common.Models
{
    public interface IApolloGraphItem
    {
        /// <summary>
        /// Unique Identifier used in the Backend Graph Database
        /// </summary>
        public string BackendId { get; set; }

        /// <summary>
        /// Used as Unique Identifier for JSON-LD in the Backend
        /// </summary>
        public Uri Schema { get; set; }
    }
}
