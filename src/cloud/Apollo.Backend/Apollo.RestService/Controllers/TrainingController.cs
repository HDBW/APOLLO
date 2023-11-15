using Apollo.Api;
using Apollo.Common.Entities;
using Apollo.RestService.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apollo.Service.Controllers
{
    /// <summary>
    /// Implements all operations required to deal with trainings.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "ApiKey")]
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

        /// <summary>
        /// Looks up the training that mathc the given criteria.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IList<QueryTrainingsResponse>> QueryTrainings([FromBody] QueryTrainingsRequest req)
        {
            try
            {
                _logger.LogTrace($"{nameof(QueryTrainings)} entered.");

                // Call the Apollo API to query trainings based on the request.
                var trainings = await _api.QueryTrainings(req);

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

        [HttpPost("insert")]
        public async Task<IActionResult> InsertTrainings([FromBody] ICollection<Training> trainings)
        {
            try
            {
                _logger.LogTrace($"{nameof(InsertTrainings)} entered.");

                if (trainings == null || trainings.Count == 0)
                {
                    // Return a 400 Bad Request with an error message.
                    return BadRequest(new { error = "No valid trainings provided" });
                }

                // Call the Apollo API to insert the provided trainings.
                await _api.CreateOrUpdateTraining(trainings.First());

                _logger.LogTrace($"{nameof(InsertTrainings)} completed.");

                // Return a success response.
                return Ok(new { message = "Trainings inserted successfully" });
            }
            catch (Exception ex)
            {
                // Log and return an error response with a 500 Internal Server Error.
                _logger.LogError($"{nameof(InsertTrainings)} failed: {ex.Message}");
                return StatusCode(500, new { error = "Failed to insert trainings" });
            }
        }



        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            try
            {
                _logger.LogTrace($"{nameof(Delete)} entered.");

                // Assuming you need to pass the ID of the training to delete.
                await _api.DeleteTrainings(new string[] { id });

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
