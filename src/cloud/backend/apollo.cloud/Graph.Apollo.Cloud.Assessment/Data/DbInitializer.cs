using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models;
using System.Reflection;
using System.Text;
using Invite.Apollo.App.Graph.Assessment.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Excel = Microsoft.Office.Interop.Excel;

namespace Invite.Apollo.App.Graph.Assessment.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AssessmentContext context)
        {
            context.Database.EnsureCreated();


            // Look for any students.
            if (context.Assessments.Any())
            {
                return; // DB has been seeded
            }

            CreateAssessmentFromCsv("Data/221111_Booklet_FK_Lagerlogistik.xlsx");


        }

        private static void CreateAssessmentFromCsv(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("Expected File not found", filename);
            }

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filename);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            var rowCount = xlRange.Rows.Count;
            var colCount = xlRange.Columns.Count;

            List<BstAssessment> items = new();

            //this is pointer stuff for iterating over the excel columns and rows
            for (int i = 1; i <= rowCount; i++)
            {
                if (i == 1)
                {
                    continue;
                }

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

                items.Add(item);

                //new line
                //if (j == 1)
                //    Console.Write("\r\n");

                //write the value to the console
                //if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                //    Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");

                //add useful things here!



            }

            //TODO: Add Items to Database
            foreach (BstAssessment bstAssessment in items)
            {

                Models.Assessment assessment = CreateAssessment(bstAssessment);



                Question question = CreateQuestion(assessment, bstAssessment);





            }


        }

        private static Question CreateQuestion(Models.Assessment assessment, BstAssessment bstAssessment)
        {
            Question question = new()
            {
                AssessmentId = assessment.Id,
                //QuestionLayout = questionLayoutType,
                //AnswerLayout = answerLayoutType,
                //Interaction = interactionType,
                //BackendId = DateTime.Now.Ticks,
                Ticks = DateTime.Now.Ticks,
                Schema = CreateApolloSchema()
            };
            return question;
        }

        private static Models.Assessment CreateAssessment(BstAssessment bstAssessment)
        {
            return new Models.Assessment
            {
                Kldb = bstAssessment.Kldb,
                AssessmentType = AssessmentType.SkillAssessment,
                Description = bstAssessment.,
                Disclaimer = "TODO",
                Duration = TimeSpan.Zero,
                EscoOccupationId = new Uri("http://data.europa.eu/esco/occupation/f2b15a0e-e65a-438a-affb-29b9d50b77d1").ToString(),
                EscoSkills = new List<EscoSkill>(),
                ExternalId = bstAssessment.ItemId,
                Profession = bstAssessment.DescriptionOfProfession,
                Publisher = String.Empty,
                Title = String.Empty,
                Schema = CreateApolloSchema(),
                Ticks = DateTime.Now.Ticks,
            };
        }

        private static Uri CreateApolloSchema()
        {
            return new Uri($"https://invite-apollo.app/{Guid.NewGuid()}");
        }

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
    }
}
