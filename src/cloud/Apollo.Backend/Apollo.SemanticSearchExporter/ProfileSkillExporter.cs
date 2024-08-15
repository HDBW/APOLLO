// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Api;
using Apollo.Common.Entities;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Logging;

namespace Apollo.SemanticSearchExporter
{
    /// <summary>
    /// Exports the given Apollo entity into the CSV file compatible with LLM-AI Semantic Search.
    /// </summary>
    internal class ProfileSkillExporter
    {
        private ApolloApi _api;
        private string _entityName;
        private string _blobConnStr;
        private readonly ILogger<ProfileSkillExporter> _logger;

        /// <summary>
        /// Creates the instance of the ProfileSkillExporter.
        /// </summary>
        /// <param name="api">The Apollo API used to return list of exporting entity instances from the backend.</param>
        /// <param name="entity">The entity to be exported: Skill.</param>
        /// <param name="blobConnStr">The connection string to the blob storage used to persist exported CSV file.</param>
        public ProfileSkillExporter(ApolloApi api, string entity, string blobConnStr, ILogger<ProfileSkillExporter> logger)
        {
            _api = api;
            _entityName = entity;
            _blobConnStr = blobConnStr;
            _logger = logger;
        }

        /// <summary>
        /// Performs the long-running export operation.
        /// </summary>
        /// <returns></returns>
        public async Task ExportAsync()
        {
            _logger.LogInformation($"Starting export for {_entityName}.");
            int totalItemsProcessed = 0;
            var startTime = DateTime.Now; // Capture start time for duration calculation

            try
            {
                var formatter = new ProfileSkillFormatter();
                var query = CreateQuery();
                var containerClient = GetContainerClient();
                await containerClient.CreateIfNotExistsAsync();
                var blockBlobClient = containerClient.GetBlockBlobClient($"{_entityName}s.csv");

                using (var stream = await blockBlobClient.OpenWriteAsync(true))
                {
                    int currentPosition = 0;
                    bool hasMoreData = true;

                    while (hasMoreData)
                    {
                        var profiles = await _api.QueryProfilesAsync(query);
                        _logger.LogInformation($"Fetched {profiles.Count} profiles.");

                        hasMoreData = profiles.Count > 0;

                        foreach (var profile in profiles)
                        {
                            _logger.LogDebug($"Processing profile {profile.Id}");

                            // Process Skills
                            if (profile.Skills != null && profile.Skills.Count > 0)
                            {
                                foreach (var skill in profile.Skills)
                                {
                                    var lines = formatter.FormatObject(skill, profile.Id);
                                    foreach (var line in lines)
                                    {
                                        var bytes = Encoding.UTF8.GetBytes(line + Environment.NewLine);
                                        await stream.WriteAsync(bytes, 0, bytes.Length);
                                    }
                                    totalItemsProcessed += lines.Count;
                                }
                            }

                            // Process LanguageSkills
                            if (profile.LanguageSkills != null && profile.LanguageSkills.Count > 0)
                            {
                                foreach (var languageSkill in profile.LanguageSkills)
                                {
                                    var lines = formatter.FormatLanguageSkill(languageSkill, profile.Id);
                                    foreach (var line in lines)
                                    {
                                        var bytes = Encoding.UTF8.GetBytes(line + Environment.NewLine);
                                        await stream.WriteAsync(bytes, 0, bytes.Length);
                                    }
                                    totalItemsProcessed += lines.Count;
                                }
                            }

                            // Process LeadershipSkills
                            if (profile.LeadershipSkills != null)
                            {
                                var lines = formatter.FormatLeadershipSkill(profile.LeadershipSkills, profile.Id);
                                foreach (var line in lines)
                                {
                                    var bytes = Encoding.UTF8.GetBytes(line + Environment.NewLine);
                                    await stream.WriteAsync(bytes, 0, bytes.Length);
                                }
                                totalItemsProcessed += lines.Count;
                            }
                        }

                        currentPosition += profiles.Count;
                        query.Skip = currentPosition;
                    }
                }

                var duration = DateTime.Now - startTime;
                _logger.LogInformation($"Export completed for {_entityName}. Total items exported: {totalItemsProcessed}. Duration: {duration.TotalSeconds} seconds.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred during export for {_entityName}.");
                throw;
            }
        }


        private Query CreateQuery()
        {
            Query query = new Query()
            {
                Fields = new List<string>
                {
                    "Id",
                    "Skills",
                    "LanguageSkills",
                    "LeadershipSkills"
                },

                Filter = new Filter
                {
                    Fields = new List<FieldExpression>
                    {
                        new FieldExpression
                        {
                            FieldName = "id",
                            Operator = QueryOperator.NotEquals,
                            Argument = new string[]{ "InvalidId" }
                        }
                    }
                },

                Skip = 0,
                Top = 1000,
            };
            _logger.LogInformation($"Creating query for {_entityName} with fields: {string.Join(", ", query.Fields)} and filter on ID not equals to 'InvalidId'. Skipping: {query.Skip}, Taking: {query.Top}");
            return query;
        }

        /// <summary>
        /// Creates the BlobClient API instance.
        /// </summary>
        /// <returns></returns>
        private BlobContainerClient GetContainerClient()
        {
            BlobContainerClient containerClient = new BlobContainerClient(_blobConnStr, "export");
            _logger.LogDebug($"Accessing blob container for export.");
            return containerClient;
        }
    }
}
