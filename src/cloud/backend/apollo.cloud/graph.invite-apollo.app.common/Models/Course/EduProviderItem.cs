using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class EduProviderItem : IEntity,IBackendEntity
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

        [DataMember(Order = 5,IsRequired = true)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 6)]
        public string Description { get; set; } = string.Empty;

        [DataMember(Order = 7)]
        public Uri Website { get; set; } = null!;

        [DataMember(Order = 8)]
        public Uri Logo { get; set; } = null!;
    }
}
