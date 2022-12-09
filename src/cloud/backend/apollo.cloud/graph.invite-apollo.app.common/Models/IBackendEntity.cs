using System;
using ProtoBuf;


namespace Invite.Apollo.App.Graph.Common.Models
{
    public interface IBackendEntity
    {
        /// <summary>
        /// Used as Unique Identifier for JSON-LD in the Backend
        /// </summary>
        public Uri Schema { get; set; }
    }
}
