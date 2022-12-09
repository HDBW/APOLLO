using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IMetaDataHasMetaDataDtoDataService
    {
        public Task<IEnumerable<MetaDataMetaDataRelation>> GetAllMetaDataMetaDataRelationsAsync();

        //public Task<IEnumerable<Models.MetaDataHasMetaData>> GetQuestionMetaDataRelationByQuestionIdAsync(long questionId);

        public void CreateMetaDataMetaDataRelation(MetaDataMetaDataRelation metaDataHasMetaData);

        public void EditMetaDataMetaDataRelation(MetaDataMetaDataRelation metaDataHasMetaData);

        public void DeleteMetaDataMetaDataRelation(MetaDataMetaDataRelation metaDataHasMetaData);
    }
}
