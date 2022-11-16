using Invite.Apollo.App.Graph.Assessment.Data;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public class QuestionHasMetaDataRepository : RepositoryBase<Models.QuestionHasMetaData>, IQuestionHasMetaDataRepository
    {
        public QuestionHasMetaDataRepository(AssessmentContext context) : base(context)
        {
        }
    }

    public interface IQuestionHasMetaDataRepository : IRepository<Models.QuestionHasMetaData>
    {
    }
}
