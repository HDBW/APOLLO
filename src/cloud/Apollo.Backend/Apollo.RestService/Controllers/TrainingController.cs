using System.ComponentModel.DataAnnotations;
using Apollo.Api;
using Apollo.Common.Attributes;
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
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested training.", typeof(GetTrainingResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Training not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<GetTrainingResponse> GetTrainingAsync([FromRoute]  string id)
        {
            try
            {
                _logger.LogTrace($"{nameof(GetTrainingAsync)} entered.");

                // Call the Apollo API to retrieve the training with the specified ID.
                var res = await _api.GetTrainingAsync(id);

                _logger.LogTrace($"{nameof(GetTrainingAsync)} completed.");

                // Return the training as a response.
                return new GetTrainingResponse { Training = res };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger.LogError($"{nameof(GetTrainingAsync)} failed: {ex.Message}");
                throw;
            }
        }

        //[HttpGet("filter")]
        //[SwaggerResponse(StatusCodes.Status200OK, "Returns a list of filtered trainings.", typeof(List<Training>))]
        //[SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        //public async Task<ActionResult<List<Training>>> GetTrainingsByDateRange(
        //   [FromQuery, ValidInputString(ErrorMessage = "Invalid training Id.")] string trainingId,
        //   [FromQuery, ValidDateTime(ErrorMessage = "Invalid start date.")] DateTime? startDate,
        //   [FromQuery, ValidDateTime(ErrorMessage = "Invalid end date.")] DateTime? endDate)
        //   {
        //        try
        //        {
        //            //var result = await _api.GetTrainingWithFilteredAppointmentsByIdAndDateRange(trainingId, startDate, endDate);
        //            return Ok(result);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log and return a 500 Internal Server Error response.
        //            return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        //        }
        //    }


        /// <summary>
        /// Looks up the training that match the given criteria.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of queried trainings.", typeof(List<QueryTrainingsResponse>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<QueryTrainingsResponse> QueryTrainingsAsync([FromBody] QueryTrainingsRequest req)
        {
            try
            {
                _logger.LogTrace($"{nameof(QueryTrainingsAsync)} entered.");

                // Call the Apollo API to query trainings based on the request.
                var trainings = await _api.QueryTrainingsAsync(req);

                _logger.LogTrace($"{nameof(QueryTrainingsAsync)} completed.");

                // Return the queried trainings as a response.
                return new QueryTrainingsResponse { Trainings = (List<Training>)trainings };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger.LogError($"{nameof(QueryTrainingsAsync)} failed: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Updates an existing training or creates a new one based on the provided training data.
        /// </summary>
        /// <param name="req">The request containing the training data for creation or update.</param>
        /// <returns>A response containing the updated or created training.</returns>
        /// <response code="200">Returns the updated training.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated training.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<CreateOrUpdateTrainingResponse> CreateOrUpdateTrainingAsync([FromBody] CreateOrUpdateTrainingRequest req)
        {
            try
            {
                _logger?.LogTrace($"{nameof(CreateOrUpdateTrainingAsync)} entered.");

                // Assuming req contains the Training object to create or update.
                var result = await _api.CreateOrUpdateTrainingAsync(new List<Training> { req.Training });

                _logger?.LogTrace($"{nameof(CreateOrUpdateTrainingAsync)} completed.");

                // Return the result of the create/update operation as a response.
                return new CreateOrUpdateTrainingResponse { Training = req.Training };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError($"{nameof(CreateOrUpdateTrainingAsync)} failed: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Inserts a collection of training objects into the system.
        /// </summary>
        /// <param name="trainings">The collection of trainings to be inserted.</param>
        /// <returns>A list of inserted training objects.</returns>
        /// <response code="200">Returns a list of inserted trainings.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("insert")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input data.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of inserted trainings.", typeof(List<Training>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "ErrorCode: 400. Error while inserting the trainings.<br/>ErrorCode: 406. Error while inserting the trainings")]
        public async Task<IList<string>> InsertTrainingsAsync([FromBody] ICollection<Training> trainings)
        {
            try
            {
                _logger.LogTrace($"{nameof(InsertTrainingsAsync)} entered.");

                if (trainings == null || trainings.Count == 0)
                {
                    // Return an empty list if no valid trainings provided.
                    return new List<string>();
                }

                // Call the Apollo API to insert the provided trainings.
                var ids =  await _api.CreateOrUpdateTrainingAsync(new List<Training>(trainings));

                _logger.LogTrace($"{nameof(InsertTrainingsAsync)} completed.");

                // Return the list of inserted trainings.
                return ids;
            }
            catch (Exception ex)
            {
                // Log and return an empty list in case of an error.
                _logger.LogError($"{nameof(InsertTrainingsAsync)} failed: {ex.Message}");
                return new List<string>();
            }
        }


        /// <summary>
        /// Deletes the training with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the training which will be deleted.</param>
        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "ErrorCode: 140. Error while deleting the trainings")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Training deleted successfully.")]
        public async Task DeleteAsync([FromRoute] string id)
        {
            try
            {
                _logger.LogTrace($"{nameof(DeleteAsync)} entered.");

                // Assuming you need to pass the ID of the training to delete.
                await _api.DeleteTrainingsAsync(new string[] { id });

                _logger.LogTrace($"{nameof(DeleteAsync)} completed.");
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger.LogError($"{nameof(DeleteAsync)} failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes all the trainings with the specified Provider Ids in the query.
        /// Return all deleted Training Ids in response
        /// </summary>
        [HttpDelete("DeleteProviderTrainings")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of queried trainings.", typeof(List<DeleteProviderTrainigsResponse>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<DeleteProviderTrainigsResponse> DeleteProviderTrainigsAsync([FromBody] DeleteProviderTrainigsRequest req)
        {
            try
            {
                _logger.LogTrace($"{nameof(DeleteProviderTrainigsAsync)} entered.");

                // Call the Apollo API to delete trainings based on the request.
                var trainings = await _api.DeleteProviderTrainingsAsync(req);

                _logger.LogTrace($"{nameof(DeleteProviderTrainigsAsync)} completed.");

                // Return all deleted Training Ids in response.
                return new DeleteProviderTrainigsResponse { Trainings = trainings.Select(id => new Training { Id = id }).ToList() };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger.LogError($"{nameof(DeleteProviderTrainigsAsync)} failed: {ex.Message}");
                throw;
            }

        }
    }
}
