using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    /// <summary>
    /// TESTING PURPOSE ONLY
    /// USECASES FOR PROTOTYPE TESTING
    /// </summary>
    [DataContract]
    public class AssessmentUseCases
    {
        public AssessmentUseCases(Collection<AssessmentItem> assessmentItems, Collection<QuestionItem> questionItems, Collection<AnswerItem> answerItems, Collection<MetaDataItem> metaDataItems, Collection<QuestionMetaDataRelation> questionMetaDataRelations, Collection<AnswerMetaDataRelation> answerMetaDataRelations, Collection<MetaDataMetaDataRelation> metaDataMetaDataRelations)
        {
            AssessmentItems = assessmentItems;
            QuestionItems = questionItems;
            AnswerItems = answerItems;
            MetaDataItems = metaDataItems;
            QuestionMetaDataRelations = questionMetaDataRelations;
            AnswerMetaDataRelations = answerMetaDataRelations;
            MetaDataMetaDataRelations = metaDataMetaDataRelations;
        }

        public AssessmentUseCases()
        {
            AssessmentItems = new Collection<AssessmentItem>();
            QuestionItems = new Collection<QuestionItem>();
            AnswerItems = new Collection<AnswerItem>();
            MetaDataItems = new Collection<MetaDataItem>();
            QuestionMetaDataRelations = new Collection<QuestionMetaDataRelation>();
            AnswerMetaDataRelations = new Collection<AnswerMetaDataRelation>();
            MetaDataMetaDataRelations = new Collection<MetaDataMetaDataRelation>();
        }

        [DataMember(Order = 1,IsRequired = false)]        
        public Collection<AssessmentItem> AssessmentItems { get; set; }

        [DataMember(Order = 2, IsRequired = false)]
        public Collection<QuestionItem> QuestionItems { get; set; }

        [DataMember(Order = 3, IsRequired = false)]
        public Collection<AnswerItem> AnswerItems { get; set; }

        [DataMember(Order = 4, IsRequired = false)]
        public Collection<MetaDataItem> MetaDataItems { get; set; }

        [DataMember(Order = 5, IsRequired = false)]
        public Collection<QuestionMetaDataRelation> QuestionMetaDataRelations { get; set; }

        [DataMember(Order = 6, IsRequired = false)]
        public Collection<AnswerMetaDataRelation> AnswerMetaDataRelations { get; set; }

        [DataMember(Order = 7, IsRequired = false)]
        public Collection<MetaDataMetaDataRelation> MetaDataMetaDataRelations { get; set; }
    }
}
