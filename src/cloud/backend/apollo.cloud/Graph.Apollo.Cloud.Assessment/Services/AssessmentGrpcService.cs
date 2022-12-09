using System.Collections.ObjectModel;
using AutoMapper;
using Invite.Apollo.App.Graph.Assessment.Data;
using Invite.Apollo.App.Graph.Assessment.Models;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Assessment.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Newtonsoft.Json;
using ProtoBuf;
using ProtoBuf.Grpc;
using System.Collections.Generic;

namespace Invite.Apollo.App.Graph.Assessment.Services;

public class AssessmentGrpcService : IAssessmentGRPCService
{
    private readonly ILogger<AssessmentGrpcService> _logger;
    private readonly IDataService _assessmentDataService;

    private UseCaseCollections _collections;


    public AssessmentGrpcService(ILogger<AssessmentGrpcService> logger, IDataService assessmentDataService)
    {
        _logger = logger;
        _assessmentDataService = assessmentDataService;

        string courseDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\UseCaseCourseData.xlsx");
        System.Console.WriteLine(courseDataFile);
        //UseCaseCourseData useCaseCourseData = new UseCaseCourseData();
        ExcelUseCaseCourse useCaseCourseData = new(courseDataFile);

        //TODO: Assessments müssen noch pro UseCase ausgegeben werden!!!!!!!
        

        for (int i = 1; i < 4; i++)
        {
            _collections = new UseCaseCollections
            {
                //AssessmentCategories = new Collection<AssessmentCategory>(_assessmentDataService.GetAllAssessmentCategoriesAsync().Result.ToList()),
                //AssessmentItems = new Collection<AssessmentItem>(_assessmentDataService.GetAllAssessmentItemsAsync().Result.ToList()),
                //AnswerItems = new Collection<AnswerItem>(_assessmentDataService.GetAllAnswerItemsAsync().Result.ToList()),
                //AnswerMetaDataRelations = new Collection<AnswerMetaDataRelation>(_assessmentDataService.GetAllAnswerMetaDataRelationsAsync().Result.ToList()),
                //MetaDataItems = new Collection<MetaDataItem>(_assessmentDataService.GetAllMetaDataItemsAsync().Result.ToList()),
                //MetaDataMetaDataRelations = new Collection<MetaDataMetaDataRelation>(_assessmentDataService.GetAllMetaDataMetaDataRelationsAsync().Result.ToList()),
                //QuestionItems = new Collection<QuestionItem>(_assessmentDataService.GetAllQuestionItemsAsync().Result.ToList()),
                //QuestionMetaDataRelations = new Collection<QuestionMetaDataRelation>(_assessmentDataService.GetAllQuestionMetaDataRelationsAsync().Result.ToList()),
                //CategoryRecomendations = new Collection<CategoryRecomendationItem>(GetAllCategoryRecomendationsByUseCaseId(i)),
                ////Course
                //CourseItems = new Collection<CourseItem>(GetAllCourseItems(i, useCaseCourseData)),
                //CourseContacts = new Collection<CourseContact>(GetAllCourseContacts(i, useCaseCourseData)),
                //EduProviderItems = new Collection<EduProviderItem>(GetAllEduProviderByUseCaseId(i, useCaseCourseData)),
                //CourseAppointments = new Collection<CourseAppointment>(GetAllAppointments(i, useCaseCourseData)),
                //CourseContactRelations = new Collection<CourseContactRelation>(GetAllCourseContactsRelations(i, useCaseCourseData))



                AssessmentCategories = new Collection<AssessmentCategory>(_assessmentDataService.GetAssessmentCategoriesByUseCase(i).ToList()),
                AssessmentItems = new Collection<AssessmentItem>(_assessmentDataService.GetAssessmentItemsByUseCase(i).ToList()),
                AnswerItems = new Collection<AnswerItem>(_assessmentDataService.GetAnswersItemsByUseCase(i).ToList()),
                AnswerMetaDataRelations = new Collection<AnswerMetaDataRelation>(_assessmentDataService.GetAnswerMetaDataRelationByUseCase(i).ToList()),
                MetaDataItems = new Collection<MetaDataItem>(_assessmentDataService.GetMetaDataItemsByUseCase(i).ToList()),
                MetaDataMetaDataRelations = new Collection<MetaDataMetaDataRelation>(_assessmentDataService.GetMetaDataMetaDataRelationsByUseCase(i).ToList()),
                QuestionItems = new Collection<QuestionItem>(_assessmentDataService.GetQuestionItemsByUseCase(i).ToList()),
                QuestionMetaDataRelations = new Collection<QuestionMetaDataRelation>(_assessmentDataService.GetQuestionMetaDataRelationByUseCase(i).ToList()),
                CategoryRecomendations = new Collection<CategoryRecomendationItem>(_assessmentDataService.GetCategoryRecomendationItemsByUseCase(i).ToList()),
                //Course
                CourseItems = new Collection<CourseItem>(GetAllCourseItems(i, useCaseCourseData)),
                CourseContacts = new Collection<CourseContact>(GetAllCourseContacts(i, useCaseCourseData)),
                EduProviderItems = new Collection<EduProviderItem>(GetAllEduProviderByUseCaseId(i, useCaseCourseData)),
                CourseAppointments = new Collection<CourseAppointment>(GetAllAppointments(i, useCaseCourseData)),
                CourseContactRelations = new Collection<CourseContactRelation>(GetAllCourseContactsRelations(i, useCaseCourseData)),
                //Results for FAKE Assessments
                AssessmentCategoryResults = CreateFakeCategoryResults(i),
                AnswerItemResults = CreateFakeAnwerResults(i)




        };

            using (StreamWriter file = File.CreateText(@"devtest"+i+".json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, _collections);
            }

            string filename = "usecase"+i+".bin";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, _collections);
                file.Close();
            }
        }

        
    }

    private Collection<AnswerItemResult> CreateFakeAnwerResults(int useCase)
    {
        Collection <AnswerItemResult> answerItems = new();
        switch (useCase)
        {
            case 1:
                break;
            case 2:
                
                //TODO: RESULT STUFF
                //var skillAssessment = 
                //_assessmentDataService.GetAllAssessmentsAsync().Result
                //    .Where(a => a.Title.Equals("Test Kaufmann/Kauffrau im E-Commerce")).First();
                //skillAssessment.
                var assessments = _assessmentDataService.GetAllAssessmentsAsync().Result.Where(a => a.UseCaseId.Equals(2));

                foreach (Models.Assessment assessment in assessments)
                {
                    if (!assessment.AssessmentType.Equals(AssessmentType.SoftSkillAssessment))
                    {
                        //EXPERIENCE Kaufmann/Kauffrau im E-Commerce
                        //SKILLASSESSMENT Kaufmann/Kauffrau im E-Commerce
                        AnswerItemResult air = new AnswerItemResult();
                        air.Id = answerItems.Count;
                        air.AssessmentItemId = assessment.Id;
                        air.QuestionItemId = assessment.Questions.FirstOrDefault()!.Id;
                        air.AnswerItemId = assessment.Questions.FirstOrDefault()!.Answers.FirstOrDefault()!.Id;
                        air.Schema = CreateApolloSchema();
                        air.Ticks = DateTime.Now.Ticks;
                        air.Value = false.ToString();
                        air.UserProfileId = 1;

                        answerItems.Add(air);
                    }
                }

                
                break;
            case 3:
                //Soziale & Kommunikative Kompetenzen
                var assessmentsCase3 = _assessmentDataService.GetAllAssessmentsAsync().Result.Where(a => a.UseCaseId.Equals(3));

                foreach (Models.Assessment assessment in assessmentsCase3)
                {
                    AnswerItemResult air = new AnswerItemResult();
                    air.Id = answerItems.Count;
                    air.AssessmentItemId = assessment.Id;
                    air.QuestionItemId = assessment.Questions.FirstOrDefault()!.Id;
                    air.AnswerItemId = assessment.Questions.FirstOrDefault()!.Answers.FirstOrDefault()!.Id;
                    air.Schema = CreateApolloSchema();
                    air.Ticks = DateTime.Now.Ticks;
                    air.Value = false.ToString();
                    air.UserProfileId = 1;

                    answerItems.Add(air);
                }

                break;
            default:
                break;
        }

        return answerItems;
    }

    private static Uri CreateApolloSchema()
    {
        return new Uri($"https://invite-apollo.app/{Guid.NewGuid()}");
    }

    private Collection<AssessmentCategoryResult> CreateFakeCategoryResults(int useCase)
    {
        Collection<AssessmentCategoryResult> answerItems = new();
        switch (useCase)
        {
            case 1:
                break;
            case 2:
                //TODO: RESULT STUFF
                //SKILLASSESSMENT Kaufmann/Kauffrau im E-Commerce
                //EXPERIENCE Kaufmann/Kauffrau im E-Commerce
                break;
            case 3:
                //Soziale & Kommunikative Kompetenzen
                break;
            default:
                break;
        }

        return answerItems;
    }


    private List<EduProviderItem> GetAllEduProviderByUseCaseId(int i, ExcelUseCaseCourse useCaseCourseData)
    {
        return useCaseCourseData.ProviderList.Values.ToList();
    }

    private List<CourseContactRelation> GetAllCourseContactsRelations(int useCase, ExcelUseCaseCourse useCaseCourseData)
    {

        
        //TODO: Implement useCase
        return useCaseCourseData.CourseContactRelationByUseCaseId[useCase];
    }

    private List<CourseContact> GetAllCourseContacts(int useCase, ExcelUseCaseCourse useCaseCourseData)
    {
        //TODO: Implement a query for useCases
        return useCaseCourseData.ContactsByUseCaseId[useCase].Distinct().ToList();
    }

    private List<CourseAppointment> GetAllAppointments(int useCase, ExcelUseCaseCourse useCaseCourseData)
    {
        //var config = new MapperConfiguration(cfg => cfg.CreateMap<Appointment, CourseAppointment>());
        //Mapper mapper = new(config);
        //List<CourseAppointment> list = new();
        //foreach (var item in useCaseCourseData.Appointments)
        //{
        //    list.Add(mapper.Map<CourseAppointment>(item));
        //}

        //return list;

        //TODO: a query to determine the usecases would be awesome?
        //return useCaseCourseData.Appointments.Values.ToList();
        
        return useCaseCourseData.AppointmentsByUseCaseId[useCase].Distinct().ToList();
    }

    //private IList<CourseContact> GetAllCourseContacts(int useCase, UseCaseCourseData useCaseCourseData)
    //{
    //    var config = new MapperConfiguration(cfg => cfg.CreateMap<CourseContact, CourseContact>());
    //    Mapper mapper = new(config);
    //    List<CourseContact> courseItems = new();
    //    foreach (var course in useCaseCourseData.useCaseContacts[useCase])
    //    {
    //        courseItems.Add(mapper.Map<CourseContact>(course));
    //    }

    //    return courseItems;
    //}

    public List<CourseItem> GetAllCourseItems(int useCase, ExcelUseCaseCourse useCaseCourseData)
    {
        var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseItem>());
        Mapper mapper = new(config);
        List<CourseItem> courseItems = new();
        courseItems.AddRange(useCaseCourseData.CoursesByUseCaseId[useCase]);
        return courseItems;
    }


    public ValueTask<AssessmentResponse> GetAssessmentsAsync(AssessmentRequest request)
    {
        AssessmentResponse response = new AssessmentResponse
            {
                Assessments = _collections.AssessmentItems, CorrelationId = request.CorrelationId
            };
        return new ValueTask<AssessmentResponse>(response);
    }

    public async ValueTask<AnswerResponse> GetAnswersAsync(AnswersRequest request) => throw new NotImplementedException();

    public async ValueTask<AnswerResponse> GetAnswersAsync(AnswersRequest request, CallContext context = default) => throw new NotImplementedException();

    public async ValueTask<AnswerResponse> GetAnswersAsync(AnswersRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public async ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request) => throw new NotImplementedException();

    public async ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request, CallContext context = default) => throw new NotImplementedException();

    public async ValueTask<QuestionResponse> GetQuestionsAsync(QuestionRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
