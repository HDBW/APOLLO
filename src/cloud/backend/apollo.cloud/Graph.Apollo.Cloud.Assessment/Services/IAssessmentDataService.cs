namespace Invite.Apollo.App.Graph.Assessment.Services
{
    public interface IAssessmentDataService
    {

        public Task<IEnumerable<Models.Assessment>> GetAllAssessmentsAsync();

        public Task<Models.Assessment> GetAssessmentByIdAsync(long assessmentId);

        public Task<IEnumerable<Models.Assessment>> GetAssessmentsByOccupation(string occupation);

        public void CreateAssessment(Models.Assessment assessment);

        public void EditAssessmentAsync(Models.Assessment assessment);

        public void DeleteAssessmentAsync(Models.Assessment assessment);
    }
}
