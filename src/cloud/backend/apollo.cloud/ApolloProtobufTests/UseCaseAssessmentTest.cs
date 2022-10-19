using System.Reflection;
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

        [Test]
        public void ShouldGenerateAssessmentWarehouseOperator()
        {
            List<QuestionItem> questions = new();
            List<AnswerItem> answers = new();
            List<MetaDataItem> metaData = new();
            List<QuestionMetaDataRelation> questionMetaDataRelations = new();
            List<AnswerMetaDataRelation> answerMetaDataRelations = new();
            List<MetaDataMetaDataRelation> metaDataMetaDataRelations = new();
            List<MetaDataItem> answersMetaDataItems = new();
            List<MetaDataItem> questionMetaDataItems = new();

            long assessmentBackendId = 24323;
            AssessmentItem assessment = CreateAssessmentItem(out long assessmentId, assessmentBackendId, new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"), "Fachlagerist:innen");



            
        }

        [Test]
        public void ShouldGenerateAssessmentECommerceDude()
        {
            List<QuestionItem> questions = new();
            List<AnswerItem> answers = new();
            List<MetaDataItem> metaData = new();
            List<QuestionMetaDataRelation> questionMetaDataRelations = new();
            List<AnswerMetaDataRelation> answerMetaDataRelations = new();
            List<MetaDataMetaDataRelation> metaDataMetaDataRelations = new();
            List<MetaDataItem> answersMetaDataItems = new();
            List<MetaDataItem> questionMetaDataItems = new();

            long assessmentBackendId = 61282;
            AssessmentItem assessment = CreateAssessmentItem(out long assessmentId, assessmentBackendId,new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"), "Kaufmann/-frau - E-Commerce");


        }

        [Test]
        public void ShouldGenerateAssessmentDigitalSkills()
        {
            List<QuestionItem> questions = new();
            List<AnswerItem> answers = new();
            List<MetaDataItem> metaData = new();
            List<QuestionMetaDataRelation> questionMetaDataRelations = new();
            List<AnswerMetaDataRelation> answerMetaDataRelations = new();
            List<MetaDataMetaDataRelation> metaDataMetaDataRelations = new();
            List<MetaDataItem> answersMetaDataItems = new();
            List<MetaDataItem> questionMetaDataItems = new();

            long assessmentBackendId = 43384;
            AssessmentItem assessment = CreateAssessmentItem(out long assessmentId, assessmentBackendId, new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"), "IT-Grundwissen");

            
        }

        /// <summary>
        /// Helper to generate a Assessment
        /// </summary>
        /// <param name="assessmentId">Needed for further processing generates the assessmentId</param>
        /// <param name="assessmentBackendId">Sets the backendId</param>
        /// <param name="schemaUri">Schema Url JSON-LD Entity of the Object for now only Url</param>
        /// <param name="title">Some random string</param>
        /// <returns></returns>
        private AssessmentItem CreateAssessmentItem(out long assessmentId, long assessmentBackendId, Uri schemaUri, string title)
        {
            AssessmentItem assessment = new();
            assessmentId = _assessments.Count + 1;

            assessment.Id = assessmentId;
            assessment.BackendId = assessmentBackendId;
            assessment.Schema = schemaUri;
            assessment.Title = title;
            return assessment;
        }

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
