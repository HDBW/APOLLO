using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IUseCase
    {
        public IEnumerable<Models.Assessment> GetAssessmentsByUseCase(int useCase);
        public IEnumerable<Models.Answer> GetAnswersByUseCase(int useCase);
        public IEnumerable<Models.Question> GetQuestionsByUseCase(int useCase);

        public IEnumerable<Models.Category> GetCategoryByUseCase(int useCase);
        public IEnumerable<Models.CategoryRecomendation> GetCategoryRecomendationsByUseCase(int useCase);

        
        public IEnumerable<Models.MetaData> GetMetaDataByUseCase(int useCase);
        public IEnumerable<Models.MetaDataHasMetaData> GetMetaDataHasMetaDataByUseCase(int useCase);

        public IEnumerable<Models.QuestionHasMetaData> GetQuestionHasMetaDataByUseCase(int useCase);
        public IEnumerable<Models.AnswerHasMetaData> GetAnswerHasMetaDataByUseCase(int useCase);



        //DTO
        public IEnumerable<AssessmentItem> GetAssessmentItemsByUseCase(int useCase);
        public IEnumerable<AnswerItem> GetAnswersItemsByUseCase(int useCase);
        public IEnumerable<QuestionItem> GetQuestionItemsByUseCase(int useCase);

        public IEnumerable<AssessmentCategory> GetAssessmentCategoriesByUseCase(int useCase);
        public IEnumerable<CategoryRecomendationItem> GetCategoryRecomendationItemsByUseCase(int useCase);


        public IEnumerable<MetaDataItem> GetMetaDataItemsByUseCase(int useCase);
        public IEnumerable<MetaDataMetaDataRelation> GetMetaDataMetaDataRelationsByUseCase(int useCase);

        public IEnumerable<QuestionMetaDataRelation> GetQuestionMetaDataRelationByUseCase(int useCase);
        public IEnumerable<AnswerMetaDataRelation> GetAnswerMetaDataRelationByUseCase(int useCase);



    }
}
