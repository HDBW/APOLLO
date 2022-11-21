using AutoMapper;
using Invite.Apollo.App.Graph.Assessment.Repository;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public class AssessmentDataService : IAssessmentDataService
    {
        //Implement logger
        private readonly ILogger<AssessmentDataService> _logger;
        private readonly IAssessmentRepository _assessmentRepository;

        public AssessmentDataService(ILogger<AssessmentDataService> logger, IAssessmentRepository assessmentRepository)
        {
            _logger = logger;
            _assessmentRepository = assessmentRepository;
        }

        //TODO: Do the DTO Mapping here
        //TODO: https://victorakpan.com/blog/ef-core-inmemory-and-dependency-injection-in-console-app

        #region Implementation of IAssessmentDataService

        public async Task<IEnumerable<AssessmentItem>> GetAllAssessmentItemsAsync()
        {
            var assessments = await _assessmentRepository.GetAllAsync();
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Models.Assessment, AssessmentItem>()
            );
            var mapper = new Mapper(config);
            List<AssessmentItem> assessmentItems = new List<AssessmentItem>();
            foreach (var assessment in assessments)
            {
                assessmentItems.Add(mapper.Map<AssessmentItem>(assessment));
            }

            return assessmentItems;
        }

        public async Task<IEnumerable<Models.Assessment>> GetAllAssessmentsAsync() =>
            await _assessmentRepository.GetAllAsync();

        public async Task<AssessmentItem> GetAssessmentItemByIdAsync(long assessmentId)
        {
            var assessment = await _assessmentRepository.GetSingleAsync(assessmentId);
            //var assessment = await _assessmentRepository.FindByAsync(a => a.Id.Equals(assessmentId));
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Models.Assessment, AssessmentItem>()
            );
            var mapper = new Mapper(config);
            return mapper.Map<AssessmentItem>(assessment);
            
        }

        public async Task<Models.Assessment> GetAssessmentByIdAsync(long assessmentId)
        {
            return await _assessmentRepository.GetSingleAsync(assessmentId);
        }


        public async Task<IEnumerable<Models.Assessment>> GetAssessmentsByOccupation(string occupation) =>
            await _assessmentRepository.FindByAsync(assessment => assessment.EscoOccupationId.Equals(occupation));

        //TODO: Implement return Assessment
        public void CreateAssessment(Models.Assessment assessment) =>
            _assessmentRepository.Add(assessment);

        public void EditAssessmentAsync(Models.Assessment assessment)
        {
            _assessmentRepository.Edit(assessment);
            _assessmentRepository.Commit();

        }

        public void DeleteAssessmentAsync(Models.Assessment assessment)
        {
            _assessmentRepository.Delete(assessment);
            //TODO: Configure Delete 
            _assessmentRepository.Commit();
        }

        #endregion
    }
}
