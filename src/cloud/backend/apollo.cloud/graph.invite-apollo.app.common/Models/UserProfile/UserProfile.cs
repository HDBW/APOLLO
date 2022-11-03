using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    [DataContract]
    public class UserProfileItem : IEntity, IBackendEntity
    {
        #region Implementation of IEntity

        [Key]
        [DataMember(Order = 1, IsRequired = false)]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity

        [DataMember(Order = 3, IsRequired = true)]
        public long BackendId { get; set; }

        [DataMember(Order = 4, IsRequired = false)]
        public Uri Schema { get; set; }

        #endregion

        [DataMember(Order = 5, IsRequired = true)]
        public string? Goal { get; set; } = string.Empty;

        [DataMember(Order = 6, IsRequired = false)]
        public string? FirstName { get; set; } = string.Empty;

        [DataMember(Order = 7, IsRequired = false)]
        public string? LastName { get; set; } = string.Empty;

        /// <summary>
        /// For Testung the value will be:
        /// - user1.png
        /// - user2.png
        /// - user3.png
        /// </summary>
        [DataMember(Order = 8, IsRequired = false)]
        public string? Image { get; set; } = string.Empty;
    }
}
