using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class AssessmentCategory : BaseItem
    {

        [DataMember(Order = 5)]
        public string Title { get; set; } = string.Empty;

        //[DataMember(Order = 6)]
        //TODO: AutoCalculate
        //public int QuestionCount { get; set; }

        /// <summary>
        /// Threshold
        /// TODO: Remove before DataBase Creation
        /// </summary>
        [DataMember(Order = 7)]
        public int ResultLimit { get; set; }

        //TODO: Remove after December
        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 8, IsRequired = true)]
        public long CourseId { get; set; }

        [DataMember(Order = 9)]
        public string Description { get; set; } = string.Empty;
    }
}
