using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IQuestionHasMetaDataDtoDataService
    {
        public Task<IEnumerable<QuestionMetaDataRelation>> GetAllQuestionMetaDataRelationsAsync();

        public Task<IEnumerable<QuestionMetaDataRelation>> GetQuestionMetaDataRelationByQuestionIdAsync(long questionId);

        public void CreateQuestionMetaDataRelation(QuestionMetaDataRelation questionMetaDataRelation);

        public void EditQuestionMetaDataRelation(QuestionMetaDataRelation questionMetaDataRelation);

        public void DeleteQuestionMetaDataRelation(QuestionMetaDataRelation questionMetaDataRelation);
    }
}
