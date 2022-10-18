using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class EduProvider : IEntity,IApolloGraphItem
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
        public string BackendId { get; set; } = null!;

        [DataMember(Order = 4)]
        public Uri Schema { get; set; } = null!;

        #endregion

        [DataMember(Order = 5,IsRequired = true)]
        public string Name { get; set; } = null!;

        [DataMember(Order = 6)]
        public string Description { get; set; } = null!;

        [DataMember(Order = 7)]
        public Uri Website { get; set; } = null!;

        [DataMember(Order = 8)]
        public Uri Logo { get; set; } = null!;
    }
}
