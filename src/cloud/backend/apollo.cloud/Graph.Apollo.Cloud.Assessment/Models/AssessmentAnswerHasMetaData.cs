namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class AssessmentAnswerHasMetaData
    {
        public long AnswerId { get; set; }
        public AssessmentAnswer AssessmentAnswer { get; set; }
        public long AssessmentMetaDataId { get; set; }
        public AssessmentMetaData AssessmentMetaData { get; set; }
        
        
    }
}
