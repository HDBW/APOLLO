using Invite.Apollo.App.Graph.Assessment.Data;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public class AnswerRepository : RepositoryBase<Models.Answer>, IAnswerRepository
    {
        public AnswerRepository(AssessmentContext context) : base(context)
        {
        }
    }

    public interface IAnswerRepository : IRepository<Models.Answer>
    {
    }
}
