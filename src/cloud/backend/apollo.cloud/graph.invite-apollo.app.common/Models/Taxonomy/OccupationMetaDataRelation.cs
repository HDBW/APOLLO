using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{
    [DataContract]
    public class OccupationMetaDataRelation : IEntity, IBackendEntity
    {
        #region Implementation of IEntity

        [Key]
        public long Id { get; set; }
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity

        public long BackendId { get; set; }
        public Uri Schema { get; set; } = null!;

        #endregion

        [ForeignKey(nameof(Occupation))]
        public long OccupationId { get; set; }

        [ForeignKey(nameof(Models.MetaDataItem))]
        public long MetaDataItemId { get; set; }
    }
}
