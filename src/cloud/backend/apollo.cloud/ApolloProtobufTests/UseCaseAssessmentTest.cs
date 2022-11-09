using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.VisualBasic;
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

        

        private QuestionItem AddQuestionItem()
        {
            return null;
        }

        //[Test]
        //public void ShouldGenerateAssessmentECommerceDude()
        //{


        //    long assessmentBackendId = 61282;
        //    AssessmentItem assessment = CreateAssessmentItem(out long assessmentId, assessmentBackendId, CreateSchema(), "Kaufmann/-frau - E-Commerce");

        //    //TODO: push assessment
        //    AssessmentUseCases auc = new AssessmentUseCases(_assessments, _questions, _answers, _metaData,
        //        _questionMetaDataRelations, _answerMetaDataRelations, _metaDataMetaDataRelations);

        //    //TODO: push to overall datastructure
        //}

        //[Test]
        //public void ShouldGenerateAssessmentDigitalSkills()
        //{


        //    long assessmentBackendId = 43384;
        //    AssessmentItem assessment = CreateAssessmentItem(out long assessmentId, assessmentBackendId, CreateSchema(), "Larissa Testdata");

        //    AssessmentUseCases auc = new AssessmentUseCases(_assessments, _questions, _answers, _metaData,
        //        _questionMetaDataRelations, _answerMetaDataRelations, _metaDataMetaDataRelations);
        //}

        [Test]
        public void ShouldGenerateDevAssessment()
        {
            ClearCollections();
            //assessment section

            CreateDevAssessment();

            UseCaseCollections auc = new UseCaseCollections(_assessments, _questions, _answers, _metaData,
                _questionMetaDataRelations, _answerMetaDataRelations, _metaDataMetaDataRelations);

            string filename = "devassessment.bin";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, auc);
                file.Close();
            }

            UseCaseCollections expected;

            using (var file = File.OpenRead(filename))
            {
                expected = Serializer.Deserialize<UseCaseCollections>(file);
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

        [Test]
        public void ShouldGenerateTestAssessment()
        {
            ClearCollections();
            //assessment section

            CreateDevAssessment();

            UseCaseCollections auc = new UseCaseCollections(_assessments, _questions, _answers, _metaData,
                _questionMetaDataRelations, _answerMetaDataRelations, _metaDataMetaDataRelations);

            string filename = "devassessment.bin";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, auc);
                file.Close();
            }

            UseCaseCollections expected;

            using (var file = File.OpenRead(filename))
            {
                expected = Serializer.Deserialize<UseCaseCollections>(file);
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

        private void CreateDevAssessment()
        {
            long assessmentBackendId = 24323;
            AssessmentItem assessment =
                CreateAssessmentItem(out long assessmentId, assessmentBackendId, CreateSchema(), "Fachlagerist:innen");
            _assessments.Add(assessment);

            //question section

            //Question Priority:
            // A: Choice, Eafrequency, Sort, Associate, rating
            // B: Imagemap (Single, Multiple Choice)
            // C: Eaconditions, cloze

            CreateMultipleChoiceQuestion(assessment);
            CreateChoiceCompareQuestion(assessment);
            CreateAssociateAssessmentItem(assessment);
            CreateImageMapAssessment(assessment);
            CreateRatingQuestion(assessment);
        }

        private void CreateTestAssessment()
        {
            long assessmentBackendId = 12345;
            AssessmentItem assessment =
                CreateAssessmentItem(out long assessmentId, assessmentBackendId, CreateSchema(), "Fachlagerist:innen");
            _assessments.Add(assessment);
        }

        private void CreateChoiceCompareQuestion(AssessmentItem assessment)
        {
            Collection<MetaDataItem> questionsMetaData = new();
            Collection<AnswerItem> answerItems = new();
            Collection<MetaDataItem> answerMetaData = new();
            Collection<MetaDataMetaDataRelation> labelMetaData = new();
            Collection<QuestionMetaDataRelation> questionMetaDataRelation = new();

            var question = CreateQuestion(assessment, LayoutType.Compare, LayoutType.Default,
                InteractionType.MultiSelect);

            int indexQuestionMeta = _metaData.Count - 1;
            int indexMetaMetaIndex = _metaDataMetaDataRelations.Count - 1;
            int questionsMetaDataIndexer = questionsMetaData.Count - 1;
            int questionMetaDataRelationIndexer = _questionMetaDataRelations.Count;



            questionsMetaData.Add(CreateMetaData(++indexQuestionMeta, MetaDataType.Text, "Du prüfst den Liefersche der bestellten Schrauben und Schraubenmuttern auf Basis der Bestellbestätigung. Welcher Fehler ist ber der Lieferung möglicherweise passiert?"));
            ++questionsMetaDataIndexer;
            questionMetaDataRelation.Add(CreateQuestionMetaDataRelation(question, questionsMetaData[questionsMetaDataIndexer], questionMetaDataRelation.Count + questionMetaDataRelationIndexer));
            questionsMetaData.Add(CreateMetaData(++indexQuestionMeta, MetaDataType.Hint, "Bitte wähle 1 bis 3 Antworten aus."));
            ++questionsMetaDataIndexer;
            questionMetaDataRelation.Add(CreateQuestionMetaDataRelation(question, questionsMetaData[questionsMetaDataIndexer], questionMetaDataRelation.Count + questionMetaDataRelationIndexer));

            //In this case we have the meta_data_metadata relation
            questionsMetaData.Add(CreateMetaData(++indexQuestionMeta,MetaDataType.Image, "Lieferschein.png"));
            ++questionsMetaDataIndexer;
            questionMetaDataRelation.Add(CreateQuestionMetaDataRelation(question, questionsMetaData[questionsMetaDataIndexer], questionMetaDataRelation.Count + questionMetaDataRelationIndexer));

            questionsMetaData.Add(CreateMetaData(++indexQuestionMeta, MetaDataType.Text, "Lieferschein!"));
            ++questionsMetaDataIndexer;
            labelMetaData.Add(CreateMetaDataMetaDataRelation(++indexMetaMetaIndex, questionsMetaData[questionsMetaDataIndexer - 1], questionsMetaData[questionsMetaDataIndexer]));
            //And another one of these beauties
            questionsMetaData.Add(CreateMetaData(++indexQuestionMeta, MetaDataType.Image, "Bestellbestaetigung.png"));
            ++questionsMetaDataIndexer;
            questionMetaDataRelation.Add(CreateQuestionMetaDataRelation(question, questionsMetaData[questionsMetaDataIndexer], questionMetaDataRelation.Count + questionMetaDataRelationIndexer));

            questionsMetaData.Add(CreateMetaData(++indexQuestionMeta, MetaDataType.Text, "Bestellbestätigungsdingens!"));
            ++questionsMetaDataIndexer;
            labelMetaData.Add(CreateMetaDataMetaDataRelation(++indexMetaMetaIndex, questionsMetaData[questionsMetaDataIndexer - 1], questionsMetaData[questionsMetaDataIndexer]));

            _questions.Add(question);

            foreach (MetaDataItem metaDataItem in questionsMetaData)
            {
                _metaData.Add(metaDataItem);
            }

            foreach (QuestionMetaDataRelation metaDataRelation in questionMetaDataRelation)
            {
                _questionMetaDataRelations.Add(metaDataRelation);
            }

            foreach (MetaDataMetaDataRelation metaDataRelation in labelMetaData)
            {
                _metaDataMetaDataRelations.Add(metaDataRelation);
            }

            int answerIndex = _answers.Count - 1;
            int metaIndex = _metaData.Count - 1;

            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Text, "Es wurde zu wenig geliefert.!"));
            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Text, "Es wurde etwas nicht geliefert."));
            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Text, "Es wurde etwas Falsches geliefert."));
            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Text, "Es wurde zu viel geliefert."));

            if (answerItems.Count == answerMetaData.Count)
            {
                for (int i = 0; i < answerItems.Count; i++)
                {
                    int count = _answerMetaDataRelations.Count - 1;
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

        private MetaDataMetaDataRelation CreateMetaDataMetaDataRelation(int i, MetaDataItem source, MetaDataItem target)
        {
            return new MetaDataMetaDataRelation()
                {
                    Id = i, BackendId = DateTime.Now.Ticks, Schema = CreateSchema(), SourceId = source.Id,
                    TargetId = target.Id,
                    Ticks = DateTime.Now.Ticks
                };
        }

        private void CreateRatingQuestion(AssessmentItem assessment)
        {
            List<MetaDataItem> questionMetaData = new();
            List<AnswerItem> answerItems = new();
            List<MetaDataItem> answerMetaData = new();

            QuestionItem question = CreateQuestion(assessment, LayoutType.Default, LayoutType.Default,
                InteractionType.SingleSelect);

            int metaIndex = _metaData.Count - 1;

            questionMetaData.Add(CreateMetaData(++metaIndex,MetaDataType.Text,"Wie gut kannst Du Schwäbisch?"));
            questionMetaData.Add(CreateMetaData(++metaIndex,MetaDataType.Hint, "Bitte wähle zwischen 1 - 3"));

            foreach (MetaDataItem metaDataItem in questionMetaData)
            {
                _metaData.Add(metaDataItem);
                _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question,metaDataItem, _questionMetaDataRelations.Count));
            }

            int answerIndex = _answers.Count - 1;
            metaIndex = _metaData.Count - 1;

            answerItems.Add(CreateAnswer(++answerIndex,question,AnswerType.Integer,null));
            answerMetaData.Add(CreateMetaData(++metaIndex,MetaDataType.Text,"Hanoi!"));
            answerItems.Add(CreateAnswer(++answerIndex,question,AnswerType.Integer,null));
            answerMetaData.Add(CreateMetaData(++metaIndex,MetaDataType.Text,"Hawoisch Karle, Seitenbacher!"));
            answerItems.Add(CreateAnswer(++answerIndex,question,AnswerType.Integer,null));
            answerMetaData.Add(CreateMetaData(++metaIndex,MetaDataType.Text,"Oz geil!"));


            if (answerItems.Count == answerMetaData.Count)
            {
                for (int i = 0; i < answerItems.Count; i++)
                {
                    int count = _answerMetaDataRelations.Count - 1;
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

        private void CreateAssociateAssessmentItem(AssessmentItem assessment)
        {
            List<MetaDataItem> questionMetaData = new();
            List<AnswerItem> answerItems = new();
            List<MetaDataItem> answerMetaData = new();

            QuestionItem question = CreateQuestion(assessment, LayoutType.UniformGrid, LayoutType.UniformGrid,
                InteractionType.Associate);

            int metaIndex = _metaData.Count - 1;

            questionMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Text, "Du bist für die Rasenpflege verantwortlich. Welche Maschine setzt du für welche Aufgabe ein?"));
            questionMetaData.Add(CreateMetaData(++metaIndex,MetaDataType.Hint, "Bitte ziehe die jeweilige Antwort in die richtige Abbildung"));

            List<long> ids = new();
            questionMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "Maeer.png"));
            ids.Add(metaIndex);
            questionMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "Vertikutierer.png"));
            ids.Add(metaIndex);
            questionMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "Rasenmaeher.png"));
            ids.Add(metaIndex);
            questionMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "Trimmer.png"));
            ids.Add(metaIndex);

            _questions.Add(question);

            foreach (MetaDataItem metaDataItem in questionMetaData)
            {
                _metaData.Add(metaDataItem);
                _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question, metaDataItem, _questionMetaDataRelations.Count));
            }

            int answerIndex = _answers.Count - 1;
            metaIndex = _metaData.Count - 1;

            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Long, ids[0].ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex,MetaDataType.Text, "Mähen große Rasenflächen"));
            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Long, ids[1].ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Text, "Mähen mittlere Rasenflächen"));
            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Long, ids[2].ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Text, "Vertikutiern Rasenflächen"));
            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Long, ids[3].ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Text, "Mähen Rasenkanten"));

            if (answerItems.Count == answerMetaData.Count)
            {
                for (int i = 0; i < answerItems.Count; i++)
                {
                    int count = _answerMetaDataRelations.Count - 1;
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
            questionMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "Sheep.png"));

            _questions.Add(question);

            foreach (MetaDataItem metaDataItem in questionMetaData)
            {
                _metaData.Add(metaDataItem);
                _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question,metaDataItem, _questionMetaDataRelations.Count));
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

            int indexQuestionMeta = _metaData.Count-1;

            questionsMetaData.Add(CreateMetaData(++indexQuestionMeta, MetaDataType.Text,
                "Du säuberst die Beete eurer Kunden von Unkraut. Welche Pflansen entfernst du nicht?"));
            questionsMetaData.Add(CreateMetaData(++indexQuestionMeta, MetaDataType.Hint, "Bitte wähle 1 bis 3 Antworten aus."));

            _questions.Add(question);

            foreach (MetaDataItem item in questionsMetaData)
            {
                _metaData.Add(item);
                _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question, item, _questionMetaDataRelations.Count));
            }

            //answer section
            var answerIndex = _answers.Count-1;
            var metaIndex = _metaData.Count-1;

            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "DeutscheHecke.png"));
            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "Gruenzeugs.png"));
            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "Salbei.png"));
            answerItems.Add(CreateAnswer(++answerIndex, question, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(++metaIndex, MetaDataType.Image, "Minze.png"));

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
        
        private QuestionMetaDataRelation CreateQuestionMetaDataRelation(QuestionItem question, MetaDataItem meta, int index)
        {
            return new()
            {
                Id = index,
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
        /// Helper to generate a AssessmentType
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
            assessment.Duration = TimeSpan.FromMinutes(10);
            assessment.Publisher = "Bertelsmann Stiftung";
            assessment.Kldb = "";
            assessment.Profession = "";
            assessment.AssessmentType = AssessmentType.SkillAssessment;
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

        [Test]
        public void ShouldGenerateAssessmentFile()
        {
            ClearCollections();
            CreateDevAssessment();
            //CreateTestAssessment();

            UseCaseCollections auc = new UseCaseCollections(_assessments, _questions, _answers, _metaData,
                _questionMetaDataRelations, _answerMetaDataRelations, _metaDataMetaDataRelations);

            string filename = "assessments.bin";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, auc);
                file.Close();
            }

            UseCaseCollections expected;

            using (var file = File.OpenRead(filename))
            {
                expected = Serializer.Deserialize<UseCaseCollections>(file);
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


    }
}
