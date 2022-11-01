using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class LoanOption : IEntity, IBackendEntity

    {
        #region client stuff
        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        #endregion

        #region cloud stuff

        [DataMember(Order = 3, IsRequired = true)]
        public long BackendId { get; set; }

        [DataMember(Order = 4)]
        public Uri Schema { get; set; } = null!;

        #endregion

        [DataMember(Order = 5)]
        public string Title { get; set; } = string.Empty;

        [DataMember(Order = 6)]
        public string Description { get; set; } = string.Empty;

        [DataMember(Order = 7)]
        public string Conditions { get; set; } = string.Empty;

        [DataMember(Order = 8)]
        public Uri LoanOptionsUrl { get; set; } = null!;

        [DataMember(Order = 9)]
        public Uri LoanOptionsImage { get; set; } = null!;

        [DataMember(Order = 10)]
        public bool? IsAvailable { get; set; }
    }
}
