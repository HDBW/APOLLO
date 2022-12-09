using Invite.Apollo.App.Graph.Assessment.Models;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IAnswerDataService
    {
        public Task<IEnumerable<Models.Answer>> GetAllAnswersAsync();

        public Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(long questionId);

        public void CreateAnswer(Models.Answer answer);

        public void EditAnswerAsync(Models.Answer answer);

        public void DeleteAnswerAsync(Models.Answer answer);
    }
}
