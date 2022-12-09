namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IAnswerHasMetaDataDataService
    {
        public Task<IEnumerable<Models.AnswerHasMetaData>> GetAllAnswerHasMetaDataAsync();

        public Task<IEnumerable<Models.AnswerHasMetaData>> GetAnswerHasMetaDataByAnswerIdAsync(long answerId);

        public void CreateAnswerHasMetaData(Models.AnswerHasMetaData answerHasMetaData);

        public void EditAnswerHasMetaData(Models.AnswerHasMetaData answerHasMetaData);

        public void DeleteAnswerHasMetaData(Models.AnswerHasMetaData answerHasMetaData);
    }
}
