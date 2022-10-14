using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Common.Models.Esco
{
    public class Skill : IEntity
    {
        public long Id { get; set; }
        public long Ticks { get; set; }

        public string EscoId { get; set; }

        public string Version { get; set; }

        [DataMember(Order = 5, IsRequired = true)]
        [ForeignKey(nameof(AssessmentItem))]
        public long AssessmentId { get; set; }
    }
}
