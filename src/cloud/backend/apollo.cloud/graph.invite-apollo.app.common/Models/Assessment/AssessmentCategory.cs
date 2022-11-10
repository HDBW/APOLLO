using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class AssessmentCategory : IEntity, IBackendEntity
    {
        #region Implementation of IEntity

        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity

        [DataMember(Order = 3)]
        public long BackendId { get; set; }

        [DataMember(Order = 4)]
        public Uri Schema { get; set; }

        #endregion

        [DataMember(Order = 5)]
        public string Title { get; set; }

        [DataMember(Order = 6)]
        public string Value { get; set; }

        /// <summary>
        /// Threshold
        /// TODO: Remove before DataBase Creation
        /// </summary>
        [DataMember(Order = 7)]
        public int ResultLimits { get; set; }
    }
}
