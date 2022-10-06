using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Graph.Apollo.Cloud.Common.Models;

//TODO: https://stackoverflow.com/questions/71420086/protobuf-net-grpc-serializing-interfaces

public interface IEntity
{
    [Key]
    public long Id { get; set; }

}
