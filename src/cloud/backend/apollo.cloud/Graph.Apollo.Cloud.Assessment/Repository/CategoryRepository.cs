using Invite.Apollo.App.Graph.Assessment.Data;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public class CategoryRepository : RepositoryBase<Models.Category>, ICategoryRepository
    {
        public CategoryRepository(AssessmentContext context) : base(context)
        {
        }
    }

    public interface ICategoryRepository : IRepository<Models.Category>
    {
    }
}
