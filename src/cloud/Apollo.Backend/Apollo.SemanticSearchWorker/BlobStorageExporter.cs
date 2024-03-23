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
    internal class BlobStorageExporter
    {
        private ApolloApi _api;
        private string _entityName;

        public BlobStorageExporter(ApolloApi api, string entity)
        {
            _api = api;
            _entityName = entity;
        }

        public async Task ExportAsync()
        {
            TrainingFormatter formatter = new TrainingFormatter();

            int currentPosition = 0;

            Query query = CreateQuery(currentPosition);

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
                }
            }
        }

        private static Query CreateQuery(int currentPosition)
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

                Skip = currentPosition,
                Top = 1000,
            };
            return query;
        }

        private static async Task UploadToStreamAsync(string blobName, StreamReader sr)
        {
            BlobContainerClient containerClient = GetContainerClient();

            await containerClient.CreateIfNotExistsAsync();

            BlockBlobClient blockBlobClient = containerClient.GetBlockBlobClient(blobName);

            using (Stream stream = await blockBlobClient.OpenWriteAsync(true))
            {
                while (true)
                {
                    string? line = await sr.ReadLineAsync();
                    if (line != null)
                    {
                        stream.Write(Encoding.UTF8.GetBytes(line));
                    }
                    else
                        break;
                }
            }
        }


        /// <summary>
        /// Creates the API from configuration.
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        private static BlobContainerClient GetContainerClient()
        {
            string? blobConnStr = _cfg["blobConnStr"];

            if (blobConnStr == null)
                throw new ArgumentException("The connection string of the blob storage with the write permission must be specified in the configuration. I.e:as argument  '--blobConnStr=...' or as environment variable 'blobConnStr=...'");

            BlobContainerClient containerClient = new BlobContainerClient(blobConnStr, "export");

            return containerClient;
        }


    }
}
