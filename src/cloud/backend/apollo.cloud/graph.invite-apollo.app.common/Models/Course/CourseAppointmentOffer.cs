using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseAppointmentOffer : IEntity, IApolloGraphItem
    {
        [Key]
        [DataMember(Order = 1,IsRequired = true)]
        public long Id { get; set; }
        [DataMember(Order = 2,IsRequired = true)]
        public long Ticks { get; set; }
        [DataMember(Order = 3, IsRequired = true)]
        public string BackendId { get; set; } = null!;

        [DataMember(Order = 4, IsRequired = true)]
        public Uri Schema { get; set; } = null!;

        #region References

        [DataMember(Order = 5, IsRequired = true)]
        [ForeignKey(nameof(CourseAppointment))]
        public long CourseAppointmentId { get; set; }

        [DataMember(Order = 6, IsRequired = true)]
        public long CoursePriceId { get; set; }


        #endregion

        [DataMember(Order = 7)]
        public OfferType OfferType { get; set; }

        [DataMember(Order = 8)]
        public string OfferDetails { get; set; } = null!;

        [DataMember(Order = 9)]
        public Uri OfferUrl { get; set; } = null!;

        [DataMember(Order = 10)]
        public bool IsAvailable { get; set; }

    }
}
