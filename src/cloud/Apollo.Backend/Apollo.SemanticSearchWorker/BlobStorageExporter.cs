// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Api;
using Apollo.Common.Entities;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs;

namespace Apollo.SemanticSearchWorker
{
    /// <summary>
    /// Exports the given Apollo entity into the CSV file compatible with daenet Semantc Search.
    /// </summary>
    internal class BlobStorageExporter
    {
        private ApolloApi _api;
        private string _entityName;
        private string _blobConnStr;

        /// <summary>
        /// Creates the instance of the BlobStorageExporter.
        /// </summary>
        /// <param name="api">The Apollo API used to return list of exporting entity instances from the backend.</param>
        /// <param name="entity">The entity to be exported: Training, User, Profile, etc..</param>
        /// <param name="blobConnStr">The conneciton string to the blob storage used to persist exported CSV file.</param>
        public BlobStorageExporter(ApolloApi api, string entity, string blobConnStr)
        {
            _api = api;
            _entityName = entity;
            _blobConnStr = blobConnStr;
        }


        /// <summary>
        /// Perfomrs the long-running export operation.
        /// </summary>
        /// <returns></returns>
        public async Task ExportAsync()
        {
            TrainingFormatter formatter = new TrainingFormatter();

            int currentPosition = 0;

            Query query = CreateQuery();

            BlobContainerClient containerClient = GetContainerClient();

            await containerClient.CreateIfNotExistsAsync();

            BlockBlobClient blockBlobClient = containerClient.GetBlockBlobClient($"{_entityName}s.csv");

            using (Stream stream = await blockBlobClient.OpenWriteAsync(true))
            {
                while (true)
                {
                    var result = await _api.QueryTrainingsAsync(query);

                    foreach (var training in result)
                    {
                        var list = formatter.FormatObject(training);
                        foreach (var item in list)
                        {
                            stream.Write(Encoding.UTF8.GetBytes(item));
                        }                       
                    }

                    if (result.Count == 0)
                        break;
                    else
                    {
                        currentPosition += result.Count;
                        query.Skip = currentPosition;
                    }
                }
            }
        }

        private static Query CreateQuery()
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
            return query;
        }


        /// <summary>
        /// Creates the BlobCLient API instance.
        /// </summary>
        /// <returns></returns>
        private BlobContainerClient GetContainerClient()
        {
            BlobContainerClient containerClient = new BlobContainerClient(_blobConnStr, "export");

            return containerClient;
        }
    }
}
