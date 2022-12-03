namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface ICategoryRecomendationDataService
    {
        public Task<IEnumerable<Models.CategoryRecomendation>> GetAllCategoryRecomendationsAsync();

        public Task<Models.CategoryRecomendation> GetCategoryRecomendationByCategoryIdAsync(long categoryId);

        public Task<Models.CategoryRecomendation> GetCategoryRecomendationByIdAsync(long categoryRecomendationId);

        public void CreateCategoryRecomendation(Models.CategoryRecomendation categoryRecomendation);

        public void EditCategoryRecomendation(Models.CategoryRecomendation categoryRecomendation);

        public void DeleteCategoryRecomendation(Models.CategoryRecomendation categoryRecomendation);
    }
}
