// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Net.Http.Json;
using System.Text.Json;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using License = Invite.Apollo.App.Graph.Common.Models.UserProfile.License;

namespace De.HDBW.Apollo.Data.Services
{
    public class MockUserHttpClientHandler : HttpClientHandler
    {
        private User? _user;

        private string _userId = $"User_00000000-0000-0000-0000-000000000000";
        private IUserRepository? _userRepository;

        public MockUserHttpClientHandler(IUserRepository? userRepository)
        {
            _userRepository = userRepository;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
        {
            switch (request.RequestUri?.AbsolutePath)
            {
                case "/api/User":
                    if (request.Method == HttpMethod.Put)
                    {
                        User? existingUser = null;
                        if (_user == null)
                        {
                            existingUser = await (_userRepository?.GetItemAsync(token) ?? Task.FromResult<User?>((User?)null)).ConfigureAwait(false);
                        }

                        var jsonContent = request.Content as JsonContent;
                        var userRequest = jsonContent?.Value as CreateOrUpdateUserRequest;
                        var user = userRequest?.User.Serialize()!.Deserialize<User>();
                        _user = existingUser ?? user!;
                        _user.Id = _userId;
                        foreach (var contact in _user.ContactInfos)
                        {
                            if (contact.Id == null)
                            {
                                contact.Id = $"{nameof(Contact)}-{Guid.NewGuid()}";
                            }
                        }

                        foreach (var item in _user.Profile?.LanguageSkills ?? new List<Language>())
                        {
                            if (item.Id == null)
                            {
                                item.Id = $"{nameof(Language)}-{Guid.NewGuid()}";
                            }
                        }

                        foreach (var item in _user.Profile?.Qualifications ?? new List<Qualification>())
                        {
                            if (item.Id == null)
                            {
                                item.Id = $"{nameof(Qualification)}-{Guid.NewGuid()}";
                            }
                        }

                        foreach (var item in _user.Profile?.Licenses ?? new List<License>())
                        {
                            if (item.Id == null)
                            {
                                item.Id = $"{nameof(License)}-{Guid.NewGuid()}";
                            }
                        }

                        foreach (var item in _user.Profile?.CareerInfos ?? new List<CareerInfo>())
                        {
                            if (item.Id == null)
                            {
                                item.Id = $"{nameof(CareerInfo)}-{Guid.NewGuid()}";
                            }
                        }

                        foreach (var item in _user.Profile?.EducationInfos ?? new List<EducationInfo>())
                        {
                            if (item.Id == null)
                            {
                                item.Id = $"{nameof(EducationInfo)}-{Guid.NewGuid()}";
                            }
                        }

                        foreach (var item in _user.Profile?.WebReferences ?? new List<WebReference>())
                        {
                            if (item.Id == null)
                            {
                                item.Id = $"{nameof(WebReference)}-{Guid.NewGuid()}";
                            }
                        }

                        var response = new CreateOrUpdateUserResponse();
                        response.Result = _userId;
                        return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = JsonContent.Create(response, options: SerializationHelper.Options) };
                    }

                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                case "/api/User/User_00000000-0000-0000-0000-000000000000":
                    if (_user != null)
                    {
                        var response = new GetUserRespnse();
                        response.User = _user;
                        return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = JsonContent.Create(response, options: SerializationHelper.Options) };
                    }

                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                default:
                    return new HttpResponseMessage();
            }
        }
    }
}
