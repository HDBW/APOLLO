using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Common.Entities;
using Daenet.EmbeddingSearchApi.Interfaces;
using Daenet.EmbeddingSearchApi.Services;

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

        ISearchApi _sApi;

        string _dataSource;

        public ApolloSemanticSearchApi(ISearchApi semanticSearchApi)
        {
           _sApi = semanticSearchApi;
        }

        public async Task<SearchResult> SearchTrainings(Query query)
        {
            throw new NotImplementedException();
            //_sApi.FindMatchingContentAsync<Training>(query.Filter)
        }
    }

    public class SearchResult
    {
        public string TrainingId { get; set; }

        public string Property { get; set; }

        public double Similarity { get; set; }

        public string   Text { get; set; }
    }
}
