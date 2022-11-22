using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IAnswerHasMetaDataDtoDataService
    {
        public Task<IEnumerable<AnswerMetaDataRelation>> GetAllAnswerMetaDataRelationsAsync();

        public Task<IEnumerable<AnswerMetaDataRelation>> GetAnswerMetaDataRelationByAnswerIdAsync(long answerId);

        public void CreateAnswerMetaDataRelation(AnswerMetaDataRelation answerMetaDataRelation);

        public void EditAnswerMetaDataRelation(AnswerMetaDataRelation answerMetaDataRelation);

        public void DeleteAnswerMetaDataRelation(AnswerMetaDataRelation answerMetaDataRelation);
    }
}
