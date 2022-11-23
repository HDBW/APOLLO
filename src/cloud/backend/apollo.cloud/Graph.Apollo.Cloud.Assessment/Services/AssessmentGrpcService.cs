using System.Collections.ObjectModel;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Newtonsoft.Json;
using ProtoBuf;
using ProtoBuf.Grpc;

namespace Invite.Apollo.App.Graph.Assessment.Services;

public class AssessmentGrpcService : IAssessmentGRPCService
{
    private readonly ILogger<AssessmentGrpcService> _logger;
    private readonly IDataService _assessmentDataService;

    private UseCaseCollections _collections;


    public AssessmentGrpcService(ILogger<AssessmentGrpcService> logger, IDataService assessmentDataService)
    {
        _logger = logger;
        _assessmentDataService = assessmentDataService;
        _collections = new UseCaseCollections
        {
            AssessmentCategories = new Collection<AssessmentCategory>(_assessmentDataService.GetAllAssessmentCategoriesAsync().Result.ToList()),
            AssessmentItems = new Collection<AssessmentItem>(_assessmentDataService.GetAllAssessmentItemsAsync().Result.ToList()),
            AnswerItems = new Collection<AnswerItem>(_assessmentDataService.GetAllAnswerItemsAsync().Result.ToList()),
            AnswerMetaDataRelations = new Collection<AnswerMetaDataRelation>(_assessmentDataService.GetAllAnswerMetaDataRelationsAsync().Result.ToList()),
            MetaDataItems = new Collection<MetaDataItem>(_assessmentDataService.GetAllMetaDataItemsAsync().Result.ToList()),
            MetaDataMetaDataRelations = new Collection<MetaDataMetaDataRelation>(_assessmentDataService.GetAllMetaDataMetaDataRelationsAsync().Result.ToList()),
            QuestionItems = new Collection<QuestionItem>(_assessmentDataService.GetAllQuestionItemsAsync().Result.ToList()),
            QuestionMetaDataRelations = new Collection<QuestionMetaDataRelation>(_assessmentDataService.GetAllQuestionMetaDataRelationsAsync().Result.ToList()),
            //TODO: Courses
        };

        using (StreamWriter file = File.CreateText(@"devtest.json"))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, _collections);
        }

        string filename = "usecase1.bin";

        if (File.Exists(filename))
        {
            File.Delete(filename);
        }

        using (var file = File.Create(filename))
        {
            Serializer.Serialize(file, _collections);
            file.Close();
        }
    }


    public ValueTask<AssessmentResponse> GetAssessmentsAsync(AssessmentRequest request)
    {
        AssessmentResponse response = new AssessmentResponse
            {
                Assessments = _collections.AssessmentItems, CorrelationId = request.CorrelationId
            };
        return new ValueTask<AssessmentResponse>(response);
    }

    public async ValueTask<AnswerResponse> GetAnswersAsync(AnswersRequest request) => throw new NotImplementedException();

    public async ValueTask<AnswerResponse> GetAnswersAsync(AnswersRequest request, CallContext context = default) => throw new NotImplementedException();

    public async ValueTask<AnswerResponse> GetAnswersAsync(AnswersRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public async ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request) => throw new NotImplementedException();

    public async ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request, CallContext context = default) => throw new NotImplementedException();

    public async ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
