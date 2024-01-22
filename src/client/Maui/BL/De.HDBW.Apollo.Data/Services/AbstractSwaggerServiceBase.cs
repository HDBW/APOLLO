// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace De.HDBW.Apollo.Data.Services
{
    public abstract class AbstractSwaggerServiceBase
    {
        protected static HttpClient? httpClient;

        public AbstractSwaggerServiceBase(
            ILogger logger,
            string baseUrl,
            string authKey,
            HttpMessageHandler httpClientHandler)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentException.ThrowIfNullOrEmpty(baseUrl);
            ArgumentException.ThrowIfNullOrEmpty(authKey);
            ArgumentNullException.ThrowIfNull(httpClientHandler);
            Logger = logger;
            BaseUri = baseUrl;
            if (httpClient == null)
            {
                httpClient = new HttpClient(httpClientHandler);
                SetupHttpClient(authKey);
            }
        }

        protected ILogger Logger { get; private set; }

        protected HttpClient? HttpClient
        {
            get
            {
                return httpClient;
            }
        }

        private string BaseUri { get; set; }

        protected async Task<TU?> DoGetAsync<TU>(string id, CancellationToken token, [CallerMemberName] string? callerName = null)
        {
            token.ThrowIfCancellationRequested();
            TU? result = default;

            var client = HttpClient;
            if (client == null)
            {
                return result;
            }

            try
            {
                using (var response = await client.GetAsync($"{BaseUri}/{id}", token).ConfigureAwait(false))
                {
                    var responseHeaders = response?.Headers.ToDictionary(k => k.Key, v => v.Value) ?? new Dictionary<string, IEnumerable<string>>();
                    var statusCode = response?.StatusCode ?? HttpStatusCode.InternalServerError;
                    if (statusCode != HttpStatusCode.OK || response == null)
                    {
                        var responseData = await (response?.Content?.ReadAsStringAsync(token) ?? Task.FromResult(string.Empty)).ConfigureAwait(false);
                        var ex = JsonConvert.DeserializeObject<ApolloApiException>(responseData);
                        throw ex ?? new ApolloApiException(-1, "Unknown response.");
                    }

                    result = await Deserialize<TU>(response, responseHeaders, token).ConfigureAwait(false);
                    if (result == null)
                    {
                        throw new ApolloApiException(-2, "Unable to read response.");
                    }
                }
            }
            catch (WebException ex)
            {
                Logger?.LogError(ex, $"WebException in {callerName} from {GetType().Name}.");
            }
            catch (HttpRequestException ex)
            {
                Logger?.LogError(ex, $"HttpRequestException in {callerName} from {GetType().Name}.");
            }
            catch (AggregateException ex)
            {
                Logger?.LogError(ex, $"AggregateException in {callerName} from {GetType().Name}.");
            }
            catch (OperationCanceledException ex)
            {
                Logger?.LogInformation(ex, $"Canceled {callerName} from {GetType().Name}.");
                throw;
            }
            catch (ObjectDisposedException ex)
            {
                Logger?.LogInformation(ex, $"Canceled {callerName} from {GetType().Name}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error in {callerName} from {GetType().Name}.");
                throw;
            }
            finally
            {
            }

            return result;
        }

        protected async Task<TU?> DoPostAsync<TU>(object content, CancellationToken token, [CallerMemberName] string? callerName = null, string? action = null)
        {
            ArgumentNullException.ThrowIfNull(content);
            token.ThrowIfCancellationRequested();
            TU? result = default;
            var requestId = Guid.NewGuid().ToString();

            var client = HttpClient;
            if (client == null)
            {
                return result;
            }

#if DEBUG
            var start = DateTime.Now;
            Logger?.LogDebug($"#HTTP# #{requestId}# {start}: Start {callerName} in {GetType().Name}.");
#endif
            try
            {
                Logger?.LogDebug($"#HTTP# #{requestId}# --------->        Start {nameof(DoPostAsync)} with action:{action ?? string.Empty} from {callerName} in {GetType().Name}.");
                var url = BaseUri;
                if (!string.IsNullOrEmpty(action))
                {
                    url = $"{BaseUri.TrimEnd('/')}/{action.TrimStart('/')}";
                }

                using (var response = await client.PostAsJsonAsync(url, content, token).ConfigureAwait(false))
                {
                    var responseHeaders = response?.Headers.ToDictionary(k => k.Key, v => v.Value) ?? new Dictionary<string, IEnumerable<string>>();
                    var statusCode = response?.StatusCode ?? HttpStatusCode.InternalServerError;
                    if (statusCode != HttpStatusCode.OK || response == null)
                    {
                        var responseData = await (response?.Content?.ReadAsStringAsync(token) ?? Task.FromResult(string.Empty)).ConfigureAwait(false);
                        var ex = JsonConvert.DeserializeObject<ApolloApiException>(responseData);
                        throw ex ?? new ApolloApiException(-1, "Unknown response.");
                    }

                    result = await Deserialize<TU>(response, responseHeaders, token).ConfigureAwait(false);
                    if (result == null)
                    {
                        throw new ApolloApiException(-2, "Unable to read response.");
                    }
                }
            }
            catch (WebException ex)
            {
                Logger?.LogError(ex, $"#HTTP# #{requestId}# WebException in {callerName} from {GetType().Name}.");
            }
            catch (HttpRequestException ex)
            {
                Logger?.LogError(ex, $"#HTTP# #{requestId}# HttpRequestException in {callerName} from {GetType().Name}.");
            }
            catch (AggregateException ex)
            {
                Logger?.LogError(ex, $"#HTTP# #{requestId}# AggregateException in {callerName} from {GetType().Name}.");
            }
            catch (OperationCanceledException ex)
            {
                Logger?.LogInformation(ex, $"#HTTP# #{requestId}# Canceled {callerName} from {GetType().Name}.");
                throw;
            }
            catch (ObjectDisposedException ex)
            {
                Logger?.LogInformation(ex, $"#HTTP# #{requestId}# Canceled {callerName} from {GetType().Name}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"#HTTP# #{requestId}# Unknown Error in {callerName} from {GetType().Name}.");
                throw;
            }
            finally
            {
#if DEBUG
                var end = DateTime.Now;
                Logger?.LogDebug($"#HTTP# #{requestId}# {end}: End {callerName} in {GetType().Name}.");
                Logger?.LogDebug($"#HTTP# #{requestId}# Request took {end.Subtract(start).TotalMilliseconds} ms.");
#endif
                Logger?.LogDebug($"#HTTP# #{requestId}# <---------        End {nameof(DoPostAsync)} {callerName} in {GetType().Name}.");
            }

            return result;
        }

        protected async Task<TU?> DoPutAsync<TU>(object content, CancellationToken token, [CallerMemberName] string? callerName = null)
        {
            ArgumentNullException.ThrowIfNull(content);
            token.ThrowIfCancellationRequested();
            TU? result = default;
            var requestId = Guid.NewGuid().ToString();

            var client = HttpClient;
            if (client == null)
            {
                return result;
            }

#if DEBUG
            var start = DateTime.Now;
            Logger?.LogDebug($"#HTTP# #{requestId}# {start}: Start {callerName} in {GetType().Name}.");
#endif
            try
            {
                Logger?.LogDebug($"#HTTP# #{requestId}# --------->        Start {nameof(DoPutAsync)} {callerName} in {GetType().Name}.");
                using (var response = await client.PutAsJsonAsync(BaseUri, content, token).ConfigureAwait(false))
                {
                    var responseHeaders = response?.Headers.ToDictionary(k => k.Key, v => v.Value) ?? new Dictionary<string, IEnumerable<string>>();
                    var statusCode = response?.StatusCode ?? HttpStatusCode.InternalServerError;
                    if (statusCode != HttpStatusCode.OK || response == null)
                    {
                        var responseData = await (response?.Content?.ReadAsStringAsync(token) ?? Task.FromResult(string.Empty)).ConfigureAwait(false);
                        var ex = JsonConvert.DeserializeObject<ApolloApiException>(responseData);
                        throw ex ?? new ApolloApiException(-1, "Unknown response.");
                    }

                    result = await Deserialize<TU>(response, responseHeaders, token).ConfigureAwait(false);
                    if (result == null)
                    {
                        throw new ApolloApiException(-2, "Unable to read response.");
                    }
                }
            }
            catch (WebException ex)
            {
                Logger?.LogError(ex, $"#HTTP# #{requestId}# WebException in {callerName} from {GetType().Name}.");
            }
            catch (HttpRequestException ex)
            {
                Logger?.LogError(ex, $"#HTTP# #{requestId}# HttpRequestException in {callerName} from {GetType().Name}.");
            }
            catch (AggregateException ex)
            {
                Logger?.LogError(ex, $"#HTTP# #{requestId}# AggregateException in {callerName} from {GetType().Name}.");
            }
            catch (OperationCanceledException ex)
            {
                Logger?.LogInformation(ex, $"#HTTP# #{requestId}# Canceled {callerName} from {GetType().Name}.");
                throw;
            }
            catch (ObjectDisposedException ex)
            {
                Logger?.LogInformation(ex, $"#HTTP# #{requestId}# Canceled {callerName} from {GetType().Name}.");
                throw;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"#HTTP# #{requestId}# Unknown Error in {callerName} from {GetType().Name}.");
                throw;
            }
            finally
            {
#if DEBUG
                var end = DateTime.Now;
                Logger?.LogDebug($"#HTTP# #{requestId}# {end}: End {callerName} in {GetType().Name}.");
                Logger?.LogDebug($"#HTTP# #{requestId}# Request took {end.Subtract(start).TotalMilliseconds} ms.");
#endif
                Logger?.LogDebug($"#HTTP# #{requestId}# <---------        End {nameof(DoPutAsync)} {callerName} in {GetType().Name}.");
            }

            return result;
        }

        private async Task<TU?> Deserialize<TU>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers, CancellationToken cancellationToken)
        {
            TU? result = default;

            if (response?.Content == null)
            {
                return result;
            }

            try
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                using (var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false))
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        using (var jsonTextReader = new JsonTextReader(streamReader))
                        {
                            var serializer = new JsonSerializer();
                            result = serializer.Deserialize<TU>(jsonTextReader);
                        }
                    }
                }
            }
            catch (JsonException exception)
            {
                throw new ApolloApiException(-3, "Unable to deserialze Json", exception);
            }

            return result;
        }

        private void SetupHttpClient(string authKey)
        {
            try
            {
                if (httpClient == null)
                {
                    httpClient = new HttpClient();
                }

                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("ApiKey", authKey);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error while SetupHttpClient in {GetType().Name}.");
                throw;
            }
        }
    }
}
