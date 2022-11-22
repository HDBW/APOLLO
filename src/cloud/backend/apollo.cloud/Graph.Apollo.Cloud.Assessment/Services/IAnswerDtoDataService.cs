using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IAnswerDtoDataService
    {
        public Task<IEnumerable<AnswerItem>> GetAllAnswerItemsAsync();

        public Task<AnswerItem> GetAnswerItemByIdAsync(long question);

        public void CreateAnswerItem(AnswerItem answer);

        public void EditAnswerItemAsync(AnswerItem answer);

        public void DeleteAnswerItemAsync(AnswerItem answer);
    }
}
