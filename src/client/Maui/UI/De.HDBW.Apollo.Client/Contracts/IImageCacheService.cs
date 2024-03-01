// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IImageCacheService
    {
        Task<(Uri Uri, string? Data)> DownloadAsync(Uri uri, CancellationToken token);
    }
}
