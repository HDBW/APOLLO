// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;


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
