using Invite.Apollo.App.Graph.Assessment.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface ICategoryRecomendationDtoDataService
    {
        public Task<IEnumerable<CategoryRecomendationItem>> GetAllCategoryRecomendationItemAsync();

        public Task<CategoryRecomendationItem> GetCategoryRecomendationItemByIdAsync(long categoryRecomendationId);

        public void CreateCategoryRecomendationItem(CategoryRecomendationItem categoryRecomendationItem);

        public void EditCategoryRecomendationItem(CategoryRecomendationItem categoryRecomendationItem);

        public void DeleteCategoryRecomendationItem(CategoryRecomendationItem categoryRecomendationItem);
    }
}
