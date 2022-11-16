using Invite.Apollo.App.Graph.Assessment.Data;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public class AssetRepository : RepositoryBase<Models.Asset>, IAssetRepository
    {
        public AssetRepository(AssessmentContext context) : base(context)
        {
        }
    }

    public interface IAssetRepository : IRepository<Models.Asset>
    {
    }
}
