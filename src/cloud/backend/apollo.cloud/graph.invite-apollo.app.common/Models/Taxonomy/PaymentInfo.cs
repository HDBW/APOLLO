using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;
using System.Text;

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{
    public class PaymentInfo : IEntity, IBackendEntity
    {
        public string Currency { get; set; } = string.Empty;

        public decimal Minimal { get; set; }

        public decimal Median { get; set; }

        public decimal Maximum { get; set; }

        [ForeignKey(nameof(Occupation))]
        public long OccupationId { get; set; }

        #region Implementation of IEntity

        [Key]
        public long Id { get; set; }
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity

        public long BackendId { get; set; }
        public Uri Schema { get; set; }

        #endregion
    }
}
