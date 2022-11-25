using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IAssessmentDtoDataService
    {
        public Task<IEnumerable<AssessmentItem>> GetAllAssessmentItemsAsync();

        public Task<AssessmentItem> GetAssessmentItemByIdAsync(long assessmentId);

        public Task<IEnumerable<AssessmentItem>> GetAssessmentItemsByType(AssessmentType type);

        public Task<IEnumerable<AssessmentItem>> GetAssessmentsByOccupation(string occupation);

        public void CreateAssessmentItem(AssessmentItem assessment);

        public void EditAssessmentItemAsync(AssessmentItem assessment);

        public void DeleteAssessmentItemAsync(AssessmentItem assessment);
    }
}
