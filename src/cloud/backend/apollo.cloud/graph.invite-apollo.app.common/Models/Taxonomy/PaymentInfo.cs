// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{
    public class PaymentInfo : BaseItem
    {
        public string Currency { get; set; } = string.Empty;

        public decimal Minimal { get; set; }

        public decimal Median { get; set; }

        public decimal Maximum { get; set; }

        [ForeignKey(nameof(Occupation))]
        public long OccupationId { get; set; }

    }
}
