
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Apollo.Cloud.Common.Models.Taxonomy
{
    [DataContract]
    public class Skill
    {
        [DataMember(Order = 1)]
        public string Schema { get; set; }
    }
}
