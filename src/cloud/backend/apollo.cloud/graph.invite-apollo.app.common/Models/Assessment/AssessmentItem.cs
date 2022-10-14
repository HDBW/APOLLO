using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Esco;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class AssessmentItem : IEntity
    {
        /// <summary>
        /// Indicates unique Identifier for client database
        /// </summary>
        [DataMember(Order = 1)]
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Indicates latest update on Assessment
        /// </summary>
        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        [DataMember(Order = 3)]
        public string Title { get; set; }

        [DataMember(Order = 4)]
        public List<Occupation> Occupations { get; set; }

        [DataMember(Order = 5)]
        public List<Skill> Skills { get; set; }
    }

    [DataContract]
    public class AssessmentRequest : ICorrelationId
    {
        [DataMember(Order = 1,IsRequired = true)]
        public string CorrelationId { get; set; }

        [DataMember(Order = 2)]
        public long AssessmentId { get; set; }

        [DataMember(Order = 3)]
        public string EscoOccupationId { get; set; }

        [DataMember(Order = 4)]
        public long Ticks { get; set; }

        //[DataMember(Order = 3)]
        //public CultureInfo CultureInfo { get; set; }
    }

    [DataContract]
    public class AssessmentResponse : ICorrelationId
    {
        [DataMember(Order = 1,IsRequired = true)]
        public string CorrelationId { get; set; }

        [DataMember(Order = 2)]
        public List<AssessmentItem> Assessments { get; set; }
    }

}
