
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Graph.Apollo.Cloud.Common.Models.Assessment.Enums;

namespace Graph.Apollo.Cloud.Common.Models.Assessment
{
    [DataContract]
    public class QuestionItem : IEntity
    {
        [DataMember(Order = 1, IsRequired = true)]
        [Key]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        [ForeignKey(nameof(AssessmentItem))]
        public long AssessmentId { get; set; }

        [DataMember(Order = 3, IsRequired = true)]
        public long Ticks { get; set; }

        [DataMember(Order = 4)]
        public LayoutType QuestionLayout { get; set; }

        [DataMember(Order = 5)]
        public LayoutType AnswerLayout { get; set; }

        [DataMember(Order = 6)]
        public InteractionType Interaction { get; set; }
    }
}
