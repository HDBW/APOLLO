using System.Collections.ObjectModel;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public interface IAssessmentRepository
    {
        public Collection<AssessmentItem> Assessments { get; set; }
        public Collection<QuestionItem> Questions { get; set; }

        public Collection<AnswerItem> Answers { get; set; }

        public Collection<MetaDataItem> MetaData { get; set; }

        public Collection<MetaDataMetaDataRelation> MetaDataMetaDataRelations { get; set; }

        public Collection<QuestionMetaDataRelation> QuestionMetaDataRelations { get; set; }

        public Collection<AnswerMetaDataRelation> AnswerMetaDataRelations { get; set; }

        public Collection<AssessmentScoringItem> AssessmentScoring { get; set; }



        public Task<Collection<AssessmentItem>> GetAssessments();

        public Task<Collection<AssessmentItem>> GetAssessmentsByOccupation();

    }
}
