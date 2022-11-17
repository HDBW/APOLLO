namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IAssessmentDataService
    {

        public Task<IEnumerable<Models.Assessment>> GetAllAssessmentsAsync();

        public Task<IEnumerable<Models.Assessment>> GetAssessmentByIdAsync(long assessmentId);

        public Task<IEnumerable<Models.Assessment>> GetAssessmentByOccupation(string occupation);

        public Task<IQueryable<Models.Assessment>> FindAllAssessmentsAsync();

        public void CreateAssessment(Models.Assessment assessment);

        public void EditAssessmentAsync(Models.Assessment assessment);

        public void DeleteAssessmentAsync(Models.Assessment assessment);
    }
}
