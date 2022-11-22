namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IMetaDataDataService
    {
        public Task<IEnumerable<Models.MetaData>> GetAllMetaDataAsync();

        public Task<Models.MetaData> GetMetaDataByIdAsync(long metadataId);

        public void CreateMetaData(Models.MetaData metaData);

        public void EditMetaDataAsync(Models.MetaData metaData);

        public void DeleteMetaDataAsync(Models.MetaData metaData);
    }
}
