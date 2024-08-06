// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Api;
using Apollo.Common.Entities;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs;
using Amazon.Runtime.Internal.Util;

namespace Apollo.SemanticSearchExporter
{
    /// <summary>
    /// Exports the given Apollo entity into the CSV file compatible with daenet Semantic Search.
    /// </summary>
    internal class BlobStorageExporter
    {
        private ApolloApi _api;
        private string _entityName;
        private string _blobConnStr;
        private readonly ILogger<BlobStorageExporter> _logger;

        /// <summary>
        /// Creates the instance of the BlobStorageExporter.
        /// </summary>
        /// <param name="api">The Apollo API used to return list of exporting entity instances from the backend.</param>
        /// <param name="entity">The entity to be exported: Training, User, Profile, etc..</param>
        /// <param name="blobConnStr">The conneciton string to the blob storage used to persist exported CSV file.</param>
        public BlobStorageExporter(ApolloApi api, string entity, string blobConnStr, ILogger<BlobStorageExporter> logger)
        {
            _api = api;
            _entityName = entity;
            _blobConnStr = blobConnStr;
            _logger = logger;
        }


        /// <summary>
        /// Perfomrs the long-running export operation.
        /// </summary>
        /// <returns></returns>
        public async Task ExportAsync()
        {
            _logger.LogInformation($"Starting export for {_entityName}.");
            int totalItemsProcessed = 0;
            var startTime = DateTime.Now; // Capture start time for duration calculation

            try
            {
                var formatter = new TrainingFormatter();
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
                        var result = await _api.QueryTrainingsAsync(query);
                        hasMoreData = result.Count > 0;

                        foreach (var training in result)
                        {
                            var lines = formatter.FormatObject(training);
                            foreach (var line in lines)
                            {
                                var bytes = Encoding.UTF8.GetBytes(line + Environment.NewLine);
                                await stream.WriteAsync(bytes, 0, bytes.Length);
                            }
                            totalItemsProcessed += lines.Count;
                            _logger.LogDebug($"Exporting {training.Id}: {training.TrainingName}");
                        }

                        currentPosition += result.Count;
                        query.Skip = currentPosition;
                        _logger.LogDebug($"Processed {totalItemsProcessed} items. Moving to next batch starting from position {currentPosition}.");
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


        // Would it be a seperate method or would be do everything in one export method: going to different cases for entities (more bug prone)?
        ///// <summary>
        ///// Performs the export operation for combined Profile and Training data.
        ///// </summary>
        //public async Task ExportProfileTrainingAsync()
        //{
        //    _logger.LogInformation($"Starting combined profile and training export.");
        //    int totalItemsProcessed = 0;
        //    var startTime = DateTime.Now;

        //    try
        //    {
        //        var formatter = new CombinedProfileTrainingFormatter();
        //        var query = CreateProfileTrainingQuery();
        //        var containerClient = GetContainerClient();
        //        await containerClient.CreateIfNotExistsAsync();
        //        var blockBlobClient = containerClient.GetBlockBlobClient($"{_entityName}s.csv");


        //        using (var stream = await blockBlobClient.OpenWriteAsync(true))
        //        {
        //            int currentPosition = 0;
        //            bool hasMoreData = true;

        //            while (hasMoreData)
        //            {
        //                var result = await _api.QueryProfileTrainingsAsync(query);
        //                hasMoreData = result.Count > 0;

        //                foreach (var combinedData in result)
        //                {
        //                    var lines = formatter.FormatObject(combinedData);
        //                    foreach (var line in lines)
        //                    {
        //                        var bytes = Encoding.UTF8.GetBytes(line + Environment.NewLine);
        //                        await stream.WriteAsync(bytes, 0, bytes.Length);
        //                    }
        //                    totalItemsProcessed += lines.Count;
        //                }

        //                currentPosition += result.Count;
        //                query.Skip = currentPosition;
        //                _logger.LogDebug($"Processed {totalItemsProcessed} items. Moving to next batch starting from position {currentPosition}.");
        //            }
        //        }

        //        var duration = DateTime.Now - startTime;
        //        _logger.LogInformation($"Export completed for {_entityName}. Total items exported: {totalItemsProcessed}. Duration: {duration.TotalSeconds} seconds.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"An error occurred during export for {_entityName}.");
        //        throw;
        //    }
        //}

        private Query CreateQuery()
        {
            Query query = new Query()
            {
                Fields = new List<string> { "Id", "TrainingName", "Subtitle", "Description", "ShortDescription" },

                Filter = new Filter
                {
                    Fields = new List<FieldExpression>
                    {
                        new FieldExpression
                        {
                            FieldName = "id",
                            Operator = QueryOperator.NotEquals,
                            Argument = new string[]{ "UaT01" }
                        }
                        // Add other FieldExpressions as needed for additional conditions
                    }
                },

                Skip = 0,
                Top = 1000,
            };
            _logger.LogInformation($"Creating query for {_entityName} with fields: {string.Join(", ", query.Fields)} and filter on ID not equals to 'UaT01'. Skipping: {query.Skip}, Taking: {query.Top}");
            return query;
        }



        //Seperate query for combined list?

        //private Query CreateProfileTrainingQuery()
        //{
        //    Query query = new Query()
        //    {
        //        // Define fields and filters that are specific to the combined profile and training data
        //        Fields = new List<string> { /* List fields needed for combined data */ },
        //        Filter = new Filter
        //        {
        //            Fields = new List<FieldExpression>
        //        {
        //            // Example field expression
        //            new FieldExpression
        //            {
        //                FieldName = "ProfileId",
        //                Operator = QueryOperator.NotEquals,
        //                Argument = new string[] { "SomeProfileId" }
        //            }
        //        }
        //        },
        //        Skip = 0,
        //        Top = 1000
        //    };
        //    _logger.LogInformation($"Creating query for {_entityName} with fields: {string.Join(", ", query.Fields)} and filter on ID not equals to 'UaT01'. Skipping: {query.Skip}, Taking: {query.Top}");
        //    return query;
        //}


        /// <summary>
        /// Creates the BlobCLient API instance.
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
