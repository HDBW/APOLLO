// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using Invite.Apollo.App.Graph.Common.Models.ContentManagement;


namespace Invite.Apollo.App.Graph.Common.Models
{
    public interface ICdnEntity : IPublishingInfo
    {
        public Uri DocumentUrl { get; set; }
    }
}
