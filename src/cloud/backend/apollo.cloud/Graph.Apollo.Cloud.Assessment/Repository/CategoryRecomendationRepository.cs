using Invite.Apollo.App.Graph.Assessment.Data;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public class CategoryRecomendationRepository : RepositoryBase<Models.CategoryRecomendation>, ICategoryRecomendationRepository
    {
        public CategoryRecomendationRepository(AssessmentContext context) : base(context)
        {
        }
    }

    public interface ICategoryRecomendationRepository : IRepository<Models.CategoryRecomendation>
    {
    }
}
