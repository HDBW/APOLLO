using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Common.Entities;
using Daenet.EmbeddingSearchApi.Interfaces;
using Daenet.EmbeddingSearchApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Apollo.SmartLib
{
    /// <summary>
    /// API that implements all semantic related functionalities.
    /// </summary>
    public class ApolloSemanticSearchApi
    {
        //TrainingName
        //SubTitle
        //Description
        //ShortDescription
        //Content
        //BenefitList
        //Prerequisites

        private readonly ISearchApi _sApi;

        private readonly ILogger<ApolloSemanticSearchApi> _logger;

        private int _topN = 5;

        private readonly IConfiguration _configuration;
        private readonly string _context;


        public ApolloSemanticSearchApi(ISearchApi semanticSearchApi,
                                        ILogger<ApolloSemanticSearchApi> logger)
        {
            _sApi = semanticSearchApi;
            _logger = logger;

            // Build configuration
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = configurationBuilder.Build();

            _context = _configuration["ApolloSemanticSearchApiConfig:context"]
                       ?? throw new ApplicationException("Context not found in appsettings.json.");

        }


        private static string[] _supportedFields = new string[] { $"{nameof(Training.TrainingName)}", $"{nameof(Training.SubTitle)}", $"{nameof(Training.Description)}", $"{nameof(Training.ShortDescription)}", $"{nameof(Training.Content)}", $"{nameof(Training.BenefitList)}", $"{nameof(Training.Prerequisites)}" };






        public async Task<List<SemanticSearchResult>> SearchTrainingsAsync(Query query)
        {
            List<SemanticSearchResult> res = new List<SemanticSearchResult>();

            StringBuilder sb = new StringBuilder();

            if (query != null && query.Filter != null && query.Fields != null)
            {
                foreach (var prop in query?.Filter?.Fields!)
                {
                    if (prop.FieldName != null && _supportedFields.Contains(prop.FieldName))
                    {

                        // Check if StringBuilder is not empty to append a comma before adding new content
                        if (sb.Length > 0)
                            sb.Append(',');
                        // Check if prop.Argument is not null before appending, otherwise append an empty string
                        sb.Append(prop.Argument != null ? String.Join(',', prop.Argument) : "");
                    }
                    else
                    {
                        _logger?.LogTrace($"{prop.FieldName} specified in the filter is not supported by semantic search!");
                        continue;
                    }
                }

                //
                //Currently we are using existing context for test
                //After Amit fix we will replace with Trainings context
                //var semRes = await _sApi.FindMatchingContentAsync(sb.ToString(), _topN, $"{nameof(Training)}s");
                var semRes = await _sApi.FindMatchingContentAsync(sb.ToString(), _topN, _context);

                res = ToSemanticSearchResult(semRes);

                return res;
            }
            else
                throw new ArgumentException($"{nameof(query)}, {nameof(query.Filter)} and {nameof(query.Filter.Fields)} must not be null!");

        }

        private List<SemanticSearchResult> ToSemanticSearchResult(List<Daenet.EmbeddingSearchApi.Entities.MatchResult> semRes)
        {
            List<SemanticSearchResult> res = new List<SemanticSearchResult>();

            foreach (var item in semRes)
            {
                res.Add(new SemanticSearchResult()
                {
                    Property = item.Title,
                    Similarity = item.Similarity,
                    Text = item.SectionText,
                    TrainingId = item.Url.Substring(item.Url.IndexOf("/api/training/") + "/api/training/".Length)

            });
            }

            return res;
        }

      

    }

   

    public class SemanticSearchResult
    {
        public string TrainingId { get; set; }

        public string Property { get; set; }

        public double Similarity { get; set; }

        public string Text { get; set; }
    }
}
