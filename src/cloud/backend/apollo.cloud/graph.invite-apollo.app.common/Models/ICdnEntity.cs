using System;
using Invite.Apollo.App.Graph.Common.Models.ContentManagement;

namespace Invite.Apollo.App.Graph.Common.Models
{
    public interface ICdnEntity : IPublishingInfo
    {
        public Uri DocumentUrl { get; set; }
    }
}
