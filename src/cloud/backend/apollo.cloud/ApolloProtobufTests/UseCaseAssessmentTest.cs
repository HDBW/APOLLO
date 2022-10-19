using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using NUnit.Framework;
using ProtoBuf;
using ReassureTest;

namespace Invite.Apollo.App.Graph.Common.Test
{
    [TestFixture]
    public class UseCaseAssessmentTest
    {
        private Collection<QuestionItem> _questions = new();
        private Collection<AnswerItem> _answers = new();
        private Collection<MetaDataItem> _metaData = new();
        private Collection<QuestionMetaDataRelation> _questionMetaDataRelations = new();
        private Collection<AnswerMetaDataRelation> _answerMetaDataRelations = new();
        private Collection<MetaDataMetaDataRelation> _metaDataMetaDataRelations = new();
        private Collection<AssessmentItem> _assessments = new();

        [Test]
        public void ShouldGenerateAssessmentECommerceDude()
        {


            long assessmentBackendId = 61282;
            AssessmentItem assessment = CreateAssessmentItem(out long assessmentId, assessmentBackendId, CreateSchema(), "Kaufmann/-frau - E-Commerce");

            //TODO: push assessment
            AssessmentUseCases auc = new AssessmentUseCases(_assessments, _questions, _answers, _metaData,
                _questionMetaDataRelations, _answerMetaDataRelations, _metaDataMetaDataRelations);

            //TODO: push to overall datastructure
        }

        [Test]
        public void ShouldGenerateAssessmentDigitalSkills()
        {


            long assessmentBackendId = 43384;
            AssessmentItem assessment = CreateAssessmentItem(out long assessmentId, assessmentBackendId, CreateSchema(), "IT-Grundwissen");

            //TODO: push assessment
            AssessmentUseCases auc = new AssessmentUseCases(_assessments, _questions, _answers, _metaData,
                _questionMetaDataRelations, _answerMetaDataRelations, _metaDataMetaDataRelations);

            //TODO: push to overall datastructure
        }

        [Test]
        public void ShouldOnlyGenerateAssessmentWarehouseOperator()
        {
            ClearCollections();

            //assessment section

            long assessmentBackendId = 24323;
            AssessmentItem assessment = CreateAssessmentItem(out long assessmentId, assessmentBackendId, CreateSchema(), "Fachlagerist:innen");
            _assessments.Add(assessment);

            //question section

            //Question Priority:
            // A: Choice, Eafrequency, Sort, Associate, rating
            // B: Imagemap (Single, Multiple Choice)
            // C: Eaconditions, cloze

            CreateMultipleChoiceQuestion(assessment);
            //CreateAssociateAssessmentItem(assessment);
            CreateImageMapAssessment(assessment);


            AssessmentUseCases auc = new AssessmentUseCases(_assessments, _questions, _answers, _metaData,
                _questionMetaDataRelations, _answerMetaDataRelations, _metaDataMetaDataRelations);

            string filename = "MultipleChoiceAssessment.bin";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, auc);
                file.Close();
            }

            AssessmentUseCases expected;

            using (var file = File.OpenRead(filename))
            {
                expected = Serializer.Deserialize<AssessmentUseCases>(file);
                file.Close();
            }

            //Debug purposes
            Debug.WriteLine(expected.QuestionItems.Count);
            if (expected.QuestionItems != null)
            {
                Assert.AreEqual(expected.AnswerItems.Count, auc.AnswerItems.Count);
            }

            Assert.IsTrue(auc.QuestionItems.Count.Equals(expected.QuestionItems.Count));
        }

        //private void CreateAssociateAssessmentItem(AssessmentItem assessment)
        //{
        //    List<MetaDataItem> questionMetaData = new();
        //    List<AnswerItem> answerItems = new();
        //    List<MetaDataItem> answerMetaData = new();

        //    QuestionItem question = CreateQuestion(assessment, LayoutType.UniformGrid, LayoutType.UniformGrid,
        //        InteractionType.Associate);

        //    int metaIndex = _metaData.Count - 1;

        //    questionMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Text, "Du bist für die Rasenpflege verantwortlich. Welche Maschine setzt du für welche Aufgabe ein?"));
        //    questionMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image,"Maeer.jpg"));
        //    questionMetaData.Add(CreateMetaData(++metaIndex,MetaDataType.Image, "Vertikutierer.jpg"));
        //    questionMetaData.Add(CreateMetaData(++metaIndex,MetaDataType.Image, "Rasenmaeher.jpg"));
        //    questionMetaData.Add(CreateMetaData(++metaIndex,MetaDataType.Image,"Trimmer.jpg"));

        //    foreach (MetaDataItem metaDataItem in questionMetaData)
        //    {
        //        _metaData.Add(metaDataItem);
        //        _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question, metaDataItem));
        //    }

        //    int answerIndex = _answers.Count - 1;
        //    metaIndex = _metaData.Count - 1;

        //    answerItems.Add(CreateAnswer(++answerIndex,question,AnswerType.Long,))

        //}

        private void CreateImageMapAssessment(AssessmentItem assessment)
        {
            List<MetaDataItem> questionMetaData = new();
            List<AnswerItem> answerItems = new();
            List<MetaDataItem> answerMetaData = new();

            //question section

            QuestionItem question = CreateQuestion(assessment, LayoutType.Default, LayoutType.Overlay,
                InteractionType.MultiSelect);

            int metaIndex = _metaData.Count-1;

            questionMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Text, "Ein Schaf deiner Herde ist lahm. Du musst es einfangen, um es genauer zu Unbtersichen. An welcher Stelle packst du das Schaf an?"));
            questionMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Hint, "Markiere die richtige Stelle auf dem Bild."));
            questionMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "Sheep.jpg"));

            _questions.Add(question);

            foreach (MetaDataItem metaDataItem in questionMetaData)
            {
                _metaData.Add(metaDataItem);
                _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question,metaDataItem));
            }

            //section answer
            int answerIndex = _answers.Count - 1;
            metaIndex = _metaData.Count - 1;

            answerItems.Add(CreateAnswer(++metaIndex, question, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Point2D, "30,40"));
            answerItems.Add(CreateAnswer(++metaIndex, question, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Point2D, "50,100"));
            answerItems.Add(CreateAnswer(++metaIndex, question, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Point2D, "150,300"));
            answerItems.Add(CreateAnswer(++metaIndex, question, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Point2D, "150,300"));

            if (answerItems.Count == answerMetaData.Count)
            {
                for (int i = 0; i < answerItems.Count; i++)
                {
                    int count = _answerMetaDataRelations.Count-1;
                    _answers.Add(answerItems[i]);
                    _metaData.Add(answerMetaData[i]);
                    _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(++count, answerItems[i].Id, answerMetaData[i].Id));
                }
            }
            else
            {
                throw new Exception("Metadata and Answers are a mess");
            }
        }

        private void CreateMultipleChoiceQuestion(AssessmentItem assessment)
        {
            Collection<MetaDataItem> questionsMetaData = new();
            Collection<AnswerItem> answerItems = new();
            Collection<MetaDataItem> answerMetaData = new();

            var question = CreateQuestion(assessment, LayoutType.Default, LayoutType.Overlay,
                InteractionType.MultiSelect);

            var indexQuestionMeta = _metaData.Count-1;

            questionsMetaData.Add(CreateMetaData(++indexQuestionMeta, MetaDataType.Text,
                "Du säuberst die Beete eurer Kunden von Unkraut. Welche Pflansen entfernst du nicht?"));
            questionsMetaData.Add(CreateMetaData(++indexQuestionMeta, MetaDataType.Text, "Bitte wähle 1 bis 3 Antworten aus."));

            _questions.Add(question);

            foreach (MetaDataItem item in questionsMetaData)
            {
                _metaData.Add(item);
                _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question, item));
            }

            //answer section
            var answerIndex = _answers.Count-1;
            var metaIndex = _metaData.Count-1;

            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "DeutscheHecke.jpg"));
            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "Grünzeugs.jpg"));
            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "Salbei.jpg"));
            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "Minze.jpg"));

            if (answerItems.Count == answerMetaData.Count)
            {
                for (int i = 0; i < answerItems.Count; i++)
                {
                    int count = _answerMetaDataRelations.Count-1;
                    _answers.Add(answerItems[i]);
                    _metaData.Add(answerMetaData[i]);
                    _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(++count, answerItems[i].Id,
                        answerMetaData[i].Id));
                }
            }
            else
            {
                throw new Exception("Metadata and Answers are a mess");
            }
        }

        private AnswerMetaDataRelation CreateAnswerMetaDataRelation(long id, long answerId, long dataId)
        {
            return new()
            {
                Id = id,
                BackendId = DateTime.Now.Ticks,
                Schema = CreateSchema(),
                AnswerId = answerId,
                MetaDataId = dataId,
                Ticks = DateTime.Now.Ticks
            };
        }

        private AnswerItem CreateAnswer(long id, QuestionItem question, AnswerType answerType, string value)
        {
            return new()
            {
                Id = id,
                BackendId = DateTime.Now.Ticks,
                Schema = CreateSchema(),
                QuestionId = question.Id,
                AnswerType = answerType,
                Value = value,
                Ticks = DateTime.Now.Ticks,
            };
        }




        private QuestionMetaDataRelation CreateQuestionMetaDataRelation(QuestionItem question, MetaDataItem meta)
        {
            return new()
            {
                Id = _questionMetaDataRelations.Count,
                BackendId = DateTime.Now.Ticks,
                Schema = CreateSchema(),
                QuestionId = question.Id,
                MetaDataId = meta.Id,
                Ticks = DateTime.Now.Ticks 
            };
        }

        private Uri CreateSchema()
        {
            return new Uri($"https://invite-apollo.app/{Guid.NewGuid()}");
        }

        private QuestionItem CreateQuestion(AssessmentItem assessment, LayoutType questionLayout, LayoutType answerLayout, InteractionType interaction)
        {
            return new()
            {
                Id = _questions.Count,
                BackendId = DateTime.Now.Ticks,
                Schema = CreateSchema(),
                AssessmentId = assessment.Id,
                AnswerLayout = answerLayout,
                QuestionLayout = questionLayout,
                Interaction = interaction,
                Ticks = DateTime.Now.Ticks
            };
        }

        private MetaDataItem CreateMetaData(long id, MetaDataType metaType, string value)
        {
            return new()
            {
                Id = id,
                BackendId = DateTime.Now.Ticks,
                Schema = CreateSchema(),
                Type = metaType,
                Value = value,
                Ticks = DateTime.Now.Ticks
            };
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
            assessmentId = _assessments.Count;

            assessment.Id = assessmentId;
            assessment.BackendId = assessmentBackendId;
            assessment.Schema = schemaUri;
            assessment.Title = title;
            return assessment;
        }

        private void ClearCollections()
        {
            _questions = new();
            _answers = new();
            _metaData = new();
            _questionMetaDataRelations = new();
            _answerMetaDataRelations = new();
            _metaDataMetaDataRelations = new();
            _assessments = new();
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
