using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CoursePriceItem : IEntity, IBackendEntity
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

        //[DataMember(Order = 5,IsRequired = true)]
        //[ForeignKey(nameof(CourseAppointment))]
        //public long CourseAppointmentId { get; set; }

        ////TODO: Proposal Price
        //#region Proposal

        //[ForeignKey(nameof(CourseItem))]
        //public long CourseId { get; set; }
        
        //public long CourseBackendId { get; set; }

        //#endregion


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
