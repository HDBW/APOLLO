﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models
{
    [DataContract]
    public class DocumentItem : IBackendEntity, IEntity, ICdnEntity
    {

        #region Implementation of IEntity
        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity
        [DataMember(Order = 3, IsRequired = true)]
        public long BackendId { get; set; }

        [DataMember(Order = 4, IsRequired = true)]
        public Uri Schema { get; set; } = null!;

        #endregion



        /// <summary>
        /// Hash of the document
        /// </summary>
        [DataMember(Order = 6)]
        public string Hash { get; set; } = null!;

        /// <summary>
        /// Description of the document
        /// </summary>
        [DataMember(Order = 7)]
        public string TextSummarization { get; set; } = null!;

        /// <summary>
        /// Key Takeaways of the document
        /// </summary>
        [DataMember(Order = 8)]
        public string KeyPhrases { get; set; } = null!;

        #region Implementation of IPublishingInfo

        /// <summary>
        /// Url to the document on the CDN
        /// </summary>
        [DataMember(Order = 5, IsRequired = true)]
        public Uri DocumentUrl { get; set; } = null!;

        [DataMember(Order = 9,IsRequired = false)]
        public DateTime? PublishingDate { get; set; }
        [DataMember(Order = 10, IsRequired = false)]
        public DateTime? LatestUpdate { get; set; }
        [DataMember(Order = 11, IsRequired = false)]
        public DateTime? Deprecation { get; set; }
        [DataMember(Order = 12, IsRequired = false)]
        public string? DeprecationReason { get; set; }
        [DataMember(Order = 13, IsRequired = false)]
        public DateTime? UnPublishingDate { get; set; }
        [DataMember(Order = 14, IsRequired = false)]
        public DateTime? Deleted { get; set; }
        [DataMember(Order = 15, IsRequired = false)]
        public long? SuccessorId { get; set; }
        [DataMember(Order = 16, IsRequired = false)]
        public long? PredecessorId { get; set; }
        [DataMember(Order = 17, IsRequired = false)]
        public long? SuccessorBackendId { get; set; }
        [DataMember(Order = 18, IsRequired = false)]
        public long? PredecessorBackendId { get; set; }


        #endregion
    }
}