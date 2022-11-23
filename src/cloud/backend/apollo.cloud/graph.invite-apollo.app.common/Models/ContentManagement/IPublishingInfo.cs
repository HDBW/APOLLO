using System;

namespace Invite.Apollo.App.Graph.Common.Models.ContentManagement
{
    public interface IPublishingInfo
    {

        public DateTime? PublishingDate { get; set; }

        public DateTime? LatestUpdate { get; set; }

        public DateTime? Deprecation { get; set; }

        public string? DeprecationReason { get; set; }

        public DateTime? UnPublishingDate { get; set; }

        public DateTime? Deleted { get; set; }

        public long? SuccessorId { get; set; }

        public long? PredecessorId { get; set; }


    }
}
