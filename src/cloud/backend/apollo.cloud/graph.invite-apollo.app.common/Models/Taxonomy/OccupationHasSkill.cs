﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Esco;

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{
    [DataContract]
    public class OccupationHasSkill : BaseItem
    {
        [ForeignKey(nameof(Occupation))]
        public long OccupationId { get; set; }

        [ForeignKey(nameof(Skill))]
        public long SkillId { get; set; }
    }
}
