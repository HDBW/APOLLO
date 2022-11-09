using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using Grpc.Core;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Test
{
    [TestFixture]
    public class UseCaseAssessmentDevTest
    {
        private AssessmentDictonary _assessments = new AssessmentDictonary();

        [Test]
        public void ShouldGenerateAssessmentBinFiles()
        {
            CreateAssessment(out AssessmentItem item);
            item = _assessments.AddAssessmentItem(item);
            Debug.WriteLine(item.Dump);

            CreateMultipleChoiceQuestion(item);
            CreateChoiceCompareQuestion(item);
            CreateAssociateQuestion(item);
            CreateImageMapQuestion(item);

            UseCaseCollections auc = new UseCaseCollections(_assessments.GetAssessmentItems(), _assessments.GetQuestionItems(), _assessments.GetAnswerItems(), _assessments.GetMetaDataItems(),
                _assessments.GetQuestionMetaDataRelations(), _assessments.GetAnswerMetaDataRelations(), _assessments.GetMetaDataMetaDataRelations());

            using (StreamWriter file = File.CreateText(@"devtest.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, auc);
            }

            string filename = "devtest.bin";

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
            Debug.WriteLine($"Questions Count: {expected.QuestionItems.Count}");
            Debug.WriteLine($"Answers Count: {expected.AnswerItems.Count}");
            Debug.WriteLine($"MetaData Count: {expected.MetaDataItems.Count}");
            Debug.WriteLine($"QuestionsRelations Count: {expected.QuestionMetaDataRelations.Count}");
            Debug.WriteLine($"AnswerRelations Count: {expected.AnswerMetaDataRelations.Count}");
            Debug.WriteLine($"MetaDataMetaDataRelations Count: {expected.MetaDataMetaDataRelations.Count}");

            Assert.IsTrue(auc.QuestionItems.Count.Equals(expected.QuestionItems.Count));
            Assert.IsTrue(auc.AnswerItems.Count.Equals(expected.AnswerItems.Count));
            Assert.IsTrue(auc.MetaDataItems.Count.Equals(expected.MetaDataItems.Count));
            Assert.IsTrue(auc.QuestionMetaDataRelations.Count.Equals(expected.QuestionMetaDataRelations.Count));
            Assert.IsTrue(auc.AnswerMetaDataRelations.Count.Equals(expected.AnswerMetaDataRelations.Count));
            Assert.IsTrue(auc.MetaDataMetaDataRelations.Count.Equals(expected.MetaDataMetaDataRelations.Count));
        }

        private void CreateAssociateQuestion(AssessmentItem assessment)
        {
            QuestionItem question = CreateQuestionItem(assessment, LayoutType.UniformGrid, LayoutType.UniformGrid,
                InteractionType.Associate);

            question = _assessments.AddQuestionItem(question);

            MetaDataItem md = CreateMetaDataItem(MetaDataType.Text, "Du bist für die Rasenpflege verantwortlich. Welche Maschine setzt du für welche Aufgabe ein?");
            MetaDataItem mdh = CreateMetaDataItem(MetaDataType.Hint, "itte ziehe die jeweilige Antwort in die richtige Abbildung");
            MetaDataItem mdi = CreateMetaDataItem(MetaDataType.Image, "Maeer.png");
            MetaDataItem mdi1 = CreateMetaDataItem(MetaDataType.Image, "Vertikutierer.png");
            MetaDataItem mdi2 = CreateMetaDataItem(MetaDataType.Image, "Rasenmaeher.png");
            MetaDataItem mdi3 = CreateMetaDataItem(MetaDataType.Image, "Trimmer.png");

            md = _assessments.AddMetaDataItem(md);
            mdh = _assessments.AddMetaDataItem(mdh);
            mdi = _assessments.AddMetaDataItem(mdi);
            mdi1 = _assessments.AddMetaDataItem(mdi1);
            mdi2 = _assessments.AddMetaDataItem(mdi2);
            mdi3 = _assessments.AddMetaDataItem(mdi3);

            var qmdr = _assessments.AddMetaDataToQuestion(question, md);
            var qmdh = _assessments.AddMetaDataToQuestion(question, mdh);
            var qmdi = _assessments.AddMetaDataToQuestion(question, mdi);
            var qmdi1 = _assessments.AddMetaDataToQuestion(question, mdi1);
            var qmdi2 = _assessments.AddMetaDataToQuestion(question, mdi2);
            var qmdi3 = _assessments.AddMetaDataToQuestion(question, mdi3);

            Debug.WriteLine(question);

            Debug.WriteLine(md);
            Debug.WriteLine(mdh);
            Debug.WriteLine(mdi);
            Debug.WriteLine(mdi1);
            Debug.WriteLine(mdi2);
            Debug.WriteLine(mdi3);

            Debug.WriteLine(qmdr);
            Debug.WriteLine(qmdh);
            Debug.WriteLine(qmdi);
            Debug.WriteLine(qmdi1);
            Debug.WriteLine(qmdi2);
            Debug.WriteLine(qmdi3);

            AnswerItem answerItem = CreateAnswer(question, AnswerType.Long, mdi.Value);
            answerItem = _assessments.AddAnswerItem(answerItem);
            AnswerItem answerItem1 = CreateAnswer(question, AnswerType.Long, mdi1.Value);
            answerItem1 = _assessments.AddAnswerItem(answerItem1);
            AnswerItem answerItem2 = CreateAnswer(question, AnswerType.Long, mdi2.Value);
            answerItem2 = _assessments.AddAnswerItem(answerItem2);
            AnswerItem answerItem3 = CreateAnswer(question, AnswerType.Long, mdi3.Value);
            answerItem3 = _assessments.AddAnswerItem(answerItem3);

            MetaDataItem answerMetaDataItem = CreateMetaDataItem(MetaDataType.Text, "Mähen große Rasenflächen");
            MetaDataItem answerMetaDataItem1 = CreateMetaDataItem(MetaDataType.Text, "Vertikutiern Rasenflächen");
            MetaDataItem answerMetaDataItem2 = CreateMetaDataItem(MetaDataType.Text, "Mähen mittlere Rasenflächen");
            MetaDataItem answerMetaDataItem3 = CreateMetaDataItem(MetaDataType.Text, "Mähen Rasenkanten");

            answerMetaDataItem = _assessments.AddMetaDataItem(answerMetaDataItem);
            answerMetaDataItem1 = _assessments.AddMetaDataItem(answerMetaDataItem1);
            answerMetaDataItem2 = _assessments.AddMetaDataItem(answerMetaDataItem2);
            answerMetaDataItem3 = _assessments.AddMetaDataItem(answerMetaDataItem3);

            var amdr = _assessments.AddMetaDataToAnswer(answerItem, answerMetaDataItem);
            var amdr1 = _assessments.AddMetaDataToAnswer(answerItem1, answerMetaDataItem1);
            var amdr2 = _assessments.AddMetaDataToAnswer(answerItem2, answerMetaDataItem2);
            var amdr3 = _assessments.AddMetaDataToAnswer(answerItem3, answerMetaDataItem3);

            Debug.WriteLine(answerItem.Dump);
            Debug.WriteLine(answerItem1.Dump);
            Debug.WriteLine(answerItem2.Dump);
            Debug.WriteLine(answerItem3.Dump);
            Debug.WriteLine(answerMetaDataItem.Dump);
            Debug.WriteLine(answerMetaDataItem1.Dump);
            Debug.WriteLine(answerMetaDataItem2.Dump);
            Debug.WriteLine(answerMetaDataItem3.Dump);
            Debug.WriteLine(amdr.Dump);
            Debug.WriteLine(amdr1.Dump);
            Debug.WriteLine(amdr2.Dump);
            Debug.WriteLine(amdr3.Dump);
        }

        private void CreateImageMapQuestion(AssessmentItem assessmentItem)
        {
            QuestionItem questionItem = CreateQuestionItem(assessmentItem, LayoutType.Default, LayoutType.Overlay,
                InteractionType.MultiSelect);

            questionItem = _assessments.AddQuestionItem(questionItem);

            MetaDataItem mdText = CreateMetaDataItem(MetaDataType.Text, "Ein Schaf deiner Herde ist lahm. Du musst es einfangen, um es genauer zu Unbtersichen. An welcher Stelle packst du das Schaf an?");
            MetaDataItem mdHint = CreateMetaDataItem(MetaDataType.Hint, "Markiere die richtige Stelle auf dem Bild.");
            MetaDataItem mdImage = CreateMetaDataItem(MetaDataType.Image, "Sheep.png");

            mdText = _assessments.AddMetaDataItem(mdText);
            mdHint = _assessments.AddMetaDataItem(mdHint);
            mdImage = _assessments.AddMetaDataItem(mdImage);

            var qmdT = _assessments.AddMetaDataToQuestion(questionItem, mdText);
            var qmdH = _assessments.AddMetaDataToQuestion(questionItem, mdHint);
            var qmdI = _assessments.AddMetaDataToQuestion(questionItem, mdImage);

            Debug.WriteLine(questionItem);
            Debug.WriteLine(mdText);
            Debug.WriteLine(mdHint);
            Debug.WriteLine(mdImage);
            Debug.WriteLine(qmdT);
            Debug.WriteLine(qmdH);
            Debug.WriteLine(qmdI);

            AnswerItem answerItem = CreateAnswer(questionItem, AnswerType.Boolean, true.ToString());
            MetaDataItem answerMetaData = CreateMetaDataItem(MetaDataType.Point2D, "30,40");
            AnswerItem answerItem1 = CreateAnswer(questionItem, AnswerType.Boolean, false.ToString());
            MetaDataItem answerMetaData1 = CreateMetaDataItem(MetaDataType.Point2D, "50,100");
            AnswerItem answerItem2 = CreateAnswer(questionItem, AnswerType.Boolean, false.ToString());
            MetaDataItem answerMetaData2 = CreateMetaDataItem(MetaDataType.Point2D, "150,300");
            AnswerItem answerItem3 = CreateAnswer(questionItem, AnswerType.Boolean, false.ToString());
            MetaDataItem answerMetaData3 = CreateMetaDataItem(MetaDataType.Point2D, "100,100");
            
            answerItem = _assessments.AddAnswerItem(answerItem);
            answerItem1 = _assessments.AddAnswerItem(answerItem1);
            answerItem2 = _assessments.AddAnswerItem(answerItem2);
            answerItem3 = _assessments.AddAnswerItem(answerItem3);

            answerMetaData = _assessments.AddMetaDataItem(answerMetaData);
            answerMetaData1 = _assessments.AddMetaDataItem(answerMetaData1);
            answerMetaData2 = _assessments.AddMetaDataItem(answerMetaData2);
            answerMetaData3 = _assessments.AddMetaDataItem(answerMetaData3);

            var amdr = _assessments.AddMetaDataToAnswer(answerItem, answerMetaData);
            var amdr1 = _assessments.AddMetaDataToAnswer(answerItem1, answerMetaData1);
            var amdr2 = _assessments.AddMetaDataToAnswer(answerItem2, answerMetaData2);
            var amdr3 = _assessments.AddMetaDataToAnswer(answerItem3, answerMetaData3);

            Debug.WriteLine(answerItem);
            Debug.WriteLine(answerMetaData);
            Debug.WriteLine(amdr);
            Debug.WriteLine(answerItem1);
            Debug.WriteLine(answerMetaData1);
            Debug.WriteLine(amdr1);
            Debug.WriteLine(answerItem2);
            Debug.WriteLine(answerMetaData2);
            Debug.WriteLine(amdr2);
            Debug.WriteLine(answerItem3);
            Debug.WriteLine(answerMetaData3);
            Debug.WriteLine(amdr3);
        }

        private void CreateChoiceCompareQuestion(AssessmentItem assessment)
        {
            QuestionItem questionItem = CreateQuestionItem(assessment, LayoutType.Compare, LayoutType.Default,
                InteractionType.MultiSelect);
            questionItem = _assessments.AddQuestionItem(questionItem);

            MetaDataItem md = CreateMetaDataItem(MetaDataType.Text,
                "Du prüfst den Liefersche der bestellten Schrauben und Schraubenmuttern auf Basis der Bestellbestätigung. Welcher Fehler ist ber der Lieferung möglicherweise passiert?");
            md = _assessments.AddMetaDataItem(md);
            var mdr = _assessments.AddMetaDataToQuestion(questionItem, md);

            MetaDataItem mdh = CreateMetaDataItem(MetaDataType.Hint, "Bitte wähle 1 bis 3 Antworten aus.");
            mdh = _assessments.AddMetaDataItem(mdh);
            var mdhr = _assessments.AddMetaDataToQuestion(questionItem, mdh);

            MetaDataItem mdi = CreateMetaDataItem(MetaDataType.Image, "Lieferschein.png");
            MetaDataItem mdit = CreateMetaDataItem(MetaDataType.Text, "Lieferschein");
            MetaDataItem mdi1 = CreateMetaDataItem(MetaDataType.Image, "Bestellbestaetigung.png");
            MetaDataItem mdit1 = CreateMetaDataItem(MetaDataType.Text, "Bestellbestätigungsdingens!");

            mdi = _assessments.AddMetaDataItem(mdi);
            var mdir = _assessments.AddMetaDataToQuestion(questionItem,mdi);
            mdit = _assessments.AddMetaDataItem(mdit);
            var mditr = _assessments.AddLabel(mdit, mdi);
            mdi1 = _assessments.AddMetaDataItem(mdi1);
            var mdi1r = _assessments.AddMetaDataToQuestion(questionItem, mdi1);
            mdit1 = _assessments.AddMetaDataItem(mdit1);
            var mdit1r = _assessments.AddLabel(mdit1, mdi1);

            Debug.WriteLine(questionItem);
            Debug.WriteLine(md);
            Debug.WriteLine(mdr);
            Debug.WriteLine(mdh);
            Debug.WriteLine(mdhr);

            Debug.WriteLine(mdi);
            Debug.WriteLine(mdit);
            Debug.WriteLine(mdi1);
            Debug.WriteLine(mdit1);

            Debug.WriteLine(mdir);
            Debug.WriteLine(mditr);
            Debug.WriteLine(mdi1r);
            Debug.WriteLine(mdit1r);

            AnswerItem answerItem = CreateAnswer(questionItem, AnswerType.Boolean, false.ToString());
            answerItem = _assessments.AddAnswerItem(answerItem);
            MetaDataItem ansMetaDataItem = CreateMetaDataItem(MetaDataType.Text, "Es wurde zu wenig geliefert.!");
            ansMetaDataItem = _assessments.AddMetaDataItem(ansMetaDataItem);
            var amdr = _assessments.AddMetaDataToAnswer(answerItem, ansMetaDataItem);

            AnswerItem answerItem1 = CreateAnswer(questionItem, AnswerType.Boolean, true.ToString());
            answerItem1 = _assessments.AddAnswerItem(answerItem1);
            MetaDataItem answerMetaDataItem =
                CreateMetaDataItem(MetaDataType.Text, "Es wurde etwas nicht geliefert.");
            answerMetaDataItem = _assessments.AddMetaDataItem(answerMetaDataItem);
            var amdr1 = _assessments.AddMetaDataToAnswer(answerItem1, answerMetaDataItem);

            AnswerItem answerItem2 = CreateAnswer(questionItem, AnswerType.Boolean, false.ToString());
            answerItem2 = _assessments.AddAnswerItem(answerItem2);
            MetaDataItem answerMetaDataItem2 = CreateMetaDataItem(MetaDataType.Text, "Es wurde etwas Falsches geliefert.");
            answerMetaDataItem2 = _assessments.AddMetaDataItem(answerMetaDataItem2);
            var amdr2 = _assessments.AddMetaDataToAnswer(answerItem2, answerMetaDataItem2);

            AnswerItem answerItem3 = CreateAnswer(questionItem, AnswerType.Boolean, false.ToString());
            answerItem3 = _assessments.AddAnswerItem(answerItem3);
            MetaDataItem answerMetaDataItem3 = CreateMetaDataItem(MetaDataType.Text, "Es wurde zu viel geliefert.");
            answerMetaDataItem3 = _assessments.AddMetaDataItem(answerMetaDataItem3);
            var amdr3 = _assessments.AddMetaDataToAnswer(answerItem3, answerMetaDataItem3);

            Debug.WriteLine(answerItem);
            Debug.WriteLine(ansMetaDataItem);
            Debug.WriteLine(amdr);
            Debug.WriteLine(answerItem1);
            Debug.WriteLine(answerMetaDataItem);
            Debug.WriteLine(amdr1);
            Debug.WriteLine(answerItem2);
            Debug.WriteLine(answerMetaDataItem2);
            Debug.WriteLine(amdr2);
            Debug.WriteLine(answerItem3);
            Debug.WriteLine(answerMetaDataItem3);
            Debug.WriteLine(amdr3);
        }

        private void CreateMultipleChoiceQuestion(AssessmentItem assessment)
        {
            #region Question Section

            QuestionItem question = CreateQuestionItem(assessment, LayoutType.Default, LayoutType.Overlay, InteractionType.MultiSelect);

            question = _assessments.AddQuestionItem(question);

            MetaDataItem md = CreateMetaDataItem(MetaDataType.Text, "Du säuberst die Beete eurer Kunden von Unkraut. Welche Pflanzen entfernst du nicht?");

            md = _assessments.AddMetaDataItem(md);

            var mdr = _assessments.AddMetaDataToQuestion(question, md);

            MetaDataItem mdh = CreateMetaDataItem(MetaDataType.Hint, "Bitte wähle 1 bis 3 Antworten aus.");
            mdh = _assessments.AddMetaDataItem(mdh);

            var mdrh = _assessments.AddMetaDataToQuestion(question, mdh);

            Debug.WriteLine(question.Dump);
            Debug.WriteLine(md.Dump);
            Debug.WriteLine(mdr.Dump);
            Debug.WriteLine(mdh.Dump);
            Debug.WriteLine(mdrh.Dump);

            #endregion

            #region Answer Section

            //new item
            AnswerItem answerItem = CreateAnswer(question, AnswerType.Boolean, true.ToString());
            answerItem = _assessments.AddAnswerItem(answerItem);

            MetaDataItem answerMetaDataItem = CreateMetaDataItem(MetaDataType.Image, "DeutscheHecke.png");
            answerMetaDataItem = _assessments.AddMetaDataItem(answerMetaDataItem);

            AnswerMetaDataRelation amdr = _assessments.AddMetaDataToAnswer(answerItem, answerMetaDataItem);

            //new item

            AnswerItem answerItem1 = CreateAnswer(question, AnswerType.Boolean, true.ToString());
            answerItem1 = _assessments.AddAnswerItem(answerItem1);

            MetaDataItem answerMetaDataItem1 = CreateMetaDataItem(MetaDataType.Image, "Gruenzeugs.png");
            answerMetaDataItem1 = _assessments.AddMetaDataItem(answerMetaDataItem1);

            AnswerMetaDataRelation amdr1 = _assessments.AddMetaDataToAnswer(answerItem1, answerMetaDataItem1);

            //new item
            AnswerItem answerItem2 = CreateAnswer(question, AnswerType.Boolean, false.ToString());
            answerItem2 = _assessments.AddAnswerItem(answerItem2);

            MetaDataItem answerMetaDataItem2 = CreateMetaDataItem(MetaDataType.Image, "Salbei.png");
            answerMetaDataItem2 = _assessments.AddMetaDataItem(answerMetaDataItem2);

            AnswerMetaDataRelation amdr2 = _assessments.AddMetaDataToAnswer(answerItem2, answerMetaDataItem2);

            //new item
            AnswerItem answerItem3 = CreateAnswer(question, AnswerType.Boolean, true.ToString());
            answerItem3 = _assessments.AddAnswerItem(answerItem3);

            MetaDataItem answerMetaDataItem3 = CreateMetaDataItem(MetaDataType.Image, "Minze.png");
            answerMetaDataItem2 = _assessments.AddMetaDataItem(answerMetaDataItem3);

            AnswerMetaDataRelation amdr3 = _assessments.AddMetaDataToAnswer(answerItem3, answerMetaDataItem3);



            Debug.WriteLine(answerItem.Dump);
            Debug.WriteLine(answerMetaDataItem.Dump);
            Debug.WriteLine(amdr.Dump);
            Debug.WriteLine(answerItem1.Dump);
            Debug.WriteLine(answerMetaDataItem1.Dump);
            Debug.WriteLine(amdr1.Dump);
            Debug.WriteLine(answerItem2.Dump);
            Debug.WriteLine(answerMetaDataItem2.Dump);
            Debug.WriteLine(amdr2.Dump);
            Debug.WriteLine(answerItem3.Dump);
            Debug.WriteLine(answerMetaDataItem3.Dump);
            Debug.WriteLine(amdr3.Dump);

            #endregion

        }

        private AnswerItem CreateAnswer(QuestionItem question, AnswerType answertype, string value)
        {
            AnswerItem answerItem = new();
            answerItem.QuestionId = question.Id;
            answerItem.AnswerType = answertype;
            answerItem.Value = value;
            answerItem.BackendId = DateTime.Now.Ticks;
            answerItem.Schema = CreateApolloSchema();
            answerItem.Ticks = DateTime.Now.Ticks;
            return answerItem;
        }

        private MetaDataItem CreateMetaDataItem(MetaDataType type, string value)
        {
            MetaDataItem md = new() { Type = type, Value = value, Ticks = DateTime.Now.Ticks, Schema = CreateApolloSchema(),
                BackendId = DateTime.Now.Ticks
            };
            return md;
        }

        private QuestionItem CreateQuestionItem(AssessmentItem assessment,LayoutType questionLayoutType, LayoutType answerLayoutType, InteractionType interactionType)
        {
            QuestionItem question = new()
                {
                    AssessmentId = assessment.Id, QuestionLayout = questionLayoutType, AnswerLayout = answerLayoutType,
                    Interaction = interactionType,
                    BackendId = DateTime.Now.Ticks,
                    Ticks = DateTime.Now.Ticks,
                    Schema = CreateApolloSchema()
                };
            return question;
        }

        private void CreateAssessment(out AssessmentItem item)
        {
            item = new()
                {
                    Schema = CreateApolloSchema(),
                    BackendId = DateTime.Now.Ticks,
                    AssessmentType = AssessmentType.Unknown,
                    Publisher = "HDBW DEV Team",
                    Duration = TimeSpan.FromMinutes(0),
                    EscoOccupationId = new Uri("http://data.europa.eu/esco/occupation/f2b15a0e-e65a-438a-affb-29b9d50b77d1").ToString(),
                    Kldb = "43412",
                    Profession = "Developer",
                    Title = "Developer Test Assessment",
                    Description = "Dieses Assessment ist ein Developer Test um die UI Elemente zu Entwickeln und testen. Hier könnte auch Lorem Ipsum stehen, ist aber langweilig. Deswegen steht hier was anderes.",
                    Disclaimer = "Dieses Assessment kann nicht abbrechen und so Zuegs, das geht mal gar nicht klar!",
                    Ticks = DateTime.Now.Ticks
                };
        }

        private Uri CreateApolloSchema()
        {
            return new Uri($"https://invite-apollo.app/{Guid.NewGuid()}");
        }
    }
}
