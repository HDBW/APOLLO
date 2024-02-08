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
        /// Returns the set of ApolloList items that matches specified filter.
        /// </summary>
        /// <param name="req">The request that filters items..</param>
        /// <returns>A response containing the list of items..</returns>
        /// <response code="200">List of ApolloList items is returned.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost()]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the set of ApolloList items that matches specified filter.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<GetListResponse> GetListAsync([FromBody] GetListRequest req)
        {
            try 
            {
                _logger?.LogTrace($"{nameof(GetListAsync)} entered.");

                // Assuming req contains the Training object to create or update.
                var result = await _api.GetListAsync(itemType: req.ItemType);

                _logger?.LogTrace($"{nameof(GetListAsync)} completed.");

                // Return the result of the create/update operation as a response.
                return new GetListResponse { Result = result };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError($"{nameof(GetListAsync)} failed: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Create item like Qualification if there is no ID on request or Updates an existing Qualifications List if there is ID.
        /// </summary>
        /// <param name="req">The request containing the data for creation or update list.</param>
        /// <returns>A response containing the updated or created List.</returns>
        /// <response code="200">Returns the updated training.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("query")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the set of List items that matches specified filter.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<QueryListResponse> QueryListItemsAsync([FromBody] QueryListRequest req)
        {
            try
            {
                _logger?.LogTrace($"{nameof(QueryListItemsAsync)} entered.");

                // Assuming req contains the Training object to create or update.
                var result = await _api.QueryListItemsAsync(req.Language, req.ItemType, req.Contains);

                _logger?.LogTrace($"{nameof(QueryListItemsAsync)} completed.");

                // Return the result of the create/update operation as a response.
                return new QueryListResponse { Result = result };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError($"{nameof(QueryListItemsAsync)} failed: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Create item like Qualification if there is no ID on request or Updates an existing Qualifications List if there is ID.
        /// </summary>
        /// <param name="req">The request containing the data for creation or update list.</param>
        /// <returns>A response containing the updated or created List.</returns>
        /// <response code="200">Returns the updated training.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut()]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated List items.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<CreateOrUpdateListResponse> CreateOrUpdateListAsync([FromBody] CreateOrUpdateListRequest req)
        {         
            try
            {
                _logger?.LogTrace($"{nameof(CreateOrUpdateListAsync)} entered.");

                // Assuming req contains the Training object to create or update.
                var result = await _api.CreateOrUpdateListAsync(req.List);

                _logger?.LogTrace($"{nameof(CreateOrUpdateListAsync)} completed.");

                // Return the result of the create/update operation as a response.
                return new CreateOrUpdateListResponse { Result = req.List };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError($"{nameof(CreateOrUpdateListAsync)} failed: {ex.Message}");
                throw;
            }
        }




    }
}
