using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Common.Models.Esco
{
    [DataContract]
    public class Occupation : IEntity
    {
        public long Id { get; set; }
        public long Ticks { get; set; }

        public string EscoId { get; set; }

        public string Version { get; set; }

        //TODO: Implement Assessment Relation
    }
}
