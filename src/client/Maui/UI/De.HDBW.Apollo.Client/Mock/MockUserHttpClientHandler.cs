using System.Net.Http.Json;
using System.Text.Json;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.Data.Services
{
    public class MockUserHttpClientHandler : HttpClientHandler
    {
        private User? _user;

        private string _userId = $"User_00000000-0000-0000-0000-000000000000";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            switch (request.RequestUri?.AbsolutePath)
            {
                case "/api/User":
                    if (request.Method == HttpMethod.Put)
                    {
                        var jsonContent = request.Content as JsonContent;
                        var userRequest = jsonContent?.Value as CreateOrUpdateUserRequest;
                        var user = JsonSerializer.Deserialize<User>(JsonSerializer.Serialize(userRequest?.User));
                        _user = user;
                        _user.Id = _userId;
                        foreach (var contact in _user.ContactInfos)
                        {
                            if (contact.Id == null)
                            {
                                contact.Id = $"{nameof(Contact)}-{Guid.NewGuid()}";
                            }
                        }

                        var response = new CreateOrUpdateUserResponse();
                        response.Result = _userId;
                        return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = JsonContent.Create(response) };
                    }

                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                case "/api/User/User_00000000-0000-0000-0000-000000000000":
                    if (_user != null)
                    {
                        var response = new GetUserRespnse();
                        response.User = _user;
                        return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = JsonContent.Create(response) };
                    }

                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                default:
                    return new HttpResponseMessage();
            }
        }
    }
}
