using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{
    [DataContract]
    public class Occupation : BaseItem
    {

        [DataMember(Order = 5, IsRequired = false)]
        public CultureInfo CultureInfo { get; set; } = null!;

        [DataMember(Order = 6, IsRequired = true)]
        public string Name { get; set; } = null!;

        [DataMember(Order = 7, IsRequired = true)]
        public string Code { get; set; } = null!;

        [DataMember(Order = 8, IsRequired = true)]
        public string Description { get; set; }

        [DataMember(Order = 9, IsRequired = true)]
        public string Regulations { get; set; }

        [DataMember(Order = 10, IsRequired = true)]
        public string Verticals { get; set; }
    }
}
