using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    [DataContract]
    public class AssessmentCategoryResult : IEntity, IBackendEntity
    {
        #region Implementation of IEntity

        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [Key]
        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity

        [DataMember(Order = 3)]
        public long BackendId { get; set; }

        [DataMember(Order = 4)]
        public Uri Schema { get; set; }

        #endregion

        [ForeignKey(nameof(UserProfile))]
        [DataMember(Order = 5)]
        public long UserProfileId { get; set; }

        [DataMember(Order = 6)]
        [ForeignKey(nameof(AssessmentCategory))]
        public long Category { get; set; }

        //The result a user scored in a Category
        [DataMember(Order = 7)]
        public long Result { get; set; }
    }
}
