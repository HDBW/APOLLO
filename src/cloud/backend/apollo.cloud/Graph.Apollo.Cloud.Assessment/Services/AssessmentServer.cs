using Invite.Apollo.App.Graph.Common.Models.Assessment;
using ProtoBuf.Grpc;

namespace Invite.Apollo.App.Graph.Assessment.Services;

public class AssessmentService : IAssessmentService
{
    private readonly ILogger<AssessmentService> _logger;

    public AssessmentService(ILogger<AssessmentService> logger)
    {
        _logger = logger;
    }


    public ValueTask<AssessmentResponse> GetAssessmentsAsync(AssessmentRequest request) => throw new NotImplementedException();

    public ValueTask<AssessmentResponse> GetAssessmentsAsync(AssessmentRequest request, CallContext context = default) => throw new NotImplementedException();

    public ValueTask<AssessmentResponse> GetAssessmentsAsync(AssessmentRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public ValueTask<AnswerResponse> GetAnswersAsync(AssessmentRequest request) => throw new NotImplementedException();

    public ValueTask<AnswerResponse> GetAnswersAsync(AnswersRequest request, CallContext context = default) => throw new NotImplementedException();

    public ValueTask<AnswerResponse> GetAnswersAsync(AnswersRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request) => throw new NotImplementedException();

    public ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request, CallContext context = default) => throw new NotImplementedException();

    public ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
