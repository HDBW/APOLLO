using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class AnswerMetaDataRelation : IEntity, IBackendEntity
    {
        #region Implementation of IEntity
        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2,IsRequired = true)]
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity
        [DataMember(Order = 3,IsRequired = true)]
        public long BackendId { get; set; }

        [DataMember(Order = 4,IsRequired = true)]
        public Uri Schema { get; set; } = null!;

        #endregion

        [DataMember(Order = 3)]
        [ForeignKey(nameof(AnswerItem))]
        public long AnswerId { get; set; }

        [ForeignKey(nameof(MetaDataItem))]
        [DataMember(Order = 4)]
        public long MetaDataId { get; set; }



    }
}
