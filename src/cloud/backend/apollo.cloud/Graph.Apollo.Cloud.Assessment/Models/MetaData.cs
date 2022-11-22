using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class MetaData : BaseItem
    {
       
        public MetaDataType Type { get; set; }

        //Do we need "value" - what are we going to save here? 
        public string Value { get; set; }

        public Asset? Asset { get; set; }

        public long? AssetId { get; set; }

        public List<AnswerHasMetaData> AnswerHasMetaDatas { get; set; }

        public List<QuestionHasMetaData> QuestionHasMetaDatas { get; set; }

        public List<MetaDataHasMetaData> SourceQuestionHasMetaDatas { get; set; }

        public List<MetaDataHasMetaData> TargetMetaDataHasMetaDatas { get; set; }
       
    }
}
