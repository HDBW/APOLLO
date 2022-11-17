using Invite.Apollo.App.Graph.Assessment.Repository;

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

        public async Task<IEnumerable<Models.Assessment>> GetAllAssessmentsAsync() =>
            await _assessmentRepository.GetAllAsync();

        public async Task<IEnumerable<Models.Assessment>> GetAssessmentByIdAsync(long assessmentId) =>
            await _assessmentRepository.FindByAsync(assessment => assessment.Id == assessmentId);

        public async Task<IEnumerable<Models.Assessment>> GetAssessmentByOccupation(string occupation) =>
            await _assessmentRepository.FindByAsync(assessment => assessment.EscoOccupationId.Equals(occupation));

        public Task<IQueryable<Models.Assessment>> FindAllAssessmentsAsync() => throw new NotImplementedException();

        //TODO: Implement return Assessment
        public void CreateAssessment(Models.Assessment assessment) =>
            _assessmentRepository.Add(assessment);

        public void EditAssessmentAsync(Models.Assessment assessment) => _assessmentRepository.Edit(assessment);
        public void DeleteAssessmentAsync(Models.Assessment assessment) => _assessmentRepository.Delete(assessment);

        #endregion
    }
}
