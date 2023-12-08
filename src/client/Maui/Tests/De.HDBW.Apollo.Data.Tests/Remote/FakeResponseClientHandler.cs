using System.Net;
using De.HDBW.Apollo.Data.Extensions;
using De.HDBW.Apollo.Data.Tests.Helper;
using Invite.Apollo.App.Graph.Common.Models;

namespace De.HDBW.Apollo.Data.Tests.Remote
{
    public class FakeResponseClientHandler : HttpMessageHandler
    {
        private readonly string? _baseDirectory;

        public FakeResponseClientHandler()
        {
            _baseDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_baseDirectory) || request?.RequestUri?.LocalPath?.Contains("/api/") != true)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            var itemTypeAsString = string.Empty;
            long id = 0;
            string restPath = string.Empty;
            string resourcesDir = "Resources";
            if (request?.Method == HttpMethod.Get)
            {
                id = request.RequestUri?.Segments?.LastOrDefault()?.TryToLong() ?? 0;
                if (id > 0)
                {
                    var ind = (request?.RequestUri?.Segments.Count() ?? 0) - 2;
                    if (ind >= 0)
                    {
                        itemTypeAsString = request?.RequestUri?.Segments[ind]?.TrimEnd('/');
                        var fileName = $"{itemTypeAsString}-{id}";
                        restPath = $"{resourcesDir}\\{fileName}";
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(restPath))
            {
                itemTypeAsString = request?.RequestUri?.LocalPath.Trim("/api/".ToCharArray());
                restPath = $"{resourcesDir}\\{itemTypeAsString}";
            }

            var path = Path.Combine(_baseDirectory, restPath);

            if (!File.Exists(path))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            using (StreamReader sr = new StreamReader(path))
            {
                return await FilterData(sr, itemTypeAsString, request);
            }
        }

        private async Task<HttpResponseMessage> FilterData(StreamReader sr, string? itemTypeAsString, HttpRequestMessage? request)
        {
            if (request?.Content == null)
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new FakeHttpContent(sr.ReadToEnd()) { } };
            }

            var filterStr = (await request.Content.ReadAsStringAsync()) ?? string.Empty;

            Filter? filter = null;
            if (filterStr != null)
            {
                filter = Newtonsoft.Json.JsonConvert.DeserializeObject<Filter>(filterStr);
            }

            if (filter == null)
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new FakeHttpContent(sr.ReadToEnd()) { } };
            }

            switch (itemTypeAsString)
            {
                case nameof(Training):
                    return FilterTrainingsData(sr.ReadToEnd(), filter, request);
            }

            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }

        private HttpResponseMessage FilterTrainingsData(string responseContent, Filter filter, HttpRequestMessage request)
        {
            var trainingsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<PostTrainingsResponse>(responseContent);
            if (trainingsResponse == null || trainingsResponse?.Trainings?.Any() != true || filter?.Fields?.Any() != true)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            List<Training> trainingResultList = new List<Training>();
            foreach (var training in trainingsResponse.Trainings)
            {
                var trainingFieldDic = Utils.MapToDictionary(training);
                if (trainingFieldDic?.Any() != true)
                {
                    continue;
                }

                foreach (var field in filter.Fields)
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

                        if (field.Argument.OfType<string>().ToList().Contains(trainingField.Value ?? string.Empty))
                        {
                            trainingResultList.Add(training);
                        }
                    }
                }
            }

            trainingsResponse.Trainings = trainingResultList;
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new FakeHttpContent(Newtonsoft.Json.JsonConvert.SerializeObject(trainingsResponse)) { } };
        }
    }
}
