namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IQuestionDataService
    {
        public Task<IEnumerable<Models.Question>> GetAllQuestionsAsync();

        public Task<Models.Question> GetQuestionByIdAsync(long assessmentId);

        public void CreateQuestion(Models.Question question);

        public void EditQuestionAsync(Models.Question question);

        public void DeleteQuestionAsync(Models.Question question);
    }
}
