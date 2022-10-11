// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Graph.Apollo.Cloud.Common.Models.Assessment;
using Graph.Apollo.Cloud.Common.Models.Assessment.Enums;

namespace ApolloProtobufTests
{
    [TestFixture]
    public class AssessmentTest
    {
        private List<AssessmentItem> _assessments = new();
        private List<QuestionItem> _questions = new();
        private List<AnswerItem> _answers = new();
        private List<MetaData> _metaDatas = new();
        private List<QuestionMetaDataRelation> _questionMetaDataRelations = new();
        private List<AnswerMetaDataRelation> _answerMetaDataRelations = new();
        private List<MetaDataMetaDataRelation> _metaDataMetaDataRelations = new();

        /// <summary>
        /// Testcase: Usecase Associate Question
        /// </summary>
        [Test]
        public void ShouldGenerateAssociateAssessmentItem()
        {
            AssessmentItem item = CreateAssessmentItem(title:"Garten-Landschaftsbau");
            _assessments.Add(item);

            QuestionItem qi = CreateQuestion(assessment: item, questionLayout: LayoutType.UniformGrid,
                answerLayout: LayoutType.UniformGrid, interaction: InteractionType.Associate);
            _questions.Add(qi);

            MetaData md = CreateMetaData(id: _metaDatas.Count + 1, type: MetaDataType.Text,
                value: "Du bist für die Rasenpflege verantwortlich. Welche Maschine setzt du für welche Aufgabe ein?");
            _metaDatas.Add(md);
            _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question:qi,meta:md));

            //TODO: Continue create Test
        }

        private QuestionMetaDataRelation CreateQuestionMetaDataRelation(QuestionItem question, MetaData meta)
        {
            return new()
                {
                    Id = _questionMetaDataRelations.Count + 1,
                    QuestionId = question.Id,
                    MetaDataId = meta.Id
                };
        }

        private MetaData CreateMetaData(long id, MetaDataType type, string value)
        {
            return new()
                {
                    Id = id,
                    Type = type,
                    Value = value,
                    Ticks = DateTime.Now.Ticks
                };
        }

        private QuestionItem CreateQuestion(AssessmentItem assessment, LayoutType questionLayout, LayoutType answerLayout, InteractionType interaction)
        {
            return new()
                {
                    Id = _questions.Count + 1, AssessmentId = assessment.Id, AnswerLayout = answerLayout,
                    QuestionLayout = questionLayout,
                    Interaction = interaction,
                    Ticks = DateTime.Now.Ticks
                };
        }

        private AssessmentItem CreateAssessmentItem(string title)
        {
                return new()
                {
                    Id = _assessments.Count + 1,
                    Title = title,
                    Ticks = DateTime.Now.Ticks
                };
        }

        [Test]
        public void ShouldGenerateQuestions()
        {

        }

        public void ShouldGenerateDataStructure()
        {

        }
    }
}
