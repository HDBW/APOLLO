using Apollo.Api;
using Apollo.Common.Entities;
using Apollo.RestService.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Apollo.Service.Controllers
{
    /// <summary>
    /// Implements all operations required to deal with trainings.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private readonly ApolloApi _api;
        private readonly ILogger<TrainingController> _logger;

        public TrainingController(ApolloApi api, ILogger<TrainingController> logger)
        {
            _api = api;
            _logger = logger;
        }

        /// <summary>
        /// Gets the training for the given training identifier.
        /// </summary>
        /// <param name="id">The id of the training to be returned.</param>
        /// <returns>A response containing the requested training.</returns>
        [HttpGet("{id}")]
        public async Task<GetTrainingResponse> GetTraining(string id)
        {
            try
            {
                _logger.LogTrace($"{nameof(GetTraining)} entered.");

                // Call the Apollo API to retrieve the training with the specified ID.
                var res = await _api.GetTraining(id);

                _logger.LogTrace($"{nameof(GetTraining)} completed.");

                // Return the training as a response.
                return new GetTrainingResponse { Training = res };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger.LogError($"{nameof(GetTraining)} failed: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        public async Task<IList<QueryTrainingsResponse>> QueryTrainings([FromBody] QueryTrainingsRequest req)
        {
            try
            {
                _logger.LogTrace($"{nameof(QueryTrainings)} entered.");

                // Create a new QueryTrainings object based on the QueryTrainingsRequest object.
                var queryTrainings = new QueryTrainings
                {
                    Contains = req.Contains,
                    From = req.From,
                    To = req.To
                    // Add more properties as needed for the query.
                };

                // Call the Apollo API to query trainings based on the request.
                var trainings = await _api.QueryTrainings(queryTrainings);

                _logger.LogTrace($"{nameof(QueryTrainings)} completed.");

                // Return the queried trainings as a response.
                return new List<QueryTrainingsResponse> { new QueryTrainingsResponse { Trainings = (List<Training>)trainings } };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger.LogError($"{nameof(QueryTrainings)} failed: {ex.Message}");
                throw;
            }
        }

        [HttpPut]
        public async Task<CreateOrUpdateTrainingResponse> CreateOrUpdateTraining([FromBody] CreateOrUpdateTrainingRequest req)
        {
            try
            {
                _logger.LogTrace($"{nameof(CreateOrUpdateTraining)} entered.");

                // Assuming req contains the Training object to create or update.
                var result = await _api.CreateOrUpdateTraining(req.Training);

                _logger.LogTrace($"{nameof(CreateOrUpdateTraining)} completed.");

                // Return the result of the create/update operation as a response.
                return new CreateOrUpdateTrainingResponse { Training = req.Training };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger.LogError($"{nameof(CreateOrUpdateTraining)} failed: {ex.Message}");
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            try
            {
                _logger.LogTrace($"{nameof(Delete)} entered.");

                // Assuming you need to pass the ID of the training to delete.
                await _api.DeleteTrainings(new int[] { int.Parse(id) });

                _logger.LogTrace($"{nameof(Delete)} completed.");
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger.LogError($"{nameof(Delete)} failed: {ex.Message}");
                throw;
            }
        }
    }
}
