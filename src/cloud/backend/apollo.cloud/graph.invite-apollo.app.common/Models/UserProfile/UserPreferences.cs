using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    [DataContract]
    public class UserPreferences : BaseItem
    {

        /// <summary>
        /// Frontend Mapping
        /// </summary>
        [DataMember(Order = 5)]
        [ForeignKey(nameof(UserProfileItem))]
        public string UserId { get; set; }

        //TODO: Preferences ???
    }
}
