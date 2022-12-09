namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IMetaDataHasMetaDataDataService
    {
        public Task<IEnumerable<Models.MetaDataHasMetaData>> GetAllMetaDataHasMetaDataAsync();

        //public Task<IEnumerable<Models.MetaDataHasMetaData>> GetQuestionMetaDataRelationByQuestionIdAsync(long questionId);

        public void CreateMetaDataHasMetaData(Models.MetaDataHasMetaData metaDataHasMetaData);

        public void EditMetaDataHasMetaData(Models.MetaDataHasMetaData metaDataHasMetaData);

        public void DeleteMetaDataHasMetaData(Models.MetaDataHasMetaData metaDataHasMetaData);
    }
}
