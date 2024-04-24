using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Common.Entities;
using Daenet.EmbeddingSearchApi.Interfaces;
using Daenet.EmbeddingSearchApi.Services;
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
     
        public ApolloSemanticSearchApi(ISearchApi semanticSearchApi, ILogger<ApolloSemanticSearchApi> logger)
        {
            _sApi = semanticSearchApi;
            _logger = logger;
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
                        //"TitleValue
                        //SubTitleValue
                        //..."
                        sb.Append(String.Join(',', prop.Argument));
                    }
                    else
                    {
                        _logger?.LogTrace($"{prop.FieldName} specified in the filter is not supported by semantic search!");
                        continue;
                    }
                }

                var semRes = await _sApi.FindMatchingContentAsync(sb.ToString(), _topN, $"{nameof(Training)}s");

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
                    //Property = item.Property,
                    //Similarity = item.Similarity,
                    //Text = item.Text,
                    //TrainingId = item.Id
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
