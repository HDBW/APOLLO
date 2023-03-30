using System;
using Invite.Apollo.App.Graph.Common.Models.ContentManagement;
using ProtoBuf;


namespace Invite.Apollo.App.Graph.Common.Models
{
    public interface ICdnEntity : IPublishingInfo
    {
        public Uri DocumentUrl { get; set; }
    }
}
