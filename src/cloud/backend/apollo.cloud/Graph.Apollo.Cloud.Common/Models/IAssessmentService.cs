using System.ServiceModel;


namespace Graph.Apollo.Cloud.Common.Models
{
    [ServiceContract(Name = "Assessment.AssessmentService")]
    public interface IAssessmentService
    {
        [OperationContract]
        [return: MessageParameter(Name = "AssessmentResult")]
        ValueTask<AssessmentResult> GetAssessmentsAsync(AssessmentRequest request);
    }
}
