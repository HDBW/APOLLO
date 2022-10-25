using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using Invite.Apollo.App.Graph.Common.Models.Esco;

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{
    [DataContract]
    public class OccupationHasSkill : IEntity,IBackendEntity
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

        [ForeignKey(nameof(Skill))]
        public long SkillId { get; set; }
    }
}
