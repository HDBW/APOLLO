using Invite.Apollo.App.Graph.Common.Models;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IMetaDataDtoDataService
    {
        public Task<IEnumerable<MetaDataItem>> GetAllMetaDataItemsAsync();

        public Task<MetaDataItem> GetMetaDataItemByIdAsync(long metadataId);

        public void CreateMetaDataItem(MetaDataItem metaData);

        public void EditMetaDataItemAsync(MetaDataItem metaData);

        public void DeleteMetaDataItemAsync(MetaDataItem metaData);
    }
}
