using System.Reflection;
using System.Text;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace CsvHeadGenerator;

public class Program
{
    public static void Main(string[] args)
    {
        //Course Stuff
        CreateCourseCsv();
        CreateCourseAppointment();
        CreateCourseBenefits();
        CreateCourseEduProvider();
        CreateSimilarCourses();
        CreateCourseModuleItem();
        CreateLoanOptions();
        CreateLearningObjectives();

        //Assessment Stuff
        CreateAssessmentCsv();
        CreateQuestionsCsv();
        CreateAnswersCsv();
        CreateMetaDataCsv();
        CreateQuestionMetaDataCsv();
        CreateAnswersMetaDataCsv();
        CreateMetaDataMetaDataCsv();
        //TODO: Add AssessmentScore
        //TODO: User Profile AssessmentScore

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

    private static void CreateLearningObjectives()
    {
        PropertyInfo[] properties = typeof(CourseLearningObjectives).GetProperties();

        string filename = "CourseLearningObjectives.csv";

        CreateCsV(properties, filename);
    }

    private static void CreateLoanOptions()
    {
        PropertyInfo[] properties = typeof(LoanOption).GetProperties();

        string filename = "CourseLoanOptions.csv";

        CreateCsV(properties, filename);
    }

    private static void CreateCourseModuleItem()
    {
        PropertyInfo[] properties = typeof(CourseModuleItem).GetProperties();

        string filename = "CourseModules.csv";

        CreateCsV(properties, filename);
    }


    private static void CreateSimilarCourses()
    {
        PropertyInfo[] properties = typeof(SimilarCourses).GetProperties();

        string filename = "SimilarCourses.csv";

        CreateCsV(properties, filename);
    }


    private static void CreateCourseEduProvider()
    {
        PropertyInfo[] properties = typeof(EduProviderItem).GetProperties();

        string filename = "EduProviderItem.csv";

        CreateCsV(properties, filename);
    }

    private static void CreateCourseBenefits()
    {
        PropertyInfo[] properties = typeof(CourseBenefits).GetProperties();

        string filename = "CourseBenefits.csv";

        CreateCsV(properties, filename);
    }

    private static void CreateCourseAppointment()
    {
        PropertyInfo[] properties = typeof(CourseAppointment).GetProperties();

        string filename = "CourseAppointment.csv";

        CreateCsV(properties, filename);
    }

    private static void CreateCourseCsv()
    {
        PropertyInfo[] properties = typeof(CourseItem).GetProperties();

        string filename = "CourseItem.csv";

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
        Console.WriteLine($"Created {filename} @ {DateTime.Now.ToUniversalTime()} : With {properties.Length} entries.");
    }


}
