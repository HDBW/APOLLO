// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;
using Graph.Apollo.Cloud.Common.Models.Assessment;
using Graph.Apollo.Cloud.Common.Models.Assessment.Enums;
using Graph.Apollo.Cloud.Common.Models.Taxonomy;
using ProtoBuf;

namespace ApolloProtobufTests
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

        /// <summary>
        /// Testcase: Usecase Associate Question
        /// TODO: Rewrite
        /// </summary>
        [Test]
        public void ShouldGenerateAssociateAssessmentItem()
        {

            QuestionItem qi = CreateQuestion(assessment: _assessment, questionLayout: LayoutType.UniformGrid,
                answerLayout: LayoutType.UniformGrid, interaction: InteractionType.Associate);
            _questions.Add(qi);

            MetaData md = CreateMetaData(id: _metaDatas.Count + 1, metaType: MetaDataType.Text,
                value: "Du bist für die Rasenpflege verantwortlich. Welche Maschine setzt du für welche Aufgabe ein?");
            _metaDatas.Add(md);
            _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question:qi,meta:md));

            List<long> tmpIdsList = new();

            //Create questions

            MetaData data = CreateMetaData(_questionMetaDataRelations.Count + 1, MetaDataType.Image, "Maeer.jpg");
            _metaDatas.Add(data);
            _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question: qi, meta: data));
            tmpIdsList.Add(data.Id);

            data = CreateMetaData(id: _metaDatas.Count + 1, MetaDataType.Image, "Vertikutierer.jpg");
            _metaDatas.Add(data);
            _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question: qi, meta: data));
            tmpIdsList.Add(data.Id);

            data = CreateMetaData(_metaDatas.Count+1, MetaDataType.Image, "Rasenmaeher.jpg");
            _metaDatas.Add(data);
            _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question: qi, meta: data));
            tmpIdsList.Add(data.Id);

            data = CreateMetaData(_metaDatas.Count + 1, MetaDataType.Image, "Trimmer.jpg");
            _metaDatas.Add(data);
            _questionMetaDataRelations.Add(CreateQuestionMetaDataRelation(question: qi, meta: data));
            tmpIdsList.Add(data.Id);

            //Create answers

            AnswerItem answer = CreateAnswer(id: _answers.Count + 1, question: qi, answerType: AnswerType.Long,
                value: tmpIdsList[0].ToString());
            _answers.Add(answer);
            data = CreateMetaData(id: _metaDatas.Count + 1, metaType: MetaDataType.Text, "Mähen große Rasenflächen");
            _metaDatas.Add(data);
            _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(id:_answerMetaDataRelations.Count+1,answerId:answer.Id,data.Id));

            answer = CreateAnswer(id: _answers.Count + 1, question: qi, answerType: AnswerType.Long,
                value: tmpIdsList[1].ToString());
            _answers.Add(answer);
            data = CreateMetaData(id: _metaDatas.Count + 1, metaType: MetaDataType.Text, "Mähen mittlere Rasenflächen");
            _metaDatas.Add(data);
            _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(id: _answerMetaDataRelations.Count + 1, answerId: answer.Id, data.Id));

            answer = CreateAnswer(_answers.Count + 1, qi, AnswerType.Long, tmpIdsList[2].ToString());
            _answers.Add(answer);
            data = CreateMetaData(_answerMetaDataRelations.Count + 1, MetaDataType.Text, "Vertikutiern Rasenflächen");
            _metaDatas.Add(data);
            _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(_answerMetaDataRelations.Count+1, answer.Id,data.Id));

            answer = CreateAnswer(_answers.Count + 1, qi, AnswerType.Long, tmpIdsList[3].ToString());
            _answers.Add(answer);
            data = CreateMetaData(_answerMetaDataRelations.Count + 1, MetaDataType.Text, "Mähen Rasenkanten");
            _metaDatas.Add(data);
            _answerMetaDataRelations.Add(CreateAnswerMetaDataRelation(_answerMetaDataRelations.Count + 1, answer.Id, data.Id));

            //TODO: Deserialize
            //In order to deserialize the data we need to wrap it in assessment class.
            Assessment assessment = new();
            assessment.Value = _assessment;
            assessment.Questions = SerializeQuestion();
            assessment.Occupation = new ();
            assessment.Occupation.Schema = "http://data.europa.eu/esco/occupation/a10eb17a-3c78-4f7a-a1da-8f31146339d3";
            assessment.Skills = new();
            assessment.Skills.Add(new Skill(){Schema = "https://esco.ec.europa.eu/en/classification/skills?uri=http://data.europa.eu/esco/skill/51097602-8e4e-4830-8f35-d77afc713fc0" });
            assessment.Skills.Add(new Skill() { Schema = "https://esco.ec.europa.eu/en/classification/skills?uri=http://data.europa.eu/esco/skill/8ce4a4af-969b-4d1c-8bfe-891cff82f913" });
            assessment.Skills.Add(new Skill() { Schema = "https://esco.ec.europa.eu/en/classification/skills?uri=http://data.europa.eu/esco/skill/8a1942a8-bebd-4cc5-bdff-fe215aaa3de5" });
            //.. and a lot more .. //
            
            assessment.Ticks = DateTime.Now.Ticks;
            
            //Serialize stuff

            //Make sure the file is killed!
            if (File.Exists("assessmentAssociate.bin"))
            {
                File.Delete("assessmentAssociate.bin");
            }    


            using var file = File.Create("assessmentAssociate.bin");
            Serializer.Serialize(file, assessment);
            file.Close();

            //Check with Data
            using var readfile = File.OpenRead("assessmentAssociate.bin");
            Assessment deserialized = Serializer.Deserialize<Assessment>(readfile);
            file.Close();

            //Assert.AreEqual(assessment,deserialized);
            
            Assert.IsTrue(deserialized.Value.Id.Equals(assessment.Value.Id));
        }

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
            //TODO: Should that be possible to do ???
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
            Assessment ass = AssessmentWrapper();

            string filename = "ImagemapAssessment.bin";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, ass);
                file.Close();
            }

            Assessment deserializedAssessment;

            using (var file = File.OpenRead(filename))
            {
                deserializedAssessment = Serializer.Deserialize<Assessment>(file);
                file.Close();
            }

            //TODO: Create overlaod for Assessment to check Equals
            Assert.IsTrue(deserializedAssessment.Questions.Count.Equals(ass.Questions.Count));
        }

        /// <summary>
        /// Usecase Choice
        /// </summary>
        [Test]
        public void ShouldGenerateChoiceAssessment()
        {
            
        }

        //Helper
        private Assessment AssessmentWrapper()
        {
            Assessment ass = new() { Value = _assessment };
            ass.Value.Ticks = DateTime.Now.Ticks;
            ass.Questions.AddRange(QuestionsWrapper());
            ass.Occupation = new() { Schema = "http://data.europa.eu/esco/occupation/a10eb17a-3c78-4f7a-a1da-8f31146339d3" };
            ass.Skills = new()
            {
                new Skill()
                {
                    Schema =
                        "https://esco.ec.europa.eu/en/classification/skills?uri=http://data.europa.eu/esco/skill/51097602-8e4e-4830-8f35-d77afc713fc0"
                },
                new Skill()
                {
                    Schema =
                        "https://esco.ec.europa.eu/en/classification/skills?uri=http://data.europa.eu/esco/skill/8ce4a4af-969b-4d1c-8bfe-891cff82f913"
                },
                new Skill()
                {
                    Schema =
                        "https://esco.ec.europa.eu/en/classification/skills?uri=http://data.europa.eu/esco/skill/8a1942a8-bebd-4cc5-bdff-fe215aaa3de5"
                }
            };
            return ass;
        }

        private List<Question> QuestionsWrapper()
        {
            List<Question> questions = new();

            foreach (QuestionItem item in _questions)
            {
                Question q = new()
                    {
                        Value = item,
                        Answers = _answers,
                        AnswerMetaDatas = _answersMetaData,
                        AnswerMetaDataRelations = _answerMetaDataRelations,
                        QuestionMetaDataRelations = _questionMetaDataRelations,
                        QuestionsMetaDatas = _questionMetaData
                    };
            }

            return questions;
        }

        private List<Question> SerializeQuestion()
        {
            List<Question> returnQuestions = new();

            foreach (QuestionItem questionItem in _questions)
            {
                Question question = new();
                question.Value = questionItem;
                question.Answers = _answers;
                question.AnswerMetaDataRelations = _answerMetaDataRelations;
                IEnumerable<MetaData> ametadata()
                {
                    foreach (var md in _metaDatas)
                    {
                        foreach (var relation in _answerMetaDataRelations)
                        {
                            if (md.Id.Equals(relation.MetaDataId))
                            {
                                yield return md;
                            }
                        }
                    }
                }

                question.AnswerMetaDatas = ametadata() as List<MetaData>;

                question.QuestionMetaDataRelations = _questionMetaDataRelations;

                IEnumerable<MetaData> qmetadatas()
                {
                    foreach (var md in _metaDatas)
                    {
                        foreach (var relation in _questionMetaDataRelations)
                        {
                            if (md.Id.Equals(relation.MetaDataId))
                            {
                                yield return md;
                            }
                        }
                    }
                }

                question.QuestionsMetaDatas = qmetadatas() as List<MetaData>;

                question.MetaDataMetaDataRelations = _metaDataMetaDataRelations;

                return returnQuestions;
            }

            return returnQuestions;
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
