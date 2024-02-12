// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{
    [DataContract]
    public class OccupationMetaDataRelation : BaseItem
    {

        [ForeignKey(nameof(Occupation))]
        public long OccupationId { get; set; }

        [ForeignKey(nameof(Models.MetaDataItem))]
        public long MetaDataItemId { get; set; }
    }
}
