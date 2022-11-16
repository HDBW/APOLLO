using Invite.Apollo.App.Graph.Assessment.Data;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public class AssessmentRepository : RepositoryBase<Models.Assessment>, IAssessmentRepository
    {
        public AssessmentRepository(AssessmentContext context) : base(context)
        {
        }
    }

    public interface IAssessmentRepository : IRepository<Models.Assessment>
    {
    }
}
