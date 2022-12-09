using Invite.Apollo.App.Graph.Assessment.Data;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public class QuestionRepository : RepositoryBase<Models.Question>, IQuestionRepository
    {
        public QuestionRepository(AssessmentContext context) : base(context)
        {
        }
    }

    public interface IQuestionRepository : IRepository<Models.Question>
    {
    }
}
