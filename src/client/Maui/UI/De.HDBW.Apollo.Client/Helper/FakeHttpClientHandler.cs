// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Net;
using Apollo.Common.Entities;

namespace De.HDBW.Apollo.Client.Helper
{
    public class FakeHttpClientHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request?.RequestUri?.LocalPath?.Contains("/api/") != true)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            var itemTypeAsString = string.Empty;
            long id = 0;
            string restPath = string.Empty;
            if (request?.Method == HttpMethod.Get)
            {
                if (long.TryParse(request.RequestUri?.Segments?.LastOrDefault(), out long tmpId))
                {
                    id = tmpId;
                }

                if (id > 0)
                {
                    var ind = (request?.RequestUri?.Segments.Count() ?? 0) - 2;
                    if (ind >= 0)
                    {
                        itemTypeAsString = request?.RequestUri?.Segments[ind]?.TrimEnd('/');
                        var fileName = $"{itemTypeAsString}-{id}";
                        restPath = $"{fileName}";
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(restPath))
            {
                itemTypeAsString = request?.RequestUri?.LocalPath.Trim("/api/".ToCharArray());
                restPath = $"{itemTypeAsString}";
            }

            var path = restPath;

            if (!await FileSystem.AppPackageFileExistsAsync(path).ConfigureAwait(false))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            using (var str = await FileSystem.OpenAppPackageFileAsync(path))
            {
                using (StreamReader sr = new StreamReader(str))
                {
                    return await FilterData(sr, itemTypeAsString, request);
                }
            }
        }

        private async Task<HttpResponseMessage> FilterData(StreamReader sr, string? itemTypeAsString, HttpRequestMessage? request)
        {
            if (request?.Content == null)
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new FakeHttpContent(sr.ReadToEnd()) { } };
            }

            var filterStr = (await request.Content.ReadAsStringAsync()) ?? string.Empty;

            Query? query = null;
            if (filterStr != null)
            {
                query = Newtonsoft.Json.JsonConvert.DeserializeObject<Query>(filterStr);
            }

            if (query == null)
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new FakeHttpContent(sr.ReadToEnd()) { } };
            }

            switch (itemTypeAsString)
            {
                case nameof(Training):
                    return FilterTrainingsData(sr.ReadToEnd(), query, request);
            }

            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }

        private HttpResponseMessage FilterTrainingsData(string responseContent, Query query, HttpRequestMessage request)
        {
            var trainings = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Training>>(responseContent);
            if (trainings?.Any() != true || query?.Filter?.Fields?.Any() != true)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            List<Training> trainingResultList = new List<Training>();
            foreach (var training in trainings)
            {
                var trainingFieldDic = MapToDictionary(training);
                if (trainingFieldDic?.Any() != true)
                {
                    continue;
                }

                foreach (var field in query.Filter.Fields)
                {
                    if (field?.Argument?.Any() != true)
                    {
                        continue;
                    }

                    var filterFieldName = field.FieldName?.ToLower();
                    if (string.IsNullOrWhiteSpace(filterFieldName))
                    {
                        continue;
                    }

                    foreach (var trainingField in trainingFieldDic)
                    {
                        if (trainingField.Key?.ToLower()?.Equals(filterFieldName) != true)
                        {
                            continue;
                        }

                        if (field.Argument.OfType<string>().ToList().Exists(f => !string.IsNullOrWhiteSpace(f) && (trainingField.Value ?? string.Empty).ToLower().Contains(f.ToLower())))
                        {
                            trainingResultList.Add(training);
                        }
                    }
                }
            }

            trainings = trainingResultList;
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new FakeHttpContent(Newtonsoft.Json.JsonConvert.SerializeObject(trainings)) { } };
        }

        private Dictionary<string, string?> MapToDictionary(object source)
        {
            var dictionary = new Dictionary<string, string?>();
            MapToDictionaryInternal(dictionary, source);
            return dictionary;
        }

        private void MapToDictionaryInternal(
            Dictionary<string, string?> dictionary, object source)
        {
            var properties = source.GetType().GetProperties();
            foreach (var p in properties)
            {
                var key = p.Name;
                object? value = p.GetValue(source, null);

                if (value == null)
                {
                    continue;
                }

                Type valueType = value.GetType();

                if (valueType.IsPrimitive || valueType == typeof(string))
                {
                    dictionary[key] = value.ToString();
                }
            }
        }
    }
}
