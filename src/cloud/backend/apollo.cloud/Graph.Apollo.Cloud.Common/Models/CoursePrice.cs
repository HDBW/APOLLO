using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ServiceModel;
using Google.Protobuf.WellKnownTypes;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace Graph.Apollo.Cloud.Common.Models;

[DataContract]
public class CoursePrice
{
    public CoursePrice()
    {
        Id = new Guid().ToString();
        Price = new decimal();
        StartDate = DateTime.Now;
        EndDate = null;
    }

    [DataMember(Order = 1)]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    [DataMember(Order = 2)]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    [DataMember(Order=3)]
    [DataType(DataType.Date)]
    [Required]
    public DateTime StartDate { get; set; }
    [DataMember(Order = 4)]
    public DateTime? EndDate { get; set; }
}

public class CoursePriceReply
{
    public CoursePriceReply()
    {
        CoursePrices = new List<CoursePrice>();
    }

    [DataMember(Order = 1)]
    public List<CoursePrice> CoursePrices { get; set; }
}

[DataContract]
public class CoursePriceRequest
{
    public TimeSpan? RequestedTimespan { get; set; }
}

[ServiceContract]
[Service("CoursePriceService")]
public interface IRetrieveCoursePricesService : ICoursePriceService
{
    [OperationContract]
    Task<CoursePriceReply> GetCoursePricesAsync(Empty empty,CallContext context = default);
    [OperationContract]
    Task<CoursePriceReply> GetCoursePricesByCourseIdAsync(CoursePriceRequest request, CallContext context = default);
}

[SubService]
[ServiceContract]
public interface ICoursePriceService
{
    Task<CoursePriceReply> GetCoursePricesAsync(CoursePriceCreationRequest request, CallContext context = default);
}

[DataContract]
public class CoursePriceCreationRequest
{
    
}



