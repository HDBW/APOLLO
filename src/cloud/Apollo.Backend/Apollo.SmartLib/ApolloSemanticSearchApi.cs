using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Common.Entities;
using Daenet.EmbeddingSearchApi.Interfaces;
using Daenet.EmbeddingSearchApi.Services;
using iText.Layout.Borders;
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



        // User Profiles:
        ////StartDate
        //- CarrerInfo
        //// StartDate 
        //- EducationInfo

        //Contain a List with Dates of the Occupation

        //Occupation in CarrerInfo, EducationInfo
        //Go to a Service lookup Skills
        //Save it List Skills in Profile
        //Occupation Service 
        //-> Occupation
        //   Skill
        //Worker extract skill
        //-> View instances for Skill Profiles - JSON
        //----
        //Training Properties that might contain skills:
        //- Benefitlist
        //- Certificate
        //- Content
        //- Description
        //- Prerequisites
        //- TargetAudience?
        //Level:
        //(
        //- Description
        //- Prerequisites
        //- TargetAudience?
        //)
        //Create Method Extract Skills:
        //- List of Skills how to implement?
        //Properties:
        //Flatend Text - StringBuilder Append blablabla
        //Invoke Model extract Skills?
        //=> Test some Skill Extractions

        // Method to extract skills from a Training object
        public List<string> ExtractSkills(Training training)
        {
            // Initialize list to store extracted skills
            List<string> extractedSkills = new List<string>();

            // Flatten text from different properties into a single string
            StringBuilder flattenedText = new StringBuilder();
            AppendIfNotNullOrEmpty(flattenedText, ConcatenateList(training.BenefitList));
            AppendIfNotNullOrEmpty(flattenedText, ConcatenateList(training.Certificate));
            AppendIfNotNullOrEmpty(flattenedText, ConcatenateList(training.Content));
            AppendIfNotNullOrEmpty(flattenedText, training.Description!);
            AppendIfNotNullOrEmpty(flattenedText, ConcatenateList(training.Prerequisites));
            AppendIfNotNullOrEmpty(flattenedText, training.TargetAudience!);

            // Extract skills from flattened text
            extractedSkills = ExtractSkillsFromText(flattenedText);

            // Return the list of extracted skills
            return extractedSkills;
        }

        // Method to concatenate a list of strings into a single string
        private string ConcatenateList(List<string>? list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            StringBuilder concatenatedText = new StringBuilder();
            foreach (var item in list)
            {
                concatenatedText.Append(item);
                concatenatedText.Append(","); 
            }
            return concatenatedText.ToString();
        }

        // Method to append non-null and non-empty strings to a StringBuilder
        private void AppendIfNotNullOrEmpty(StringBuilder stringBuilder, string? text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                stringBuilder.Append(text);
                stringBuilder.Append(","); 
            }
        }

        // Method to extract skills from text using a StringBuilder
        private List<string> ExtractSkillsFromText(StringBuilder text)
        {
           

            // For demonstration purposes, just splitting text by space
            string[] words = text.ToString().Split(',');

            // Filter out non-skills (e.g., common words, stopwords)
            List<string> skills = new List<string>();
            foreach (string word in words)
            {
           
                if (!string.IsNullOrWhiteSpace(word))
                {
                    skills.Add(word);
                }
            }

            return skills;
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
