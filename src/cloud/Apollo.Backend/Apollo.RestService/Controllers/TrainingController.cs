using Apollo.Api;
using Apollo.Common.Entities;
using Apollo.RestService.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="api">Authenticated instance of the API.</param>
        /// <param name="logger"></param>
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
        /// <exception cref="ArgumentNullException">Thrown when the id is null or empty.</exception>"
        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "ErrorCode: 101. Error while querying trainings")]
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
        /// Looks up the training that match the given criteria.
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
                _logger?.LogTrace($"{nameof(CreateOrUpdateTraining)} entered.");

                // Assuming req contains the Training object to create or update.
                var result = await _api.CreateOrUpdateTraining(new List<Training> { req.Training });

                _logger?.LogTrace($"{nameof(CreateOrUpdateTraining)} completed.");

                // Return the result of the create/update operation as a response.
                return new CreateOrUpdateTrainingResponse { Training = req.Training };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError($"{nameof(CreateOrUpdateTraining)} failed: {ex.Message}");
                throw;
            }
        }

        [HttpPost("insert")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input data.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "ErrorCode: 400. Error while inserting the trainings.<br/>ErrorCode: 406. Error while inserting the trainings")]
        public async Task<IList<Training>> InsertTrainings([FromBody] ICollection<Training> trainings)
        {
            try
            {
                _logger.LogTrace($"{nameof(InsertTrainings)} entered.");

                if (trainings == null || trainings.Count == 0)
                {
                    // Return an empty list if no valid trainings provided.
                    return new List<Training>();
                }

                // Call the Apollo API to insert the provided trainings.
                await _api.CreateOrUpdateTraining(new List<Training>(trainings));

                _logger.LogTrace($"{nameof(InsertTrainings)} completed.");

                // Return the list of inserted trainings.
                return trainings.ToList();
            }
            catch (Exception ex)
            {
                // Log and return an empty list in case of an error.
                _logger.LogError($"{nameof(InsertTrainings)} failed: {ex.Message}");
                return new List<Training>();
            }
        }


        /// <summary>
        /// Deletes the training with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the training which will be deleted.</param>
        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "ErrorCode: 140. Error while deleting the trainings")]

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
