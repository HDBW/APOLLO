using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ProtoBuf.Grpc;

namespace Graph.Apollo.Cloud.Common.Models;

[DataContract]
public class CoursePrice
{
    [DataMember(Order = 1)]
    public decimal Price { get; set; }
    [DataMember(Order=2)]
    public DateTime StartDate { get; set; }
    [DataMember(Order = 3)]
    public DateTime? EndDate { get; set; }
}

