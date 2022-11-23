namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IDataService :
        IAssessmentDataService,
        IAssessmentDtoDataService,
        IQuestionDataService,
        IQuestionDtoDataService,
        IAnswerDataService,
        IAnswerDtoDataService,
        IMetaDataDataService,
        IMetaDataDtoDataService,
        IAnswerHasMetaDataDataService,
        IAnswerHasMetaDataDtoDataService,
        IQuestionHasMetaDataDataService,
        IQuestionHasMetaDataDtoDataService,
        IMetaDataHasMetaDataDataService,
        IMetaDataHasMetaDataDtoDataService,
        ICategoryDataService,
        ICategoryDtoDataService
    {
    }
}
