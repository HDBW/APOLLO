using Apollo.Api;
using Apollo.Common.Entities;
using Apollo.RestService.Apollo.Common.Messages;
using Apollo.RestService.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Apollo.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApolloApi _api;
        private readonly ILogger<UserController> _logger;

        // Constructor that initializes the UserController with required dependencies.
        public UserController(ApolloApi api, ILogger<UserController> logger)
        {
            _api = api;
            _logger = logger;
        }

        [HttpGet("{id}")]
        // Handles HTTP GET requests to retrieve a user by ID.
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

        [HttpPost()]
        // Handles HTTP POST requests to query users based on a request object.
        public async Task<QueryUsersResponse> QueryUsers([FromBody] QueryTrainings req)
        {
            try
            {
                _logger.LogTrace("Enter {method}", nameof(QueryUsers));

                // Call the Apollo API to query users based on the request.
                var users = await _api.QueryUser(req);

                _logger.LogTrace("Leave {method}", nameof(QueryUsers));

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

        [HttpPut]
        // Handles HTTP PUT requests to create or update a user based on a request object.
        public async Task<CreateOrUpdateUserResponse> CreateOrUpdateUser([FromBody] CreateOrUpdateUserRequest req)
        {
            try
            {
                _logger.LogTrace("Enter {method}", nameof(CreateOrUpdateUser));

                // Call the Apollo API to create or update a user based on the request.
                var result = await _api.CreateOrUpdateUser(req.User);

                _logger.LogTrace("Leave {method}", nameof(CreateOrUpdateUser));

                // Return the result of the create/update operation as a response.
                return new CreateOrUpdateUserResponse { Result = result };
            }
            catch (Exception ex)
            {
                // Log and re-throw any exceptions encountered.
                _logger?.LogError(ex, "Method {method} failed", nameof(CreateOrUpdateUser));
                throw;
            }
        }

        [HttpPost("insert")]
        // Handles HTTP POST requests to insert multiple users.
        public async Task<IActionResult> InsertUsers([FromBody] IList<User> users)
        {
            try
            {
                _logger.LogTrace("Enter {method}", nameof(InsertUsers));

                if (users == null || users.Count == 0)
                {
                    // Return a 400 Bad Request with an error message.
                    return BadRequest(new { error = "No valid users provided" });
                }

                // Call the Apollo API to insert the provided users.
                await _api.InsertUsers(users);

                _logger.LogTrace("Leave {method}", nameof(InsertUsers));

                // Return a success response.
                return Ok(new { message = "Users inserted successfully" });
            }
            catch (Exception ex)
            {
                // Log and return an error response with a 500 Internal Server Error.
                _logger.LogError($"{nameof(InsertUsers)} failed: {ex.Message}");
                return StatusCode(500, new { error = "Failed to insert users" });
            }
        }

        [HttpDelete("{id}")]
        // Handles HTTP DELETE requests to delete a user by ID.
        public async Task DeleteUser(int[] id)
        {
            try
            {
                _logger.LogTrace("Enter {method}", nameof(DeleteUser));

                // Call the Apollo API to delete a user by ID.
                await _api.DeleteUser(id);

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
