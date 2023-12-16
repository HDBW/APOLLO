// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface ISheetService
    {
        Task<bool> CloseAsync(BaseViewModel viewModel);

        Task<bool> OpenAsync(string route, CancellationToken token, NavigationParameters? parameters = null);
    }
}
