using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseModules : IEntity, IApolloGraphItem
    {
        [DataMember(Order = 1,IsRequired = true)]
        [Key]
        public long Id { get; set; }

        [DataMember(Order = 2,IsRequired = true)]
        public long Ticks { get; set; }

        [DataMember(Order = 3,IsRequired = true)]
        public string BackendId { get; set; }

        [DataMember(Order = 4)]
        public Uri Schema { get; set; }

        [DataMember(Order=5,IsRequired = true)]
        [ForeignKey(nameof(CourseItem))]
        public long CourseId { get; set; }

        [DataMember(Order = 6,IsRequired = true)]
        public string CourseBackendId { get; set; }

        [DataMember(Order = 7,IsRequired = true)]
        [ForeignKey(nameof(CourseItem))]
        public long Module { get; set; }

        [DataMember(Order = 8,IsRequired = true)]
        public string CourseBackendModule { get; set; }
    }
}
