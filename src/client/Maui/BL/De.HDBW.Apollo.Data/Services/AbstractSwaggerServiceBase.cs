﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Apollo.Api;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static SQLite.SQLite3;

namespace De.HDBW.Apollo.Data.Services
{
    public abstract class AbstractSwaggerServiceBase
    {
        private static HttpClient? httpClient;

        public AbstractSwaggerServiceBase(
            ILogger logger,
            Uri baseUrl,
            string authKey,
            HttpMessageHandler httpClientHandler)
        {
            ArgumentNullException.ThrowIfNull(logger);
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

        private Uri BaseUri { get; set; }

        protected async Task<TU?> DoGetAsync<TU>(long id, CancellationToken token, [CallerMemberName] string? callerName = null)
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
                using (var response = await client.GetAsync(new Uri($"{BaseUri.OriginalString.TrimEnd('/')}/SER01"), token).ConfigureAwait(false))
                {
                    var responseHeaders = response?.Headers.ToDictionary(k => k.Key, v => v.Value) ?? new Dictionary<string, IEnumerable<string>>();
                    var statusCode = response?.StatusCode ?? HttpStatusCode.InternalServerError;
                    if (statusCode != HttpStatusCode.OK || response == null)
                    {
                        var responseData = await (response?.Content?.ReadAsStringAsync(token) ?? Task.FromResult(string.Empty)).ConfigureAwait(false);
                        var ex = JsonConvert.DeserializeObject<ApolloApiException>(responseData);
                        throw ex ?? new ApolloApiException(-1, "Unknown response.");
                    }

                    result = await ReadObjectResponseAsync<TU>(response, responseHeaders, token).ConfigureAwait(false);
                    return result;
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
                Logger?.LogError(ex, $"Unknown Error in {callerName} from {GetType().Name}.");
                throw;
            }
            finally
            {
            }

            return result;
        }

        protected async Task<TU?> DoPostAsync<TU>(object content, CancellationToken token, [CallerMemberName] string? callerName = null)
        {
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
                Logger?.LogDebug($"#HTTP# #{requestId}# --------->        Start {nameof(DoPostAsync)} {callerName} in {GetType().Name}.");
                using (var response = await client.PostAsJsonAsync(BaseUri, content, token).ConfigureAwait(false))
                {
                    var responseHeaders = response?.Headers.ToDictionary(k => k.Key, v => v.Value) ?? new Dictionary<string, IEnumerable<string>>();
                    var statusCode = response?.StatusCode ?? HttpStatusCode.InternalServerError;
                    if (statusCode != HttpStatusCode.OK || response == null)
                    {
                        var responseData = await (response?.Content?.ReadAsStringAsync(token) ?? Task.FromResult(string.Empty)).ConfigureAwait(false);
                        var ex = JsonConvert.DeserializeObject<ApolloApiException>(responseData);
                        throw ex ?? new ApolloApiException(-1, "Unknown response.");
                    }

                    result = await ReadObjectResponseAsync<TU>(response, responseHeaders, token).ConfigureAwait(false);
                    return result;
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

        private async Task<TU?> ReadObjectResponseAsync<TU>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers, CancellationToken cancellationToken)
        {
            if (response?.Content == null)
            {
                return default(TU);
            }

            try
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<TU>(content);
            }
            catch (JsonException exception)
            {
                throw new ApolloApiException(-3, "Unable to deserialze Json", exception);
            }
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
                Logger.LogError(ex, $"Unknown Error while SetupHttpClient in {GetType().Name}.");
                throw;
            }
        }

        protected struct ObjectResponseResult<TU>
        {
            public ObjectResponseResult(TU? responseObject, string responseText)
            {
                Object = responseObject;
                Text = responseText;
            }

            public TU? Object { get; }

            public string Text { get; }
        }
    }
}