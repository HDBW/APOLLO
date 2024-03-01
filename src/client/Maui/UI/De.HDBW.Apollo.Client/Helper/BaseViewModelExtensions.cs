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
            var listOfServices = AuthenticatedServiceHelper.GetAuthenticatedServices();
            foreach (var type in listOfServices)
            {
                var method = type.ClassType.GetMethod(nameof(AbstractAuthorizedSwaggerServiceBase.UpdateAuthorizationHeader));
                if (method == null || method.IsStatic)
                {
                    continue;
                }

                var instance = serviceProvider.GetService(type.InterfaceType);
                method?.Invoke(instance, new object?[] { authorizationHeader });
            }
        }
    }
}
