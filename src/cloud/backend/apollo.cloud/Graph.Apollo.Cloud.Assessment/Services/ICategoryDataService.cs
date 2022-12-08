namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface ICategoryDataService
    {
        public Task<IEnumerable<Models.Category>> GetAllCategoriesAsync();

        public Task<Models.Category> GetCategoryByIdAsync(long categoryId);

        public void CreateCategory(Models.Category category);

        public void EditCategory(Models.Category category);

        public void DeleteCategory(Models.Category category);
    }
}
