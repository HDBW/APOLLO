using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface ICategoryDtoDataService
    {
        public Task<IEnumerable<AssessmentCategory>> GetAllAssessmentCategoriesAsync();

        public Task<AssessmentCategory> GetAssessmentCategoryByIdAsync(long categoryId);

        public void CreateAssessmentCategory(AssessmentCategory category);

        public void EditAssessmentCategory(AssessmentCategory category);

        public void DeleteAssessmentCategory(AssessmentCategory category);
    }
}
