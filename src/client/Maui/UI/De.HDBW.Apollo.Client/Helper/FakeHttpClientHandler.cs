// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Net;
using Apollo.Common.Entities;

namespace De.HDBW.Apollo.Client.Helper
{
    public class FakeHttpClientHandler : HttpClientHandler
    {
        public FakeHttpClientHandler(string baseUrl)
        {
            BaseUrl = new Uri($"{baseUrl?.TrimEnd('/')}/", UriKind.Absolute);
        }

        private Uri BaseUrl { get; }

        private Uri TrainingUrl
        {
            get
            {
                return new Uri(BaseUrl, nameof(Training));
            }
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!(string.Equals(request?.RequestUri?.LocalPath, TrainingUrl.LocalPath) && request?.Method == HttpMethod.Post))
            {
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (!await FileSystem.AppPackageFileExistsAsync(nameof(Training)).ConfigureAwait(false))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var stream = await FileSystem.OpenAppPackageFileAsync(nameof(Training)).ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested)
            {
                stream?.Dispose();
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (!(stream?.CanRead ?? false))
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(stream) };
        }
    }
}
