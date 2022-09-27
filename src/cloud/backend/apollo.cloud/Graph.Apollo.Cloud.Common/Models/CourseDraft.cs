using System.Runtime.Serialization;
using System.ServiceModel;
using ProtoBuf.Grpc;

namespace Graph.Apollo.Cloud.Common.Models
{
    /// <summary>
    /// Course Draft is the internal Class used for managing courses in the backend.
    /// This is the data exchange format for internal course management as well as exchange with the DAC repository
    /// </summary>
    [DataContract]
    public class CourseDraft : Course
    {
        /// <summary>
        /// Segment Metadata
        /// </summary>

        [DataMember(Order = 2)]
        public string? ExternalId { get; set; }

        [DataMember(Order = 3)]
        public Uri ExternalUrl { get; set; }

        /// <summary>
        /// Segment Content
        /// </summary>
        [DataMember()]
        public string ExternalDescription { get; set; }

        [DataMember()]
        public List<Uri> InternaDocuments { get; set; }

        [DataMember()]
        public List<Uri> Documents => InternaDocuments.Concat(PublicDocuments).ToList();

        /// <summary>
        /// Segment Content Management
        /// </summary>
        [DataMember]
        public DateTime LatestUpdate { get; set; }
        [DataMember]
        public DateTime Created { get; set; }


        
    }

    [DataContract]
    public class CourseDraftRequest
    {
        [DataMember(Order = 1)]
        public string Title { get; set; }
    }

    [DataContract]
    public class CourseDraftReply
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }
    }

    [ServiceContract]
    public interface ICourseDraftService
    {
        [OperationContract]
        Task<CourseDraftReply> CreateCourseDraftAsync(CourseDraftRequest request, CallContext context = default);
    }
}
