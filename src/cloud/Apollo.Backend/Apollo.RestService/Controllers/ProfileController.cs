using Apollo.Api;
using Apollo.Common.Entities;
using Apollo.RestService.Apollo.Common.Messages;
using Apollo.RestService.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Apollo.Service.Controllers
{
    /// <summary>
    /// Controller handling profile-related operations within the Apollo service.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "ApiKey")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApolloApi _api;
        private readonly ILogger<ProfileController> _logger;

        /// <summary>
        /// Constructor that initializes the ProfileController with required dependencies.
        /// </summary>
        /// <param name="api">ApolloApi instance for profile operations.</param>
        /// <param name="logger">Logger for logging profile controller actions.</param>
        public ProfileController(ApolloApi api, ILogger<ProfileController> logger)
        {
            _api = api;
            _logger = (ILogger<ProfileController>?)logger;
        }

        /// <summary>
        /// Handles HTTP GET requests to retrieve a profile by ID.
        /// </summary>
        /// <param name="id">ID of the profile to retrieve.</param>
        /// <returns>Response containing the retrieved profile.</returns>
        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the retrieved profile.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<GetProfileResponse> GetProfile(string id)
        {
            try
            {
                _logger.LogTrace("Enter {method}", nameof(GetProfile));

                // Call the Apollo API to retrieve a profile by ID.
                var profile = await _api.GetProfile(id);

                _logger.LogTrace("Leave {method}", nameof(GetProfile));

                // Return the retrieved profile as a response.
                return new GetProfileResponse { Profile = profile};
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError(ex, "Method {method} failed", nameof(GetProfile));
                throw;
            }
        }

        /// <summary>
        /// Handles HTTP POST requests to query profiles based on a request object.
        /// </summary>
        /// <param name="req">Query request object specifying profile search criteria.</param>
        /// <returns>Response containing the queried profiles.</returns>
        [HttpPost()]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the queried profiles.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<QueryProfilesResponse> QueryProfiles([FromBody] QueryProfilesRequest req)
        {
            try
            {
                _logger?.LogTrace("Enter {method}", nameof(QueryProfiles));

                // Call the Apollo API to query profiles based on the request.
                var profiles = await _api.QueryProfilesAsync(req);

                _logger?.LogTrace("Leave {method}", nameof(QueryProfiles));

                // Return the queried profiles as a response.
                return new QueryProfilesResponse(profiles);
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError(ex, "Method {method} failed", nameof(QueryProfiles));
                throw;
            }
        }

        /// <summary>
        /// Handles HTTP PUT requests to  update a profile based on a request object.
        /// </summary>
        /// <param name="req">Request object containing profile information for create or update.</param>
        /// <returns>Response containing the result of the create/update operation.</returns>
        [HttpPut]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the result of the create/update operation.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<CreateOrUpdateProfileResponse> CreateOrUpdateProfile([FromBody] CreateOrUpdateProfileRequest req)
        {
            try
            {
                _logger.LogTrace("Enter {method}", nameof(CreateOrUpdateProfile));

                // Call the Apollo API to create or update a profile based on the request.
                var result = await _api.CreateOrUpdateProfile(req.UserId, req.Profile);

                _logger.LogTrace("Leave {method}", nameof(CreateOrUpdateProfile));

                // Return the result of the create/update operation as a response.
                return new CreateOrUpdateProfileResponse { Result = result.FirstOrDefault() };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError(ex, "Method {method} failed", nameof(CreateOrUpdateProfile));
                throw;
            }
        }


        ///// <summary>
        ///// Handles HTTP POST requests to insert multiple profiles.
        ///// </summary>
        ///// <param name="profiles">List of profiles to be inserted.</param>
        ///// <returns>ActionResult indicating the success or failure of the operation.</returns>
        //[HttpPost("insert")]
        //[SwaggerResponse(StatusCodes.Status200OK, "Returns the list of inserted profile IDs.")]
        //[SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input data.")]
        //[SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        //public async Task<string> InsertProfile([FromBody] IList<Profile> profiles)
        //{
        //    try
        //    {
        //        _logger.LogTrace("Enter {method}", nameof(InsertProfile));

        //        if (profiles == null || profiles.Count == 0)
        //        {
        //            throw new ArgumentException("No valid profile provided");
        //        }

        //        var profileIds = new List<string>();
        //        foreach (var profile in profiles)
        //        {
        //            // Generate new ID if not provided
        //            if (string.IsNullOrEmpty(profile.Id))
        //            {
        //                profile.Id = Guid.NewGuid().ToString();
        //            }

        //            await _api.InsertProfileAsync(profile);
        //            profileIds.Add(profile.Id);
        //        }

        //        _logger.LogTrace("Leave {method}", nameof(InsertProfile));

        //        // Return the list of profile IDs
        //        return profileIds;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"{nameof(InsertProfile)} failed: {ex.Message}");
        //        throw; // Rethrow the exception for the middleware to handle
        //    }
        //}

        /// <summary>
        /// Handles HTTP DELETE requests to delete a profile by ID.
        /// </summary>
        /// <param name="id">ID of the profile to delete.</param>
       
        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Profiles deleted successfully.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task DeleteProfile(string[] ids)
        {
            try
            {
                _logger.LogTrace("Enter {method}", nameof(DeleteProfile));

                // Call the Apollo API to delete a profile by ID.
                await _api.DeleteProfiles(ids);

                _logger.LogTrace("Leave {method}", nameof(DeleteProfile));
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError(ex, "Method {method} failed", nameof(DeleteProfile));
                throw;
            }
        }
    }
}
