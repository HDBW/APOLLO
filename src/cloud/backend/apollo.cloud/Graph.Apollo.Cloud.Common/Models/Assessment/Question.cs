
using System.Runtime.Serialization;

namespace Graph.Apollo.Cloud.Common.Models.Assessment
{
    [DataContract]
    public class Question
    {
        [DataMember(Order = 1)]
        public QuestionItem Value { get; set; }

        [DataMember(Order = 2)]
        public List<QuestionMetaDataRelation> QuestionMetaDataRelations { get; set; }

        [DataMember(Order = 3)]
        public List<MetaData> QuestionsMetaDatas { get; set; }

        [DataMember(Order = 4)]
        public List<AnswerItem> Answers { get; set; }

        [DataMember(Order = 5)]
        public List<MetaData> AnswerMetaDatas { get; set; }

        [DataMember(Order = 6)]
        public List<AnswerMetaDataRelation> AnswerMetaDataRelations { get; set; }

        [DataMember(Order = 7)]
        public List<MetaDataMetaDataRelation> MetaDataMetaDataRelations { get; set; }
    }
}
