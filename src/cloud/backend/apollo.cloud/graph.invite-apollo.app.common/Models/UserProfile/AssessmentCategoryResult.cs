using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    [DataContract]
    [ProtoContract]
    public class AssessmentCategoryResult : BaseItem
    {

        [ForeignKey(nameof(UserProfile))]
        [DataMember(Order = 5)]
        public long UserProfileId { get; set; }

        [DataMember(Order = 6)]
        [ForeignKey(nameof(AssessmentCategory))]
        public long CategoryId { get; set; }

        //The result a user scored in a Category
        [DataMember(Order = 7)]
        public long Result { get; set; }


    }
}
