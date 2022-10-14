using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [ServiceContract(Name = "AssessmentService")]
    public interface IAssessmentService
    {
        [OperationContract]
        ValueTask<AssessmentResponse> GetAssessmentsAsync(AssessmentRequest request);

        [OperationContract]
        ValueTask<AssessmentResponse> GetAssessmentsAsync(AssessmentRequest request, CallContext context = default);

        [OperationContract]
        ValueTask<AssessmentResponse> GetAssessmentsAsync(AssessmentRequest request, CancellationToken cancellationToken = default);

        [OperationContract]
        ValueTask<AnswerResponse> GetAnswersAsync(AssessmentRequest request);

        [OperationContract]
        ValueTask<AnswerResponse> GetAnswersAsync(AnswersRequest request, CallContext context = default);

        [OperationContract]
        ValueTask<AnswerResponse> GetAnswersAsync(AnswersRequest request, CancellationToken cancellationToken = default);

        [OperationContract]
        ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request);

        [OperationContract]
        ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request, CallContext context = default);

        [OperationContract]
        ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request, CancellationToken cancellationToken = default);

    }
}
