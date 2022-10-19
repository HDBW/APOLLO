using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class QuestionMetaDataRelation : IEntity, IBackendEntity
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

        [DataMember(Order = 5)]
        [ForeignKey(nameof(QuestionItem))]
        public long QuestionId { get; set; }

        [DataMember(Order = 6)]
        [ForeignKey(nameof(MetaDataItem))]
        public long MetaDataId { get; set; }
    }
}
