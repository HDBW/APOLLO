using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Graph.Apollo.Cloud.Common.Models;

[DataContract]
public partial class Assessment : IEntity
{
    public Assessment()
    {
        Title = string.Empty;
    }

    [DataMember(Order = 1)]
    [Key]
    public long Id { get; set; }

    [DataMember(Order = 2)]
    public string Title { get; set; }
}

[DataContract]
public partial class AssessmentRequest
{

    [DataMember(Order = 1)]
    public string? Title { get; set; }
}

[DataContract]
public partial class AssessmentResult
{
    [DataMember(Order = 1)]
    public List<Assessment> Assessments { get; set; }
}
