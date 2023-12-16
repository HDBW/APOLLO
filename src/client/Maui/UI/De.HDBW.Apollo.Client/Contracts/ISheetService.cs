// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface ISheetService
    {
        Task<bool> OpenAsync(string route, CancellationToken token, NavigationParameters? parameters = null);
    }
}
