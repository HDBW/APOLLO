using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Esco;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class AssessmentItem : IEntity
    {
        public AssessmentItem()
        {
            Occupations = new Collection<Occupation>();
            Skills = new Collection<Skill>();
        }

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

        [DataMember(Order = 3)] public string Title { get; set; } = null!;

        [DataMember(Order = 4)]
        public Collection<Occupation> Occupations { get; set; }

        [DataMember(Order = 5)]
        public Collection<Skill> Skills { get; set; }
    }

    [DataContract]
    public class AssessmentRequest : ICorrelationId
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string CorrelationId { get; set; } = null!;

        [DataMember(Order = 2)]
        public long? AssessmentId { get; set; }

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

        //TODO: Add Occupation

        //TODO: Add Skills
    }

}
