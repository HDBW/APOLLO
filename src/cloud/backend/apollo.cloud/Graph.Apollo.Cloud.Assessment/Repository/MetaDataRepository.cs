using Invite.Apollo.App.Graph.Assessment.Data;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public class MetaDataRepository : RepositoryBase<Models.MetaData>, IMetaDataRepository
    {
        public MetaDataRepository(AssessmentContext context) : base(context)
        {
        }
    }

    public interface IMetaDataRepository : IRepository<Models.MetaData>
    {
    }
}
