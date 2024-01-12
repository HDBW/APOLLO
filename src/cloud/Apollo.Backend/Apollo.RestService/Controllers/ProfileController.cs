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
    /// Controller handling user-related operations within the Apollo service.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "ApiKey")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApolloApi _api;
        private readonly ILogger<ProfileController> _logger;

        /// <summary>
        /// Constructor that initializes the UserController with required dependencies.
        /// </summary>
        /// <param name="api">ApolloApi instance for user operations.</param>
        /// <param name="logger">Logger for logging user controller actions.</param>
        public ProfileController(ApolloApi api, ILogger<ProfileController> logger)
        {
            _api = api;
            _logger = logger;
        }

        /// <summary>
        /// Handles HTTP GET requests to retrieve a user by ID.
        /// </summary>
        /// <param name="id">ID of the user to retrieve.</param>
        /// <returns>Response containing the retrieved user.</returns>
        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the retrieved user.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<GetUserResponse> GetUser(string id)
        {
            try
            {
                _logger.LogTrace("Enter {method}", nameof(GetUser));

                // Call the Apollo API to retrieve a user by ID.
                var user = await _api.GetUser(id);

                _logger.LogTrace("Leave {method}", nameof(GetUser));

                // Return the retrieved user as a response.
                return new GetUserResponse { User = user };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError(ex, "Method {method} failed", nameof(GetUser));
                throw;
            }
        }

        /// <summary>
        /// Handles HTTP POST requests to query users based on a request object.
        /// </summary>
        /// <param name="req">Query request object specifying user search criteria.</param>
        /// <returns>Response containing the queried users.</returns>
        [HttpPost()]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the queried users.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<QueryUsersResponse> QueryUsers([FromBody] QueryUsersRequest req)
        {
            try
            {
                _logger?.LogTrace("Enter {method}", nameof(QueryUsers));

                // Call the Apollo API to query users based on the request.
                var users = await _api.QueryUsers(req);

                _logger?.LogTrace("Leave {method}", nameof(QueryUsers));

                // Return the queried users as a response.
                return new QueryUsersResponse(users);
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError(ex, "Method {method} failed", nameof(QueryUsers));
                throw;
            }
        }

        /// <summary>
        /// Handles HTTP PUT requests to create or update a user based on a request object.
        /// </summary>
        /// <param name="req">Request object containing user information for create or update.</param>
        /// <returns>Response containing the result of the create/update operation.</returns>
        [HttpPut]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the result of the create/update operation.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<CreateOrUpdateUserResponse> CreateOrUpdateUser([FromBody] CreateOrUpdateUserRequest req)
        {
            try
            {
                _logger.LogTrace("Enter {method}", nameof(CreateOrUpdateUser));

                // Call the Apollo API to create or update a user based on the request.
                var users = new List<User> { req.User };
                var result = await _api.CreateOrUpdateUser(users);

                _logger.LogTrace("Leave {method}", nameof(CreateOrUpdateUser));

                // Return the result of the create/update operation as a response.
                return new CreateOrUpdateUserResponse { Result = result.FirstOrDefault() };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError(ex, "Method {method} failed", nameof(CreateOrUpdateUser));
                throw;
            }
        }


        /// <summary>
        /// Handles HTTP POST requests to insert multiple users.
        /// </summary>
        /// <param name="users">List of users to be inserted.</param>
        /// <returns>ActionResult indicating the success or failure of the operation.</returns>
        [HttpPost("insert")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the list of inserted user IDs.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input data.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IList<string>> InsertUsers([FromBody] IList<User> users)
        {
            try
            {
                _logger.LogTrace("Enter {method}", nameof(InsertUsers));

                if (users == null || users.Count == 0)
                {
                    throw new ArgumentException("No valid users provided");
                }

                var userIds = new List<string>();
                foreach (var user in users)
                {
                    // Generate new ID if not provided
                    if (string.IsNullOrEmpty(user.Id))
                    {
                        user.Id = Guid.NewGuid().ToString();
                    }

                    await _api.InsertUser(user);
                    userIds.Add(user.Id);
                }

                _logger.LogTrace("Leave {method}", nameof(InsertUsers));

                // Return the list of user IDs
                return userIds;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(InsertUsers)} failed: {ex.Message}");
                throw; // Rethrow the exception for the middleware to handle
            }
        }

        /// <summary>
        /// Handles HTTP DELETE requests to delete a user by ID.
        /// </summary>
        /// <param name="id">ID of the user to delete.</param>
        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Users deleted successfully.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task DeleteUser(string[] ids)
        {
            try
            {
                _logger.LogTrace("Enter {method}", nameof(DeleteUser));

                // Call the Apollo API to delete a user by ID.
                await _api.DeleteUsers(ids);

                _logger.LogTrace("Leave {method}", nameof(DeleteUser));
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError(ex, "Method {method} failed", nameof(DeleteUser));
                throw;
            }
        }
    }
}
