using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    [DataContract]
    public class AssessmentScore : IEntity, IBackendEntity
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

        [DataMember(Order = 4, IsRequired = true)]
        public Uri Schema { get; set; }

        #endregion

        /// <summary>
        /// Represents the ssid of the user
        /// </summary>
        [DataMember(Order = 5, IsRequired = true)]
        public string UserId { get; set; }

        /// <summary>
        /// Represents the ClientId
        /// </summary>
        [DataMember(Order = 6, IsRequired = false)]
        public long AssessmentId { get; set; }

        /// <summary>
        /// Represents the Assessment BackendId
        /// </summary>
        [DataMember(Order = 7, IsRequired = true)]
        public long AssessmentBackendId { get; set; }

        [DataMember(Order = 8, IsRequired = true)]
        public string ScoreOccupation { get; set; }

        [DataMember(Order = 9, IsRequired = true)]
        public string ScoreSkills { get; set; }
    }
}
