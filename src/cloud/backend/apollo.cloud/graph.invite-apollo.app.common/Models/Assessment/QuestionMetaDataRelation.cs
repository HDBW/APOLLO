// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class QuestionMetaDataRelation : BaseItem
    {

        [DataMember(Order = 5)]
        [ForeignKey(nameof(QuestionItem))]
        public long QuestionId { get; set; }

        [DataMember(Order = 6)]
        [ForeignKey(nameof(MetaDataItem))]
        public long MetaDataId { get; set; }
    }
}
