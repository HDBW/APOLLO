using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CoursePrice : IEntity, IApolloGraphItem
    {
        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }
        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        [DataMember(Order = 3,IsRequired = true)]
        public string BackendId { get; set; } = null!;

        [DataMember(Order = 4)]
        public Uri Schema { get; set; } = null!;

        [DataMember(Order = 5,IsRequired = true)]
        [ForeignKey(nameof(CourseAppointment))]
        public long CourseAppointmentId { get; set; }

        //TODO: Proposal Price
        #region Proposal

        [ForeignKey(nameof(CourseItem))]
        public long CourseId { get; set; }
        
        public string CourseBackendId { get; set; } = null!;

        #endregion


        [DataMember(Order = 7)]
        public decimal Price { get; set; }

        [DataMember(Order = 8)]
        public string Currency { get; set; } = null!;

        [DataMember(Order = 9)]
        public DateTime? StartTime { get; set; }

        [DataMember(Order = 10)]
        public DateTime? EndTime { get; set; }

        [DataMember(Order = 11)]
        public string Description { get; set; } = null!;

        [DataMember(Order = 12)]
        public string Conditions { get; set; } = null!;
    }
}
