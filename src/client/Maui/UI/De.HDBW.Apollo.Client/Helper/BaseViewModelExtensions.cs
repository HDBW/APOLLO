// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;
using De.HDBW.Apollo.Data.Helper;

namespace De.HDBW.Apollo.Client.Helper
{
    internal static class BaseViewModelExtensions
    {
        public static void UpdateAuthorizationHeader(this BaseViewModel baseViewModel, IServiceProvider serviceProvider, string? authorizationHeader)
        {
            AuthenticatedServiceHelper.UpdateAuthorizationHeader(serviceProvider!, authorizationHeader);
        }
    }
}
