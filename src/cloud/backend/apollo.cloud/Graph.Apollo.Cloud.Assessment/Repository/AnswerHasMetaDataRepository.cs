using Invite.Apollo.App.Graph.Assessment.Data;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public class AnswerHasMetaDataRepository : RepositoryBase<Models.AnswerHasMetaData>, IAnswerHasMetaDataRepository
    {
        public AnswerHasMetaDataRepository(AssessmentContext context) : base(context)
        {
        }
    }

    public interface IAnswerHasMetaDataRepository : IRepository<Models.AnswerHasMetaData>
    {
    }
}
