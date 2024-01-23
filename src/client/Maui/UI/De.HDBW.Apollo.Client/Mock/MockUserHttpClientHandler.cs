﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Net.Http.Json;
using System.Text.Json;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using License = Invite.Apollo.App.Graph.Common.Models.UserProfile.License;

namespace De.HDBW.Apollo.Data.Services
{
    public class MockUserHttpClientHandler : HttpClientHandler
    {
        private User? _user;

        private string _userId = $"User_00000000-0000-0000-0000-000000000000";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            switch (request.RequestUri?.AbsolutePath)
            {
                case "/api/User":
                    if (request.Method == HttpMethod.Put)
                    {
                        var jsonContent = request.Content as JsonContent;
                        var userRequest = jsonContent?.Value as CreateOrUpdateUserRequest;
                        var user = JsonSerializer.Deserialize<User>(JsonSerializer.Serialize(userRequest?.User));
                        _user = user!;
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
                                item.Id = $"{nameof(License)}-{Guid.NewGuid()}";
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
                        return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = JsonContent.Create(response) });
                    }

                    return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
                case "/api/User/User_00000000-0000-0000-0000-000000000000":
                    if (_user != null)
                    {
                        var response = new GetUserRespnse();
                        response.User = _user;
                        return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = JsonContent.Create(response) });
                    }

                    return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
                default:
                    return Task.FromResult(new HttpResponseMessage());
            }
        }
    }
}