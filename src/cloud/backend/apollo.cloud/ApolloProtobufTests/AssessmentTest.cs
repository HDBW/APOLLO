using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Invite.Apollo.App.Graph.Common.Models.Course.Assessment.Enums;
using Invite.Apollo.App.Graph.Common.Models.Esco;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Test
{
    [TestFixture]
    public class AssessmentTest
    {
        private List<QuestionItem> _questions = new();
        private List<AnswerItem> _answers = new();
        private List<MetaData> _metaDatas = new();
        private List<QuestionMetaDataRelation> _questionMetaDataRelations = new();
        private List<AnswerMetaDataRelation> _answerMetaDataRelations = new();
        private List<MetaDataMetaDataRelation> _metaDataMetaDataRelations = new();
        private List<MetaData> _answersMetaData = new();
        private List<MetaData> _questionMetaData = new();
        private AssessmentItem _assessment = CreateAssessmentItem(title: "Garten-Landschaftsbau");


        public void ShouldBeLongId()
        {
            SetUp();
            
            AssessmentItem assi = new() { Title = "Awesome Test", Id = long.MaxValue, Ticks = DateTime.Now.Ticks };

            _assessment = assi;

            AssessmentResponse ar = new() { CorrelationId = new Guid().ToString() };
            ar.Assessments.Add(assi);


            string filename = "longAssessment.bin";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, ar);
                file.Close();
            }

            AssessmentResponse deserializedAssessment;

            using (var file = File.OpenRead(filename))
            {
                deserializedAssessment = Serializer.Deserialize<AssessmentResponse>(file);
                file.Close();
            }

            //TODO: Create overlaod for Assessment to check Equals
            Assert.IsTrue(deserializedAssessment.Assessments[0].Id.Equals(assi.Id));
        }

        /// <summary>
        /// Testcase: Usecase Associate Question
        /// TODO: Rewrite
        /// </summary>
        //[Test]
        //public void ShouldGenerateAssociateAssessmentItem()
        //{

        //    QuestionItem qi = CreateQuestion(assessment: _assessment, questionLayout: LayoutType.UniformGrid,
        //        answerLayout: LayoutType.UniformGrid, interaction: InteractionType.Associate);
        //    _questions.Add(qi);

        //    MetaData md = CreateMetaData(id: _metaDatas.Count + 1, metaType: MetaDataType.Text,
        //        value: "Du bist für die Rasenpflege verantwortlich. Welche Maschine setzt du für welche Aufgabe ein?");
        //    _metaDatas.Add(md);
        //    _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question:qi,meta:md));

        //    List<long> tmpIdsList = new();

        //    //Create questions

        //    MetaData data = CreateMetaData(_questionMetaDataRelations.Count + 1, MetaDataType.Image, "Maeer.jpg");
        //    _metaDatas.Add(data);
        //    _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question: qi, meta: data));
        //    tmpIdsList.Add(data.Id);

        //    data = CreateMetaData(id: _metaDatas.Count + 1, MetaDataType.Image, "Vertikutierer.jpg");
        //    _metaDatas.Add(data);
        //    _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question: qi, meta: data));
        //    tmpIdsList.Add(data.Id);

        //    data = CreateMetaData(_metaDatas.Count+1, MetaDataType.Image, "Rasenmaeher.jpg");
        //    _metaDatas.Add(data);
        //    _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question: qi, meta: data));
        //    tmpIdsList.Add(data.Id);

        //    data = CreateMetaData(_metaDatas.Count + 1, MetaDataType.Image, "Trimmer.jpg");
        //    _metaDatas.Add(data);
        //    _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question: qi, meta: data));
        //    tmpIdsList.Add(data.Id);

        //    //Create answers

        //    AnswerItem answer = CreateAnswer(id: _answers.Count + 1, question: qi, answerType: AnswerType.Long,
        //        value: tmpIdsList[0].ToString());
        //    _answers.Add(answer);
        //    data = CreateMetaData(id: _metaDatas.Count + 1, metaType: MetaDataType.Text, "Mähen große Rasenflächen");
        //    _metaDatas.Add(data);
        //    _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(id:_answerMetaDataRelations.Count+1,answerId:answer.Id,data.Id));

        //    answer = CreateAnswer(id: _answers.Count + 1, question: qi, answerType: AnswerType.Long,
        //        value: tmpIdsList[1].ToString());
        //    _answers.Add(answer);
        //    data = CreateMetaData(id: _metaDatas.Count + 1, metaType: MetaDataType.Text, "Mähen mittlere Rasenflächen");
        //    _metaDatas.Add(data);
        //    _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(id: _answerMetaDataRelations.Count + 1, answerId: answer.Id, data.Id));

        //    answer = CreateAnswer(_answers.Count + 1, qi, AnswerType.Long, tmpIdsList[2].ToString());
        //    _answers.Add(answer);
        //    data = CreateMetaData(_answerMetaDataRelations.Count + 1, MetaDataType.Text, "Vertikutiern Rasenflächen");
        //    _metaDatas.Add(data);
        //    _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(_answerMetaDataRelations.Count+1, answer.Id,data.Id));

        //    answer = CreateAnswer(_answers.Count + 1, qi, AnswerType.Long, tmpIdsList[3].ToString());
        //    _answers.Add(answer);
        //    data = CreateMetaData(_answerMetaDataRelations.Count + 1, MetaDataType.Text, "Mähen Rasenkanten");
        //    _metaDatas.Add(data);
        //    _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(_answerMetaDataRelations.Count + 1, answer.Id, data.Id));

        //    //Serialize stuff

        //    //Make sure the file is killed!
        //    if (File.Exists("assessmentAssociate.bin"))
        //    {
        //        File.Delete("assessmentAssociate.bin");
        //    }    


        //    using var file = File.Create("assessmentAssociate.bin");
        //    Serializer.Serialize(file, assessment);
        //    file.Close();

        //    //Check with Data
        //    using var readfile = File.OpenRead("assessmentAssociate.bin");
        //    Assessment deserialized = Serializer.Deserialize<Assessment>(readfile);
        //    file.Close();

        //    //Assert.AreEqual(assessment,deserialized);
            
        //    Assert.IsTrue(deserialized.Value.Id.Equals(assessment.Value.Id));
        //}

        /// <summary>
        /// In this usecase we have image and you can press on predefined spots like find it.
        /// </summary>
        [Test]
        public void ShouldGenerateImagemapAssessmentItem()
        {
            //TODO: Setup clear the class datatypes maybe someday?

            List<MetaData> questionMetaData = new();
            List<AnswerItem> answerItems = new();
            List<MetaData> answerMetaData = new();


            //question section

            QuestionItem questionItem = CreateQuestion(assessment: _assessment, LayoutType.Default,
                LayoutType.Overlay, InteractionType.MultiSelect);


            long myfancyindex = _metaDatas.Count;

            questionMetaData.Add(CreateMetaData(myfancyindex++, MetaDataType.Text, "Ein Schaf deiner Herde ist lahm. Du musst es einfangen, um es genauer zu Unbtersichen. An welcher Stelle packst du das Schaf an?"));
            questionMetaData.Add(CreateMetaData(myfancyindex++, MetaDataType.Hint, "Markiere die richtige Stelle auf dem Bild."));
            questionMetaData.Add(CreateMetaData(myfancyindex++,MetaDataType.Image,"Sheep.jpg"));

            //set stuff
            _questions.Add(questionItem);

            //set MetaData for Question
            _questionMetaData.AddRange(questionMetaData);

            //set relations
            foreach (MetaData md in questionMetaData)
            {
                _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(questionItem,md));
            }

            //answer section
            myfancyindex = _answers.Count;
            long myMetaDatasCount = _metaDatas.Count;

            answerItems.Add(CreateAnswer(myfancyindex++, questionItem, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(myMetaDatasCount++, MetaDataType.Point2D, "30,40"));
            answerItems.Add(CreateAnswer(myfancyindex++,questionItem,AnswerType.Boolean,false.ToString()));
            answerMetaData.Add(CreateMetaData(myMetaDatasCount++, MetaDataType.Point2D, "50,100"));
            answerItems.Add(CreateAnswer(myfancyindex++, questionItem, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(myMetaDatasCount++, MetaDataType.Point2D, "150,300"));
            answerItems.Add(CreateAnswer(myfancyindex++, questionItem, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(myMetaDatasCount++, MetaDataType.Point2D, "150,300"));

            //set stuff
            _answers.AddRange(answerItems);
            _answersMetaData.AddRange(answerMetaData);

            long count = _answerMetaDataRelations.Count;
            int c = 0;

            foreach (AnswerItem answer in answerItems)
            {
                _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(count++, answer.Id, answerMetaData[c].Id));
                c++;
            }

            //wrap the thing up in fancy wrapping paper

            AssessmentResponse ar = new();
            ar.Assessments.Add(_assessment);

            string filename = "ImagemapAssessment.bin";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, ar);
                file.Close();
            }

            AssessmentResponse deserializedAssessment;

            using (var file = File.OpenRead(filename))
            {
                deserializedAssessment = Serializer.Deserialize<AssessmentResponse>(file);
                file.Close();
            }

            //TODO: Create overlaod for Assessment to check Equals
            Assert.IsTrue(deserializedAssessment.Assessments.Count>0);
        }

        /// <summary>
        /// Usecase Choice
        /// </summary>
        [Test]
        public void ShouldGenerateChoiceAssessment()
        {
            SetUp();
            List<MetaData> questionsMetaData = new();
            List<AnswerItem> answerItems = new();
            List<MetaData> answerMetaData = new();

            //section question

            QuestionItem questionItem = CreateQuestion(_assessment,LayoutType.Default,LayoutType.UniformGrid,InteractionType.MultiSelect);

            var indexQuestionMeta = _questionMetaData.Count;

            questionsMetaData.Add(CreateMetaData(indexQuestionMeta++, MetaDataType.Text, "Du säuberst die Beete eurer Kunden von Unkraut. Welche Pflansen entfernst du nicht?"));
            questionsMetaData.Add(CreateMetaData(indexQuestionMeta++, MetaDataType.Text, "Bitte wähle 1 bis 3 Antworten aus."));

            _questions.Add(questionItem);
            _questionMetaData.AddRange(questionsMetaData);

            foreach (MetaData md in questionsMetaData)
            {
                _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(questionItem, md));
            }

            //answer section
            var answerIndex = _answers.Count;
            var answerMetaIndex = _answersMetaData.Count;

            answerItems.Add(CreateAnswer(answerIndex++,questionItem,AnswerType.Boolean,true.ToString()));
            answerMetaData.Add(CreateMetaData(answerMetaIndex,MetaDataType.Image, "DeutscheHecke.jpg"));
            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Image, "Grünzeugs.jpg"));
            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Image, "Salbei.jpg"));
            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Image, "Minze.jpg"));

            _answers.AddRange(answerItems);
            _answersMetaData.AddRange(answerMetaData);

            long count = _answerMetaDataRelations.Count;
            int c = 0;

            foreach (AnswerItem item in answerItems)
            {
                _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(count++, item.Id, answerMetaData[c].Id));
                c++;
            }

            //wrap the thing up in fancy wrapping paper
            AssessmentResponse ar = new();
            ar.Assessments.Add(_assessment);

            string filename = "ImagemapAssessment.bin";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, ar);
                file.Close();
            }

            AssessmentResponse deserializedAssessment;

            using (var file = File.OpenRead(filename))
            {
                deserializedAssessment = Serializer.Deserialize<AssessmentResponse>(file);
                file.Close();
            }

            //TODO: Create overlaod for Assessment to check Equals
            Assert.IsTrue(deserializedAssessment.Assessments.Count>0);
        }

        /// <summary>
        /// Usecase Choice
        /// </summary>
        [Test]
        public void ShouldGenerateChoiceScenario2Assessment()
        {
            SetUp();
            List<MetaData> questionsMetaData = new();
            List<AnswerItem> answerItems = new();
            List<MetaData> answerMetaData = new();

            //section question

            //TODO: Verify this should be the default layout? Had we a discussion on this? That we had a special layout for this type?
            QuestionItem questionItem = CreateQuestion(_assessment, LayoutType.Default, LayoutType.Default, InteractionType.MultiSelect);

            var indexQuestionMeta = _questionMetaData.Count;

            questionsMetaData.Add(CreateMetaData(indexQuestionMeta++, MetaDataType.Text, "Du prüfst den Liefersche der bestellten Schrauben und Schraubenmuttern auf Basis der Bestellbestätigung. Welcher Fehler ist ber der Lieferung möglicherweise passiert?"));
            questionsMetaData.Add(CreateMetaData(indexQuestionMeta++, MetaDataType.Text, "Bitte wähle 1 bis 3 Antworten aus."));
            questionsMetaData.Add(CreateMetaData(indexQuestionMeta++, MetaDataType.Text, "Lieferschein.jpg"));

            _questions.Add(questionItem);
            _questionMetaData.AddRange(questionsMetaData);

            foreach (MetaData md in questionsMetaData)
            {
                _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(questionItem, md));
            }

            //answer section
            var answerIndex = _answers.Count;
            var answerMetaIndex = _answersMetaData.Count;

            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Image, "Es wurde zu wenig geliefert."));
            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Image, "Es wurde etwas nicht geliefert."));
            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Image, "Es wurde etwas Falsches geliefert."));
            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Image, "Es wurde zu viel geliefert."));

            _answers.AddRange(answerItems);
            _answersMetaData.AddRange(answerMetaData);

            long count = _answerMetaDataRelations.Count;
            int c = 0;

            foreach (AnswerItem item in answerItems)
            {
                _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(count++, item.Id, answerMetaData[c].Id));
                c++;
            }

            //wrap the thing up in fancy wrapping paper
            AssessmentResponse ar = new();
            ar.Assessments.Add(_assessment);

            string filename = "ImagemapAssessment.bin";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, ar);
                file.Close();
            }

            AssessmentResponse deserializedAssessment;

            using (var file = File.OpenRead(filename))
            {
                deserializedAssessment = Serializer.Deserialize<AssessmentResponse>(file);
                file.Close();
            }

            //TODO: Create overlaod for Assessment to check Equals
            Assert.IsTrue(deserializedAssessment.Assessments.Count > 0);
        }

        /// <summary>
        /// Usecase Choice 3 with labeled MetaData
        /// </summary>
        [Test]
        public void ShouldGenerateChoiceScenario3Assessment()
        {
            SetUp();
            List<MetaData> questionsMetaData = new();
            List<AnswerItem> answerItems = new();
            List<MetaData> answerMetaData = new();

            //section question

            //TODO: Verify this should be the default layout? Had we a discussion on this? That we had a special layout for this type?
            QuestionItem questionItem = CreateQuestion(_assessment, LayoutType.Compare, LayoutType.Default, InteractionType.MultiSelect);

            var indexQuestionMeta = _questionMetaData.Count;

            questionsMetaData.Add(CreateMetaData(indexQuestionMeta++, MetaDataType.Text, "Du prüfst den Liefersche der bestellten Schrauben und Schraubenmuttern auf Basis der Bestellbestätigung. Welcher Fehler ist ber der Lieferung möglicherweise passiert?"));
            questionsMetaData.Add(CreateMetaData(indexQuestionMeta++, MetaDataType.Text, "Bitte wähle 1 bis 3 Antworten aus."));
            long metaDataTarget = indexQuestionMeta++;
            questionsMetaData.Add(CreateMetaData(metaDataTarget, MetaDataType.Text, "Lieferschein.jpg"));
            
            long metaDataIndex = _metaDatas.Count + 1;
            _metaDatas.Add(CreateMetaData(metaDataIndex, MetaDataType.Text,"Lieferschein"));

            MetaDataMetaDataRelation mdmdr = new()
                {
                    Id = _metaDataMetaDataRelations.Count + 1, SourceId = metaDataIndex, TargetId = metaDataTarget,
                    Ticks = DateTime.Now.Ticks
                };

            _metaDataMetaDataRelations.Add(mdmdr);

            metaDataTarget = indexQuestionMeta++;
            questionsMetaData.Add(CreateMetaData(metaDataTarget, MetaDataType.Image, "Bestellbestaetigung.jpg"));

            metaDataIndex = _metaDatas.Count + 1;
            _metaDatas.Add(CreateMetaData(metaDataIndex, MetaDataType.Text, "Bestellbestätigung"));

            mdmdr = new()
            {
                Id = _metaDataMetaDataRelations.Count + 1,
                SourceId = metaDataIndex,
                TargetId = metaDataTarget,
                Ticks = DateTime.Now.Ticks
            };

            _metaDataMetaDataRelations.Add(mdmdr);

            _questions.Add(questionItem);
            _questionMetaData.AddRange(questionsMetaData);

            foreach (MetaData md in questionsMetaData)
            {
                _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(questionItem, md));
            }

            //answer section
            var answerIndex = _answers.Count;
            var answerMetaIndex = _answersMetaData.Count;

            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Image, "Es wurde zu wenig geliefert."));
            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Boolean, true.ToString()));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Image, "Es wurde etwas nicht geliefert."));
            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Image, "Es wurde etwas Falsches geliefert."));
            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Boolean, false.ToString()));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Image, "Es wurde zu viel geliefert."));

            _answers.AddRange(answerItems);
            _answersMetaData.AddRange(answerMetaData);

            long count = _answerMetaDataRelations.Count;
            int c = 0;

            foreach (AnswerItem item in answerItems)
            {
                _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(count++, item.Id, answerMetaData[c].Id));
                c++;
            }

            //wrap the thing up in fancy wrapping paper
            AssessmentResponse ar = new();
            ar.Assessments.Add(_assessment);

            string filename = "ImagemapAssessment.bin";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, ar);
                file.Close();
            }

            AssessmentResponse deserializedAssessment;

            using (var file = File.OpenRead(filename))
            {
                deserializedAssessment = Serializer.Deserialize<AssessmentResponse>(file);
                file.Close();
            }

            //TODO: Create overlaod for Assessment to check Equals
            Assert.IsTrue(deserializedAssessment.Assessments.Count > 0);

            //Assert.IsTrue(deserializedAssessment.Questions[0].MetaDataMetaDataRelations.Count.Equals(ass.Questions[0].MetaDataMetaDataRelations.Count));
        }

        /// <summary>
        /// Usecase Rating
        /// </summary>
        [Test]
        public void ShouldGenerateRatingAssessment()
        {
            SetUp();
            List<MetaData> questionsMetaData = new();
            List<AnswerItem> answerItems = new();
            List<MetaData> answerMetaData = new();

            //section question

            //TODO: Verify this should be the default layout? Had we a discussion on this? That we had a special layout for this type?
            QuestionItem questionItem = CreateQuestion(_assessment, LayoutType.Compare, LayoutType.Default, InteractionType.SingleSelect);

            var indexQuestionMeta = _questionMetaData.Count;

            questionsMetaData.Add(CreateMetaData(indexQuestionMeta++, MetaDataType.Text, "Wie gut kannst du c#?"));
            questionsMetaData.Add(CreateMetaData(indexQuestionMeta++, MetaDataType.Hint, "Bitte wähle zwischen 1 - 5"));

            //set
            _questions.Add(questionItem);
            _questionMetaData.AddRange(questionsMetaData);

            foreach (MetaData md in questionsMetaData)
            {
                _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(questionItem, md));
            }

            //answer section
            var answerIndex = _answers.Count;
            var answerMetaIndex = _answersMetaData.Count;

            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Integer, string.Empty));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Text, "Lesen."));
            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Integer, string.Empty));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Text, "Schreiben."));
            answerItems.Add(CreateAnswer(answerIndex++, questionItem, AnswerType.Integer, string.Empty));
            answerMetaData.Add(CreateMetaData(answerMetaIndex, MetaDataType.Text, "Verstehen."));

            _answers.AddRange(answerItems);
            _answersMetaData.AddRange(answerMetaData);

            long count = _answerMetaDataRelations.Count;
            int c = 0;

            foreach (AnswerItem item in answerItems)
            {
                _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(count++, item.Id, answerMetaData[c].Id));
                c++;
            }

            //wrap the thing up in fancy wrapping paper
            AssessmentResponse ar = new();
            ar.Assessments.Add(_assessment);

            string filename = "ImagemapAssessment.bin";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, ar);
                file.Close();
            }

            AssessmentResponse deserializedAssessment;

            using (var file = File.OpenRead(filename))
            {
                deserializedAssessment = Serializer.Deserialize<AssessmentResponse>(file);
                file.Close();
            }

            //TODO: Create overlaod for Assessment to check Equals
            Assert.IsTrue(deserializedAssessment.Assessments.Count > 0);

            //Assert.IsTrue(deserializedAssessment.Questions[0].MetaDataMetaDataRelations.Count.Equals(ass.Questions[0].MetaDataMetaDataRelations.Count));
        }

        //Helper
        private void SetUp()
        {
            _questions = new();
            _answers = new();
            _questionMetaDataRelations = new();
            _answerMetaDataRelations = new();
            _metaDataMetaDataRelations = new();
            _answersMetaData = new();
            _questionMetaData = new();
            _metaDatas = new();
        }
        
        private AnswerMetaDataRelation CreateAnswerMetaDataRelation(long id, long answerId, long dataId)
        {
            return new()
            {
                Id = id,
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
                QuestionId = question.Id,
                AnswerType = answerType,
                Value = value,
                Ticks = DateTime.Now.Ticks,
            };
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

        private MetaData CreateMetaData(long id, MetaDataType metaType, string value)
        {
            return new()
                {
                    Id = id,
                    Type = metaType,
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

        private static AssessmentItem CreateAssessmentItem(string title)
        {
                return new()
                {
                    Id = 0,
                    Title = title,
                    Ticks = DateTime.Now.Ticks
                };
        }
    }
}
