
using System.Runtime.Serialization;

namespace Graph.Apollo.Cloud.Common.Models.Taxonomy
{
    [DataContract]
    public class Occupation
    {
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public string Schema { get; set; }
    }
}
