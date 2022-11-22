using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models;
using System.Reflection;
using System.Text;
using Invite.Apollo.App.Graph.Assessment.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Invite.Apollo.App.Graph.Assessment.Data;
using Microsoft.EntityFrameworkCore;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json.Linq;
using Grpc.Core;
using Serilog;
using System;
using System.Globalization;
using Invite.Apollo.App.Graph.Assessment.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;

namespace Invite.Apollo.App.Graph.Assessment.Data
{
    public static class DbInitializer
    {

        public static void Initialize(CourseContext context)
        {
            context.Database.EnsureCreated();


            // Look for any students.
            if (context.Courses.Any())
            {
                return; // DB has been seeded
            }


            foreach (Course course in EduProvider.CourseList.Values)
            {
                context.Courses.Add(course);
               
                
            }
            foreach (Contact contact in EduProvider.Contacts.Values)
            {
                context.CourseContacts.Add(contact);
            }

            context.SaveChanges();
        }

        public static void Initialize(AssessmentContext context)
        {
            context.Database.EnsureCreated();


            // Look for any students.
            if (context.Assessments.Any())
            {
                return; // DB has been seeded
            }

            //string filepath = System.AppDomain.CurrentDomain.BaseDirectory +
            //                  "Data/221111_Booklet_FK_Lagerlogistik.xlsx";
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Booklet_FK_Lagerlogistik.xlsx");
            System.Console.WriteLine(filepath);
            CreateAssessmentFromCsv(filepath, context);

            filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Digitale_Kompetenzen.xlsx");
            System.Console.WriteLine(filepath);
            CreateAssessmentFromCsv(filepath, context);


        }

        private static void CreateAssessmentFromCsv(string filename, AssessmentContext context)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("Expected File not found", filename);
            }

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = null;
            List<BstAssessment> items = new();

            #region Read from CSV into items

            try
            {
                xlWorkbook = xlApp.Workbooks.Open(filename);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;

                var rowCount = xlRange.Rows.Count;
                var colCount = xlRange.Columns.Count;
                
                //this is pointer stuff for iterating over the excel columns and rows
                for (int i = 2; i <= rowCount; i++)
                {

                    BstAssessment item = new();

                    item.ItemId = (xlRange.Cells[i, ExcelColumnIndex.ItemId].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelColumnIndex.ItemId].Value2.ToString()
                        : string.Empty;

                    item.ItemType = (xlRange.Cells[i, ExcelColumnIndex.ItemType].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelColumnIndex.ItemType].Value2.ToString()
                        : string.Empty;

                    item.ItemStem = (xlRange.Cells[i, ExcelColumnIndex.ItemStem].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelColumnIndex.ItemStem].Value2.ToString()
                        : string.Empty;

                    item.ImageResourceName1 = (xlRange.Cells[i, ExcelColumnIndex.ImageResourceName1].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelColumnIndex.ImageResourceName1].Value2.ToString()
                        : string.Empty;

                    item.Instruction = (xlRange.Cells[i, ExcelColumnIndex.Instruction].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelColumnIndex.Instruction].Value2.ToString()
                        : string.Empty;

                    item.NumberOfPrimaryDisctrators =
                        (xlRange.Cells[i, ExcelColumnIndex.NumberOfPrimaryDisctrators].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.NumberOfPrimaryDisctrators].Value2.ToString()
                            : string.Empty;

                    item.NumberSelectable = xlRange.Cells[i, ExcelColumnIndex.NumberSelectable].Value2 != null
                        ? (int)Convert.ToInt32(xlRange.Cells[i, ExcelColumnIndex.NumberSelectable].Value2.ToString())
                        : default;

                    item.HTMLDistractorPrimary_1 =
                        (xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorPrimary_1].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorPrimary_1].Value2.ToString()
                            : string.Empty;

                    item.HTMLDistractorPrimary_2 =
                        (xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorPrimary_2].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorPrimary_2].Value2.ToString()
                            : string.Empty;

                    item.HTMLDistractorPrimary_3 =
                        (xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorPrimary_3].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorPrimary_3].Value2.ToString()
                            : string.Empty;

                    item.HTMLDistractorPrimary_4 =
                        (xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorPrimary_4].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorPrimary_4].Value2.ToString()
                            : string.Empty;


                    item.ScoringOption_1 = (xlRange.Cells[i, ExcelColumnIndex.ScoringOption_1].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelColumnIndex.ScoringOption_1].Value2.ToString()
                        : string.Empty;

                    item.HTMLDistractorSecondary_1 =
                        (xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorSecondary_1].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorSecondary_1].Value2.ToString()
                            : string.Empty;
                    item.HTMLDistractorSecondary_2 =
                        (xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorSecondary_2].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorSecondary_2].Value2.ToString()
                            : string.Empty;
                    item.HTMLDistractorSecondary_3 =
                        (xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorSecondary_3].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorSecondary_3].Value2.ToString()
                            : string.Empty;

                    item.HTMLDistractorSecondary_4 =
                        (xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorSecondary_4].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.HTMLDistractorSecondary_4].Value2.ToString()
                            : string.Empty;

                    item.Credit_ScoringOption_1 = xlRange.Cells[i, ExcelColumnIndex.Credit_ScoringOption_1].Value2 != null
                        ? (int)Convert.ToInt32(xlRange.Cells[i, ExcelColumnIndex.Credit_ScoringOption_1].Value2.ToString())
                        : default;

                    item.DescriptionOfProfession =
                        (xlRange.Cells[i, ExcelColumnIndex.DescriptionOfProfession].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.DescriptionOfProfession].Value2.ToString()
                            : string.Empty;

                    item.Kldb = (xlRange.Cells[i, ExcelColumnIndex.Kldb].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelColumnIndex.Kldb].Value2.ToString()
                        : string.Empty;

                    item.DescriptionOfPartialQualification =
                        (xlRange.Cells[i, ExcelColumnIndex.DescriptionOfPartialQualification].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.DescriptionOfPartialQualification].Value2.ToString()
                            : string.Empty;

                    item.DescriptionOfWorkingProcess =
                        (xlRange.Cells[i, ExcelColumnIndex.DescriptionOfWorkingProcess].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.DescriptionOfWorkingProcess].Value2.ToString()
                            : string.Empty;

                    item.Description =
                        (xlRange.Cells[i, ExcelColumnIndex.Description].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.Description].Value2.ToString()
                            : string.Empty;

                    item.AssessmentType =
                        (xlRange.Cells[i, ExcelColumnIndex.AssessmentType].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.AssessmentType].Value2.ToString()
                            : string.Empty;

                    item.Disclaimer =
                        (xlRange.Cells[i, ExcelColumnIndex.Disclaimer].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.Disclaimer].Value2.ToString()
                            : string.Empty;

                    item.Duration =
                        (xlRange.Cells[i, ExcelColumnIndex.Duration].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.Duration].Value2.ToString()
                            : string.Empty;

                    item.EscoOccupationId =
                        (xlRange.Cells[i, ExcelColumnIndex.EscoOccupationId].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.EscoOccupationId].Value2.ToString()
                            : string.Empty;

                    item.SubjectArea = (xlRange.Cells[i, ExcelColumnIndex.SubjectArea].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelColumnIndex.SubjectArea].Value2.ToString()
                        : string.Empty;

                    item.DescriptionOfSkills = (xlRange.Cells[i, ExcelColumnIndex.DescriptionOfSkills].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelColumnIndex.DescriptionOfSkills].Value2.ToString()
                        : string.Empty;

                    item.EscoId = (xlRange.Cells[i, ExcelColumnIndex.EscoId].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelColumnIndex.EscoId].Value2.ToString()
                        : string.Empty;

                    item.EscoSkills =
                        (xlRange.Cells[i, ExcelColumnIndex.EscoSkills].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.EscoSkills].Value2.ToString()
                            : string.Empty;

                    item.Publisher =
                        (xlRange.Cells[i, ExcelColumnIndex.Publisher].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.Publisher].Value2.ToString()
                            : string.Empty;

                    item.Title =
                        (xlRange.Cells[i, ExcelColumnIndex.Title].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelColumnIndex.Title].Value2.ToString()
                            : string.Empty;

                    item.Limit = xlRange.Cells[i, ExcelColumnIndex.Limit].Value2 != null
                        ? (int)Convert.ToInt32(xlRange.Cells[i, ExcelColumnIndex.Limit].Value2.ToString())
                        : default;

                    
                    item.CourseId = Convert.ToInt64(
                        (xlRange.Cells[i, ExcelColumnIndex.CourseId].Value2 != null)
                            ? xlRange.Cells[i, ExcelColumnIndex.CourseId].Value2
                            : -1);

                    item.QuestionHasPictures = xlRange.Cells[i, ExcelColumnIndex.QuestionHasPicture].Value2 != null
                        ? (int)Convert.ToInt32(xlRange.Cells[i, ExcelColumnIndex.QuestionHasPicture].Value2.ToString())
                        : default;


                    item.AnswerHasPicture = xlRange.Cells[i, ExcelColumnIndex.AnswerHasPicture].Value2 != null
                        ? (int)Convert.ToInt32(xlRange.Cells[i, ExcelColumnIndex.AnswerHasPicture].Value2.ToString())
                        : default;

                    item.AmountAnswers = xlRange.Cells[i, ExcelColumnIndex.AmountAnswers].Value2 != null
                        ? (int)Convert.ToInt32(xlRange.Cells[i, ExcelColumnIndex.AmountAnswers].Value2.ToString())
                        : default;

                    items.Add(item);

                }



            }
            catch (Exception e)
            {
                throw new Exception("AHHHHHH Excel Workbook Stuff", e);
            }
            finally
            {
                xlWorkbook.Close();
            }

            #endregion

            Models.Assessment assessment = null;
            if (items.Count > 0)
                assessment = CreateAssessment(items.First(), context);
            //TODO: Add Items to Database

            Category category = new Category();
            String categoryTitle  = String.Empty;
            string newCategoryTitle = String.Empty;
            foreach (BstAssessment bstAssessment in items)
            {
                newCategoryTitle = bstAssessment.DescriptionOfPartialQualification;
                //Category 
                if (categoryTitle.Equals(String.Empty) || !categoryTitle.Equals(newCategoryTitle))
                {
                    
                    
                    Log.Information($"{DateTime.Now} : {Assembly.GetEntryAssembly()?.GetName().Name} - Create New Category {category.Title}");
                    category = new Category()
                    {
                        Title = bstAssessment.DescriptionOfPartialQualification,
                        CourseId = bstAssessment.CourseId,
                        EscoId = bstAssessment.EscoId,
                        ResultLimit = bstAssessment.Limit,
                        Schema = CreateApolloSchema(),
                        Ticks = DateTime.Now.Ticks,
                        Subject = bstAssessment.SubjectArea,
                        Description = bstAssessment.DescriptionOfSkills
                        //TODO: @talisi please add minima and maxima to excel
                        //Maximum = ,
                        //Minimum = 
                    };
                    categoryTitle = newCategoryTitle;
                    context.SaveChanges();

                    

                }
                //Category

                Question question = CreateQuestion(assessment, bstAssessment, category, context);
                if (!categoryTitle.Equals(String.Empty) && category.Questions != null)
                    Log.Information($"{DateTime.Now} : {Assembly.GetEntryAssembly()?.GetName().Name} - Category {category.Title} has Questions {category.Questions.Count}");
                
                //category.Questions.Add(question);
                
                context.SaveChanges();

                MetaData mdInstruction = CreateMetaData(MetaDataType.Hint, bstAssessment.Instruction, context);
                CreateQuestionHasMetaData(mdInstruction, question, context);
                
                switch (bstAssessment.GetQuestionType())
                {
                    case QuestionType.Associate:

                        MetaData md =  CreateMetaData(MetaDataType.Text,bstAssessment.ItemStem, context);
                        MetaData md1 = CreateMetaData(MetaDataType.Image, bstAssessment.HTMLDistractorSecondary_1, context);
                        MetaData md2 = CreateMetaData(MetaDataType.Image, bstAssessment.HTMLDistractorSecondary_2, context);
                        MetaData md3 = CreateMetaData(MetaDataType.Image, bstAssessment.HTMLDistractorSecondary_3, context);
                        MetaData md4 = CreateMetaData(MetaDataType.Image, bstAssessment.HTMLDistractorSecondary_4, context);
                        
                        CreateQuestionHasMetaData(md, question, context);
                        CreateQuestionHasMetaData(md1, question, context);
                        CreateQuestionHasMetaData(md2, question, context);
                        CreateQuestionHasMetaData(md3, question, context);
                        CreateQuestionHasMetaData(md4, question, context);

                        for (int i = 0; i < bstAssessment.AmountAnswers; i++)
                        {
                            Answer answer = CreateAnswer(question, bstAssessment, i, context);
                            MetaData answerMetaData =
                                CreateMetaData(MetaDataType.Text, bstAssessment.GetHTMLDistractorPrimary(i), context);
                            AnswerHasMetaData answerHasMetaData =
                                CreateAnswerHasMetaData(answerMetaData, answer, context);
                        }
                        
                        break;
                    case QuestionType.Choice:
                        switch (bstAssessment.ItemType.ToUpper())
                        {
                            case "CHOICE":
                                MetaData qmd = CreateMetaData(MetaDataType.Text, bstAssessment.ItemStem, context);
                                CreateQuestionHasMetaData(qmd, question,context);
                                for (int i = 0; i < bstAssessment.AmountAnswers; i++)
                                {
                                    Answer tempAnswer = CreateAnswer(question, bstAssessment, i, context);
                                    MetaData tempAnswerMetaData = CreateMetaData(MetaDataType.Text,
                                        bstAssessment.GetHTMLDistractorPrimary(i), context);
                                    AnswerHasMetaData tempAnswerHasMetaData = CreateAnswerHasMetaData(tempAnswerMetaData, tempAnswer, context);
                                }
                                break;
                            case "CHOICE_AP":
                                MetaData qapmd = CreateMetaData(MetaDataType.Text, bstAssessment.ItemType, context);
                                CreateQuestionHasMetaData(qapmd, question, context);

                                for (int i = 0; i < bstAssessment.AmountAnswers; i++)
                                {
                                    Answer tempAnswer = CreateAnswer(question, bstAssessment, i, context);
                                    MetaData tempAnswerMetaData = CreateMetaData(MetaDataType.Image,
                                        bstAssessment.GetHTMLDistractorPrimary(i), context);
                                    AnswerHasMetaData tempAnswerHasMetaData =
                                        CreateAnswerHasMetaData(tempAnswerMetaData, tempAnswer, context);
                                }
                                break;
                            case "CHOICE_QP":
                                MetaData cmd = CreateMetaData(MetaDataType.Text, bstAssessment.ItemStem, context);
                                MetaData cmd1 = CreateMetaData(MetaDataType.Image, bstAssessment.ImageResourceName1,
                                    context);
                                CreateQuestionHasMetaData(cmd, question, context);
                                CreateQuestionHasMetaData(cmd1, question, context);

                                for (int i = 0; i < bstAssessment.AmountAnswers; i++)
                                {
                                    Answer temAnswer = CreateAnswer(question, bstAssessment, i, context);
                                    MetaData temMetaData = CreateMetaData(MetaDataType.Text, bstAssessment.GetHTMLDistractorPrimary(i),
                                        context);
                                    AnswerHasMetaData temAnswerHasMetaData =
                                        CreateAnswerHasMetaData(temMetaData, temAnswer, context);
                                }
                                break;
                            default:
                                //TODO: move choice and choice ap to default?
                                break;
                        }
                        break;
                    case QuestionType.Imagemap:
                        MetaData mdText = CreateMetaData(MetaDataType.Text, bstAssessment.ItemStem, context);
                        MetaData mdImage =
                            CreateMetaData(MetaDataType.Image, bstAssessment.ImageResourceName1, context);

                        CreateQuestionHasMetaData(mdText, question, context);
                        CreateQuestionHasMetaData(mdImage, question, context);

                        for (int i = 0; i < bstAssessment.AmountAnswers; i++)
                        {
                            Answer a = CreateAnswer(question, bstAssessment, i, context);
                            MetaData amd = CreateMetaData(MetaDataType.Point2D, bstAssessment.GetHTMLDistractorPrimary(i), context);
                            AnswerHasMetaData ahmd = CreateAnswerHasMetaData(amd,a,context);
                        }
                        break;
                    case QuestionType.Rating:
                        MetaData questionText = CreateMetaData(MetaDataType.Text, bstAssessment.ItemStem, context);
                        QuestionHasMetaData qhmd = CreateQuestionHasMetaData(questionText, question, context);
                        for (int i = 0; i < bstAssessment.AmountAnswers; i++)
                        {
                            Answer answerItem = CreateAnswer(question, bstAssessment, i, context);
                            MetaData answerMetaData = CreateMetaData(MetaDataType.Text,bstAssessment.GetHTMLDistractorPrimary(i), context);
                            AnswerHasMetaData answerHasMeta =
                                CreateAnswerHasMetaData(answerMetaData, answerItem, context);
                        }
                        break;
                    case QuestionType.Sort:
                        MetaData tempData = CreateMetaData(MetaDataType.Text, bstAssessment.ItemStem, context);
                        var tempQuestionMetaData = CreateQuestionHasMetaData(tempData, question, context);

                        for (int i = 0; i < bstAssessment.AmountAnswers; i++)
                        {
                            Answer answerItem = CreateAnswer(question, bstAssessment, i, context);
                            MetaData answerMetaDataItem = CreateMetaData(MetaDataType.Text,
                                bstAssessment.GetHTMLDistractorPrimary(i), context);
                            AnswerHasMetaData amdi = CreateAnswerHasMetaData(answerMetaDataItem, answerItem, context);
                            MetaData ansMetaDataStartPosition = CreateMetaData(MetaDataType.Position,i.ToString(), context);
                            AnswerHasMetaData amdipos =
                                CreateAnswerHasMetaData(ansMetaDataStartPosition, answerItem, context);
                        }

                        break;
                    case QuestionType.Survey:
                        //TODO: Add Position and Pole Description to MetaData?
                        //Problem of Tomorrow Patric
                        break;
                }
            }


        }

        private static MetaData CreateMetaData(MetaDataType text, string value, AssessmentContext context)
        {
            MetaData md = new()
            {
                Type = MetaDataType.Text,
                Value = value,
                Ticks = DateTime.Now.Ticks,
                Schema = CreateApolloSchema()

            };
            context.MetaDatas.Add(md);
            context.SaveChanges();
            return md;
        }

        private static QuestionHasMetaData CreateQuestionHasMetaData(MetaData md, Question question, AssessmentContext context)
        {
            QuestionHasMetaData qm1 = new()
            {
                Ticks = DateTime.Now.Ticks,
                Schema = CreateApolloSchema(),
                MetaData = md,
                Question = question,
                QuestionId = question.Id
            };
            if (question.QuestionHasMetaDatas == null)
                question.QuestionHasMetaDatas = new List<QuestionHasMetaData>();
            question.QuestionHasMetaDatas.Add(qm1);
            context.SaveChanges();
            return qm1;
        }

        private static AnswerHasMetaData CreateAnswerHasMetaData(MetaData md, Answer answer, AssessmentContext context)
        {
            AnswerHasMetaData qm1 = new()
            {
                Ticks = DateTime.Now.Ticks,
                Schema = CreateApolloSchema(),
                MetaData = md,
                Answer = answer,
                AnswerId = answer.Id
            };
            if (answer.AnswerHasMetaDatas == null)
                answer.AnswerHasMetaDatas = new List<AnswerHasMetaData>();
            answer.AnswerHasMetaDatas.Add(qm1);
            context.SaveChanges();
            return qm1;
        }


        private static Answer CreateAnswer(Question question, BstAssessment bstAssessment, int answerIndex, AssessmentContext context)
        {
            Answer answer = new();
            answer.QuestionId = question.Id;
            answer.AnswerType = bstAssessment.GetAnswerType();

            var questionType = bstAssessment.GetQuestionType();
            var somevalue = bstAssessment.GetHTMLDistractorPrimary(answerIndex);
            string resultvector = bstAssessment.ScoringOption_1;
            var strReplace = resultvector.Replace(" ", "");
            var result = strReplace.Split('-');

            switch (questionType)
            {
                case QuestionType.Associate:
                    answer.Value = bstAssessment.GetHTMLDistractorSecondary(answerIndex);
                    break;
                case QuestionType.Choice:
                    answer.Value = Convert.ToBoolean(Convert.ToInt16(result[answerIndex])).ToString();
                    break;
                case QuestionType.Imagemap:
                    answer.Value = bstAssessment.GetHTMLDistractorPrimary(answerIndex);
                    break;
                case QuestionType.Binary:
                    answer.Value = Convert.ToBoolean(Convert.ToInt16(result[answerIndex])).ToString();
                    break;
                case QuestionType.Sort:
                    answer.Value = result[answerIndex].ToString();
                    break;
                case QuestionType.Rating:
                    answer.Value = result[answerIndex].ToString();
                    break;
                case QuestionType.Eafrequency:
                    answer.Value = result[answerIndex].ToString();
                    break;
                case QuestionType.Survey:
                    answer.Value = result[answerIndex].ToString();
                    break;
                //TODO: Rating Implementation
            }

            //answer.Value = bstAssessment.GetHTMLDistractorPrimary(answerIndex);
            //answer.BackendId = DateTime.Now.Ticks;
            answer.Schema = CreateApolloSchema();
            answer.Ticks = DateTime.Now.Ticks;
            context.Answers.Add(answer);
            context.SaveChanges();
            return answer;
        }

        private static Question CreateQuestion(Models.Assessment assessment, BstAssessment bstAssessment, Category category, AssessmentContext context)
        {
            Question question = new();
            question.AssessmentId = assessment.Id;
            question.Assessment = assessment;
            question.ExternalId = bstAssessment.ItemId;
            question.QuestionType = bstAssessment.GetQuestionType();
            question.Category = category;
            question.CategoryId = category.CourseId;
            question.ScoringOption = bstAssessment.ScoringOption_1.Replace(" ","");
            question.Scalar = bstAssessment.Credit_ScoringOption_1;
            //TODO: Set via Mapping
            //question.//QuestionLayout = questionLayoutType,
            //question.//AnswerLayout = answerLayoutType,
            //question.//Interaction = interactionType,
            question.Ticks = DateTime.Now.Ticks;
            question.Schema = CreateApolloSchema();

            context.Questions.Add(question);
            context.SaveChanges();
            Log.Information($"{DateTime.Now} : {Assembly.GetEntryAssembly()?.GetName().Name} - Create New Question {question.Id}");
            return question;
        }

        private static Models.Assessment CreateAssessment(BstAssessment bstAssessment, AssessmentContext context)
        {

            Models.Assessment assessment  = new Models.Assessment
            {
                Kldb = bstAssessment.Kldb,
                AssessmentType = AssessmentType.SkillAssessment,
                Description = bstAssessment.Description,
                Disclaimer = bstAssessment.Disclaimer,
                Duration = TimeSpan.Zero,
                EscoOccupationId = new Uri("http://data.europa.eu/esco/occupation/f2b15a0e-e65a-438a-affb-29b9d50b77d1").ToString(),
                EscoSkills = new List<EscoSkill>(),
                ExternalId = bstAssessment.ItemId,
                Profession = bstAssessment.DescriptionOfProfession,
                //TODO: Change for Survey
                Publisher = "Bertelsmann Stiftung",
                Title = bstAssessment.Title,
                Schema = CreateApolloSchema(),
                Ticks = DateTime.Now.Ticks,
            };

            context.Assessments.Add(assessment);
            context.SaveChanges();
            Log.Information($"{DateTime.Now} : {Assembly.GetEntryAssembly()?.GetName().Name} - Create New Assessment {assessment.Title}");
            return assessment;
        }

        private static Uri CreateApolloSchema()
        {
            return new Uri($"https://invite-apollo.app/{Guid.NewGuid()}");
        }

        #region Create CSV 

        private static void CreateMetaDataMetaDataCsv()
        {
            PropertyInfo[] properties = typeof(MetaDataMetaDataRelation).GetProperties();

            string filename = "MetaDataMetaDataRelation.csv";

            CreateCsV(properties, filename);
        }

        private static void CreateAnswersMetaDataCsv()
        {
            PropertyInfo[] properties = typeof(AnswerMetaDataRelation).GetProperties();

            string filename = "AnswerMetaDataRelation.csv";

            CreateCsV(properties, filename);
        }

        private static void CreateQuestionMetaDataCsv()
        {
            PropertyInfo[] properties = typeof(QuestionMetaDataRelation).GetProperties();

            string filename = "assessment.csv";

            CreateCsV(properties, filename);
        }

        private static void CreateMetaDataCsv()
        {
            PropertyInfo[] properties = typeof(MetaDataItem).GetProperties();

            string filename = "MetaDataItem.csv";

            CreateCsV(properties, filename);
        }

        private static void CreateAnswersCsv()
        {
            PropertyInfo[] properties = typeof(AnswerItem).GetProperties();

            string filename = "AnswerItem.csv";

            CreateCsV(properties, filename);
        }

        private static void CreateQuestionsCsv()
        {
            PropertyInfo[] properties = typeof(QuestionItem).GetProperties();

            string filename = "QuestionItem.csv";

            CreateCsV(properties, filename);
        }

        private static void CreateAssessmentCsv()
        {
            PropertyInfo[] properties = typeof(AssessmentItem).GetProperties();

            string filename = "AssessmentItem.csv";

            CreateCsV(properties, filename);
        }

        private static void CreateCsV(PropertyInfo[] properties, string filename)
        {
            StringBuilder csvString = new();
            StringBuilder sbHeadBuilder = new();
            StringBuilder sbHeadDescription = new();

            foreach (PropertyInfo property in properties)
            {
                sbHeadBuilder.Append($"{property.Name};");
            }

            foreach (PropertyInfo property in properties)
            {
                sbHeadDescription.Append($"{property.PropertyType};");
            }

            csvString.AppendLine(sbHeadBuilder.ToString());
            csvString.AppendLine(sbHeadDescription.ToString());


            File.WriteAllText(filename, csvString.ToString());
            Console.WriteLine(
                $"Created {filename} @ {DateTime.Now.ToUniversalTime()} : With {properties.Length} entries.");
        }

        #endregion
    }
}
