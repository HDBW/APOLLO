// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.Data.Services;

namespace De.HDBW.Apollo.Client.Helper
{
    internal static class BaseViewModelExtensions
    {
        public static void UpdateAuthorizationHeader(this BaseViewModel baseViewModel, IServiceProvider serviceProvider, string? authorizationHeader)
        {
            var authenticatedServices = AuthenticatedServiceHelper.GetAuthenticatedServices();
            AuthenticatedServiceHelper.UpdateAuthorizationHeader(serviceProvider!, authenticatedServices, authorizationHeader);
        }
    }
}
