using Invite.Apollo.App.Graph.Assessment.Data;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public class MetaDataHasMetaDataRepository : RepositoryBase<Models.MetaDataHasMetaData>, IMetaDataHasMetaDataRepository
    {
        public MetaDataHasMetaDataRepository(AssessmentContext context) : base(context)
        {
        }
    }

    public interface IMetaDataHasMetaDataRepository : IRepository<Models.MetaDataHasMetaData>
    {
    }
}
