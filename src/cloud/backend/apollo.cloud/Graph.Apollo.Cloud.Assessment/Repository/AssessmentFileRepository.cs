using System.Collections.ObjectModel;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Microsoft.VisualBasic.FileIO;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public class AssessmentFileRepository : IAssessmentRepository
    {
        #region Implementation of IAssessmentRepository

        public Collection<AssessmentItem> Assessments { get; set; }
        public Collection<QuestionItem> Questions { get; set; }
        public Collection<AnswerItem> Answers { get; set; }
        public Collection<MetaDataItem> MetaData { get; set; }
        public Collection<MetaDataMetaDataRelation> MetaDataMetaDataRelations { get; set; }
        public Collection<QuestionMetaDataRelation> QuestionMetaDataRelations { get; set; }
        public Collection<AnswerMetaDataRelation> AnswerMetaDataRelations { get; set; }
        public Collection<AssessmentScoringItem> AssessmentScoring { get; set; }

        public AssessmentFileRepository()
        {
            var path = "Assessments.csv";

            using (TextFieldParser csvFieldParser = new(path, System.Text.Encoding.Default))
            {
                csvFieldParser.CommentTokens = new[] { "#" };
                csvFieldParser.SetDelimiters(new[] { ";" });
                csvFieldParser.HasFieldsEnclosedInQuotes = true;

                csvFieldParser.ReadLine();

                while (Equals(!csvFieldParser.EndOfData))
                {
                    string[] fields = csvFieldParser.ReadFields();
                    //TODO: Implement Assessment
                    //string Name = fields[0];
                    //string Address = fields[1];



                }

            }
        }

        public Task<Collection<AssessmentItem>> GetAssessments() => throw new NotImplementedException();

        public Task<Collection<AssessmentItem>> GetAssessmentsByOccupation() => throw new NotImplementedException();

        #endregion
    }
}
