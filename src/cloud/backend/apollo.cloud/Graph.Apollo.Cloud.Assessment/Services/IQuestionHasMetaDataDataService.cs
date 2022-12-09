namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IQuestionHasMetaDataDataService
    {
        public Task<IEnumerable<Models.QuestionHasMetaData>> GetAllQuestionHasMetaDataAsync();

        public Task<IEnumerable<Models.QuestionHasMetaData>> GetQuestionHasMetaDataByQuestionIdAsync(long questionId);

        public void CreateQuestionHasMetaData(Models.QuestionHasMetaData questionHasMeta);

        public void EditQuestionHasMetaData(Models.QuestionHasMetaData questionHasMeta);

        public void DeleteQuestionHasMetaData(Models.QuestionHasMetaData questionHasMeta);
    }
}
