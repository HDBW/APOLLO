using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Esco;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class AssessmentItem : IEntity, IBackendEntity
    {
        public AssessmentItem()
        {
        }

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

        [DataMember(Order = 3)] public string Title { get; set; } = null!;
    }

    [DataContract]
    public class AssessmentRequest : ICorrelationId
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string CorrelationId { get; set; } = null!;

        [DataMember(Order = 2)]
        public long? AssessmentBackendId { get; set; }

        [DataMember(Order = 3)]
        public string? EscoOccupationId { get; set; }

        [DataMember(Order = 4)]
        public long? Ticks { get; set; }

        [DataMember(Order=5)]
        public CultureInfo? CultureInfo { get; set; }
    }

    [DataContract]
    public class AssessmentResponse : ICorrelationId
    {
        public AssessmentResponse()
        {
            Assessments = new Collection<AssessmentItem>();
        }

        [DataMember(Order = 1, IsRequired = true)]
        public string CorrelationId { get; set; } = null!;

        [DataMember(Order = 2)]
        public Collection<AssessmentItem> Assessments { get; set; }
    }

}
