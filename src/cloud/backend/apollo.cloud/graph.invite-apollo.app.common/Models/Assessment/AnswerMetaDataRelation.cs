// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class AnswerMetaDataRelation : BaseItem
    {

        [DataMember(Order = 5)]
        [ForeignKey(nameof(AnswerItem))]
        public long AnswerId { get; set; }

        [ForeignKey(nameof(MetaDataItem))]
        [DataMember(Order = 6)]
        public long MetaDataId { get; set; }



    }
}
