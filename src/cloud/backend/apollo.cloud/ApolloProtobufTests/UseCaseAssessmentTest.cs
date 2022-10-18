using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using NUnit.Framework;

namespace Invite.Apollo.App.Graph.Common.Test
{
    [TestFixture]
    public class UseCaseAssessmentTest
    {
        private List<QuestionItem> _questions = new();
        private List<AnswerItem> _answers = new();
        private List<MetaDataItem> _metaData = new();
        private List<QuestionMetaDataRelation> _questionMetaDataRelations = new();
        private List<AnswerMetaDataRelation> _answerMetaDataRelations = new();
        private List<MetaDataMetaDataRelation> _metaDataMetaDataRelations = new();
        private List<MetaDataItem> _answersMetaData = new();
        private List<MetaDataItem> _questionMetaData = new();
        private List<AssessmentItem> _assessments = new();


        /// <summary>
        /// Priority for this test case is to have several deep structured test
        /// See Issue #55 priority table.
        /// </summary>
        [Test]
        public void ShouldGenerateUseCaseAssessments()
        {

        }
    }
}
