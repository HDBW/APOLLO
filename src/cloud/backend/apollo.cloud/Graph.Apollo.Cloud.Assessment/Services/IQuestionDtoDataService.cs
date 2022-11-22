using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IQuestionDtoDataService
    {
        public Task<IEnumerable<QuestionItem>> GetAllQuestionItemsAsync();

        public Task<QuestionItem> GetQuestionItemByIdAsync(long assessmentId);

        public void CreateQuestionItem(QuestionItem question);

        public void EditQuestionItemAsync(QuestionItem question);

        public void DeleteQuestionItemAsync(QuestionItem question);
    }
}
