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
    /// Implements all operations required to deal with List items.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "ApiKey")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly ApolloApi _api;
        private readonly ILogger<ListController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="api">Authenticated instance of the API.</param>
        /// <param name="logger"></param>
        public ListController(ApolloApi api, ILogger<ListController> logger)
        {
            _api = api;
            _logger = logger;
        }


       


        /// <summary>
        /// Create item like Qualification if there is no ID on request or Updates an existing Qualifications List if there is ID.
        /// </summary>
        /// <param name="req">The request containing the data for creation or update list.</param>
        /// <returns>A response containing the updated or created List.</returns>
        /// <response code="200">Returns the updated training.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated List items.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<CreateOrUpdateListResponse> CreateOrUpdateQualificationAsync([FromBody] CreateOrUpdateListRequest req)
        {
            try
            {
                _logger?.LogTrace($"{nameof(CreateOrUpdateQualificationAsync)} entered.");

                // Assuming req contains the Training object to create or update.
                var result = await _api.CreateOrUpdateQualificationAsync(new List<List> { req.List });

                _logger?.LogTrace($"{nameof(CreateOrUpdateQualificationAsync)} completed.");

                // Return the result of the create/update operation as a response.
                return new CreateOrUpdateListResponse { List = req.List };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError($"{nameof(CreateOrUpdateQualificationAsync)} failed: {ex.Message}");
                throw;
            }
        }




    }
}
