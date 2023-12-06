﻿using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Amazon.Runtime.Internal.Util;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;

namespace Daenet.MongoDal
{
    /// <summary>
    /// Implements the Data Access Layer for communication with the Mongo Database.
    /// </summary>
    public class MongoDataAccessLayer
    {        
        private readonly IMongoDatabase _db;

        private readonly MongoDalConfig _cfg;

        private readonly ILogger<MongoDataAccessLayer>? _logger;


        /// <summary>
        /// Client instance.
        /// </summary>
        private readonly MongoClient _client;

        /// <summary>
        /// The key used for partitioning.  
        /// </summary>
       // public string? ShredKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="logger"></param>
        /// <param name="shredKey">Used as a partitioning key.</param>
        public MongoDataAccessLayer(MongoDalConfig cfg, ILogger<MongoDataAccessLayer>? logger = null/*, string? shredKey = null*/)
        {
            _cfg = cfg;

            _logger = logger;

            //ShredKey = shredKey;

            this._client = new MongoClient(GetSettings(_cfg.MongoConnStr));

            this._db = this._client.GetDatabase(_cfg.MongoDatabase);
        }

        /// <summary>
        ///  Inserts the set of documents into the collection.
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        public async Task InsertManyAsync(string collectionName, IEnumerable<ExpandoObject> documents)
        {
            InsertManyOptions options = new InsertManyOptions
            {
                IsOrdered = false,
                BypassDocumentValidation = true
            };

            var coll = GetCollection(collectionName);

            ICollection<BsonDocument> bsons = new List<BsonDocument>();

            foreach (var document in documents)
            {
                bsons.Add(new BsonDocument(document));
            }

            await coll.InsertManyAsync(bsons, options);
        }


        /// <summary>
        ///  Inserts a document into the collection.
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public async Task InsertAsync(string collectionName, ExpandoObject document)
        {
            var coll = GetCollection(collectionName);
            await coll.InsertOneAsync(new BsonDocument(document));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<UpsertResult> UpsertAsync(string collectionName, ICollection<ExpandoObject> documents)
        {
            if (documents == null)
                throw new ArgumentNullException($"Argument {nameof(documents)} cannot be nulL!");

            var coll = GetCollection(collectionName);

            try
            {
                int inserted = 0;
                int updated = 0;

                foreach (IDictionary<string, object> item in documents)
                {
                    if (item == null)
                        throw new ArgumentNullException(nameof(item));

                    FilterDefinition<BsonDocument> filter =
                    Builders<BsonDocument>.Filter.Eq("_id", item["_id"]);                   

                    //FilterDefinition<BsonDocument> filter = ShredKey == null ?
                    //    Builders<BsonDocument>.Filter.Eq("_id", item["_id"]) :
                    //    Builders<BsonDocument>.Filter.Eq("_id", item["_id"]) &
                    //    Builders<BsonDocument>.Filter.Eq("ShredKey", item[ShredKey]);

                    BsonDocument? doc =
                        await coll.FindOneAndUpdateAsync<BsonDocument>(filter, BuildUpdate(item as ExpandoObject),
                        new FindOneAndUpdateOptions<BsonDocument>
                        {
                            IsUpsert = true,
                            ReturnDocument = ReturnDocument.Before
                        });

                    if (doc == null)
                    {
                        // inserted
                        inserted++;
                    }
                    else
                    {
                        // updated
                        updated++;
                    }
                }

                return new UpsertResult
                {
                    Inserted = inserted,
                    Updated = updated
                };
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex.Message, $"{nameof(UpsertAsync)} has failed");
                throw;

            }
        }


        /// <summary>
        /// Builds an update definition for the given document.
        /// </summary>
        /// <param name="doc">The document to build the update definition for.</param>
        /// <returns>The update definition for the given document.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the given document is null.</exception>
        private UpdateDefinition<BsonDocument> BuildUpdate(ExpandoObject doc)
        {
            if (doc == null)
                throw new ArgumentNullException("The argument doc cannot be null.");

            var builder = Builders<BsonDocument>.Update;
            var updates = new List<UpdateDefinition<BsonDocument>>();

            foreach (var prop in (IDictionary<string, object>)doc!)
            {
                if (prop.Key == "_id")
                    continue; // Mongo doesn't allow changing Mongo IDs
                else if (prop.Value == null)// TODO we cannot update existing value to NULL!
                    continue;
                else
                {
                    if (prop.Key == "CreatedAt" || prop.Key == "CreatedBy")
                    {
                        updates.Add(builder.SetOnInsert(prop.Key, prop.Value));
                    }
                    else
                    {
                        updates.Add(builder.Set(prop.Key, prop.Value));
                    }
                }
            }

            return builder.Combine(updates);
        }


        /// <summary>
        /// Deletes document.
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="id"></param>
        /// <param name="throwIfNotDeleted">Set on false if exception should be thrown if the document is not deleted. Default value is true.</param>
        public async Task DeleteAsync(string collectionName, string id, bool throwIfNotDeleted = true)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var coll = GetCollection(collectionName);

            var result = await coll.DeleteOneAsync(filter);

            if (throwIfNotDeleted && result.DeletedCount != 1)
                throw new ApplicationException("Document cannot be deleted!");

        }


        /// <summary>
        /// Deletes many documents.
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="ids"></param>
        /// <param name="throwIfNotDeleted">Set on false if exception should be thrown if the document is not deleted. Default value is true.</param>
        /// <returns>Number of deleted records.</returns>
        public async Task<long> DeleteManyAsync(string collectionName, string[] ids, bool throwIfNotDeleted = true)
        {
            var filter = Builders<BsonDocument>.Filter.In("_id", ids.Select(i => i));

            var coll = GetCollection(collectionName);

            var result = await coll.DeleteManyAsync(filter);

            if (throwIfNotDeleted && result.DeletedCount != ids.Length)
                throw new ApplicationException("Documents cannot be deleted!");

            return result.DeletedCount;
        }


        /// <summary>
        /// Looks up the set of documents.  
        /// </summary>        
        public async Task<IList<ExpandoObject>> ExecuteQuery(string collectionName, ICollection<string>? fields, /*string partitionKey,*/ Query query, int top, int skip, SortExpression sortExpression = null, DateTime? dateTime = null)
        {
            if (skip < 0)
                skip = 0;
            var results = new List<ExpandoObject>();

            var coll = GetCollection(collectionName);

            FilterDefinition<BsonDocument> filter;

            //
            // Firts, match everything
            if (query == null || query.IsOrOperator == false)
                filter = FilterDefinition<BsonDocument>.Empty;
            else
                filter = Builders<BsonDocument>.Filter.Eq("_id", "should not match anything");

            // filter = Builders<BsonDocument>.Filter.Eq(EntitySet.PartitionKey, partitionKey);

            if (query != null)
            {
                filter = BuildFilterFind(query, filter);
                //filter = BuildFilter(query, filter); 
            }

            if (dateTime.HasValue)
            {
                filter &= Builders<BsonDocument>.Filter.Gte("ChangedAt", dateTime.Value);
            }

            SortDefinition<BsonDocument> sort;

            if (sortExpression != null)
            {
                if (sortExpression.Order == SortOrder.Ascending)
                {
                    sort = Builders<BsonDocument>.Sort.Ascending(sortExpression.FieldName);
                }
                else
                {
                    sort = Builders<BsonDocument>.Sort.Descending(sortExpression.FieldName);
                }
            }
            else
            {
                sort = Builders<BsonDocument>.Sort.Descending("ChangedAt");
            }
            var projection = new BsonDocument();

            var bsonElements = new List<BsonElement>();

            IAsyncCursor<BsonDocument> documents;

            //FindOptions findOptions = new FindOptions
            //{
            //    BatchSize = 10000,
            //    NoCursorTimeout = true
            //};

            if (fields != null)
            {
                projection.Add(new BsonElement("_id", 0));

                foreach (var field in fields)
                {
                    projection.Add(new BsonElement(field, 1));
                }


                //documents = await coll.Aggregate().Unwind("ArticleInfo").Match(filter).Sort(sort).Skip(skip).Limit(top).Project(projection).ToCursorAsync();

                documents = await coll.Find(filter).Sort(sort).Skip(skip).Limit(top).Project(projection).ToCursorAsync();
            }
            else
            {
                documents = await coll.Find(filter).Sort(sort).Skip(skip).Limit(top).ToCursorAsync();
                //documents = await coll.Aggregate().Sort(sort).Skip(skip).Limit(top).ToCursorAsync();
            }

            foreach (var doc in documents.ToEnumerable())
            {
                dynamic dynRow = BsonSerializer.Deserialize<dynamic>(doc);

                ExpandoObject expando = dynRow;

                results.Add(expando);
            }
            //var counter = 0;
            //while (await documents.MoveNextAsync())
            //{
            //    var docs = documents.Current;
            //    foreach (var doc in docs)
            //    {
            //        counter++;
            //        if (counter > skip)
            //        {
            //            dynamic dynRow = BsonSerializer.Deserialize<dynamic>(doc);

            //            ExpandoObject expando = dynRow;

            //            results.Add(expando);
            //        }
            //    }
            //    docs = null;
            //    GC.Collect();
            //}


            //var docs = await coll.Aggregate().Unwind(new StringFieldDefinition<BsonDocument>("ReferencePrices")).Match(filter).Skip(skip).Limit(top).Project(projection).ToListAsync();
            //var res = new List<ExpandoObject>();
            //foreach (var doc in docs)
            //{
            //    dynamic dynRow = BsonSerializer.Deserialize<dynamic>(doc);

            //    ExpandoObject expando = dynRow;

            //    res.Add(expando);
            //}

            return results;
        }

        private static MongoClientSettings GetSettings(string connectionString)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.MinConnectionPoolSize = 50;
            settings.MaxConnectionPoolSize = 1000;
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };

            return settings;
        }

        /// <summary>
        /// Gets the typed instance of the collection.
        /// </summary>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <param name="collectionName">The name of collection.</param>
        /// <returns></returns>
        private IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentException($"Parameter '{nameof(collectionName)}' is empty!");

            var todoTaskCollection = this._db.GetCollection<BsonDocument>(collectionName);

            return todoTaskCollection;
        }

        /// <summary>
        /// Build Filter for collection.find
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static FilterDefinition<BsonDocument> BuildFilterFind(Query query, FilterDefinition<BsonDocument> filter)
        {
            //
            // find all filter with fields contains dot (.) character
            var specialQueries = query.Fields.Where(f => f.FieldName.Contains('.')).GroupBy(f => f.FieldName.Split('.')[0]);

            if (query.IsOrOperator)
            {
                foreach (var q in query.Fields)
                {
                    if (IsSpecialFilter(specialQueries, q))
                        continue;

                    filter |= BuildOrFilter(q);
                }

                filter = BuildArrayElementMatchOrFilter(filter, specialQueries);
            }
            else
            {
                foreach (var q in query.Fields)
                {
                    if (IsSpecialFilter(specialQueries, q))
                        continue;

                    filter &= BuildAndFilter(q);
                }

                filter = BuildArrayElementMatchAndFilter(filter, specialQueries);
            }

            //object t = 115.44;
            //filter |= Builders<BsonDocument>.Filter.Lt("HP", t);

            return filter;
        }

        private static FilterDefinition<BsonDocument> BuildArrayElementMatchAndFilter(FilterDefinition<BsonDocument> filter, IEnumerable<IGrouping<string, FieldExpression>> specialQueries)
        {
            foreach (var item in specialQueries)
            {
                if (item.Count() == 1)
                    continue;

                FilterDefinition<BsonDocument> andFilter = FilterDefinition<BsonDocument>.Empty;
                foreach (var q in item)
                {
                    var f = new FieldExpression
                    {
                        FieldName = q.FieldName.Split('.')[1],
                        Argument = q.Argument,
                        Operator = q.Operator
                    };

                    andFilter &= BuildAndFilter(f);
                }

                filter &= Builders<BsonDocument>.Filter.ElemMatch<BsonDocument>(item.Key, andFilter);
            }

            return filter;
        }

        private static FilterDefinition<BsonDocument> BuildArrayElementMatchOrFilter(FilterDefinition<BsonDocument> filter, IEnumerable<IGrouping<string, FieldExpression>> specialQueries)
        {
            foreach (var item in specialQueries)
            {
                if (item.Count() == 1)
                    continue;

                FilterDefinition<BsonDocument> andFilter = FilterDefinition<BsonDocument>.Empty;
                foreach (var q in item)
                {
                    var f = new FieldExpression
                    {
                        FieldName = q.FieldName.Split('.')[1],
                        Argument = q.Argument,
                        Operator = q.Operator
                    };

                    andFilter &= BuildAndFilter(f);
                }

                filter |= Builders<BsonDocument>.Filter.ElemMatch<BsonDocument>(item.Key, andFilter);
            }

            return filter;
        }

        private static bool IsSpecialFilter(IEnumerable<IGrouping<string, FieldExpression>> specialQueries, FieldExpression q)
        {
            foreach (var item in specialQueries)
            {
                if (item.Count() > 1 && item.Contains(q))
                {
                    return true;
                }
            }
            return false;
        }

        private static FilterDefinition<BsonDocument> BuildAndFilter(FieldExpression q)
        {
            if (q.Operator == QueryOperator.Contains)
                return BuildOrContainsFilter(q);
            else if (q.Operator == QueryOperator.Equals)
                return BuildOrEqFilter(q);
            else if (q.Operator == QueryOperator.GreaterThan)
                return BuildOrGtFilter(q);
            else if (q.Operator == QueryOperator.LessThan)
                return BuildOrLtFilter(q);
            else if (q.Operator == QueryOperator.StartsWith)
                return BuildStarsWithFilter(q);
            else if (q.Operator == QueryOperator.NotEquals)
                return BuildNotEqFilter(q);
            else if (q.Operator == QueryOperator.Empty)
                return BuildOrEmptyFilter(q);
            else if (q.Operator == QueryOperator.GreaterThanEqualTo)
                return BuildOrGteFilter(q);
            else if (q.Operator == QueryOperator.LessThanEqualTo)
                return BuildOrLteFilter(q);
            else
                throw new NotSupportedException($"The operator {q.Operator} is not supported.");
        }

        private static FilterDefinition<BsonDocument> BuildOrGteFilter(FieldExpression q)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");

            foreach (var arg in q.Argument)
            {
                orFilter |= Builders<BsonDocument>.Filter.Gte(q.FieldName, arg);
            }

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildOrLteFilter(FieldExpression q)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");

            foreach (var arg in q.Argument)
            {
                orFilter |= Builders<BsonDocument>.Filter.Lte(q.FieldName, arg);
            }

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildOrFilter(FieldExpression q)
        {
            if (q.Operator == QueryOperator.Contains)
                return BuildOrContainsFilter(q);
            else if (q.Operator == QueryOperator.Equals)
                return BuildOrEqFilter(q);
            else if (q.Operator == QueryOperator.GreaterThan)
                return BuildOrGtFilter(q);
            else if (q.Operator == QueryOperator.LessThan)
                return BuildOrLtFilter(q);
            else if (q.Operator == QueryOperator.StartsWith)
                return BuildStarsWithFilter(q);
            else if (q.Operator == QueryOperator.NotEquals)
                return BuildNotEqFilter(q);
            else if (q.Operator == QueryOperator.Empty)
                return BuildOrEmptyFilter(q);
            else if (q.Operator == QueryOperator.In)
                return BuildInFilter(q);
            else if (q.Operator == QueryOperator.GreaterThanEqualTo)
                return BuildOrGteFilter(q);
            else if (q.Operator == QueryOperator.LessThanEqualTo)
                return BuildOrLteFilter(q);
            else
                throw new NotSupportedException($"The operator {q.Operator} is not supported.");
        }

        private static FilterDefinition<BsonDocument> BuildOrContainsFilter(FieldExpression qField)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");

            orFilter |= Builders<BsonDocument>.Filter.In(qField.FieldName, qField.Argument.Select(arg =>
            {
                var text = arg == null ? string.Empty : arg.ToString();
                //if (qField.FieldName == nameof(EntityBase.ObjectId))
                //{
                //    return new BsonRegularExpression(GetCorrectRegexPattern(text));
                //}
                return new BsonRegularExpression(GetCorrectRegexPattern(text), "i");
            }));

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildOrEqFilter(FieldExpression qField)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");

            var emptyValues = new List<object> { "", null, new List<object>() };

            var arguments = qField.Argument;
            if (qField.Argument.Intersect(emptyValues).Any())
            {
                arguments = qField.Argument.Union(emptyValues).ToList();
            }
            orFilter |= Builders<BsonDocument>.Filter.In(qField.FieldName, arguments);

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildInFilter(FieldExpression qField)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");
            orFilter |= Builders<BsonDocument>.Filter.In(qField.FieldName, qField.Argument);

            return orFilter;
        }


        private static FilterDefinition<BsonDocument> BuildOrEmptyFilter(FieldExpression qField)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");

            orFilter |= Builders<BsonDocument>.Filter.Eq(qField.FieldName, string.Empty);
            orFilter |= Builders<BsonDocument>.Filter.Eq(qField.FieldName, default(string));
            orFilter |= Builders<BsonDocument>.Filter.Eq(qField.FieldName, new List<object>());

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildOrGtFilter(FieldExpression qField)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");

            foreach (var arg in qField.Argument)
            {
                orFilter |= Builders<BsonDocument>.Filter.Gt(qField.FieldName, arg);
            }

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildOrLtFilter(FieldExpression qField)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");
            foreach (var arg in qField.Argument)
            {
                orFilter |= Builders<BsonDocument>.Filter.Lt(qField.FieldName, arg);
            }

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildStarsWithFilter(FieldExpression qField)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");

            orFilter |= Builders<BsonDocument>.Filter.In(qField.FieldName, qField.Argument.Select(arg => new BsonRegularExpression($"^{GetCorrectRegexPattern(arg.ToString())}", "i")));

            //foreach (var arg in qField.Argument)
            //{
            //    orFilter |= Builders<BsonDocument>.Filter.Regex(qField.FieldName, $"/^{arg}/");
            //}

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildNotEqFilter(FieldExpression qField)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");

            var emptyValues = new List<object> { "", null, new List<object>() };

            var arguments = qField.Argument;
            if (qField.Argument.Intersect(emptyValues).Any())
            {
                arguments = qField.Argument.Union(emptyValues).ToList();
            }
            orFilter |= Builders<BsonDocument>.Filter.Nin(qField.FieldName, arguments);

            return orFilter;
        }

        private static string GetCorrectRegexPattern(string inputPattern)
        {
            //return Regex.Replace(inputPattern, "(?=[^a-zA-Z0-9üöä ])", "\\\\");
            var pattern = Regex.Replace(inputPattern, "(?=[\\.\\^\\$\\*\\+\\-\\?\\(\\)\\[\\]\\{\\}\\\\\\|\\—\\/])", "\\");
            return pattern;
        }

        public async Task<T> GetByIdAsync<T>(string collectionName, string id)
        {
            var coll = GetCollection(collectionName);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var document = await coll.Find(filter).FirstOrDefaultAsync();

            if (document == null)
            {
                // Document not found, return null or throw an exception as needed.
                return default(T);
            }

            // Deserialize the BsonDocument to the desired type T.
            return BsonSerializer.Deserialize<T>(document);
        }

        public async Task<long> CountDocumentsAsync(string collectionName, FilterDefinition<BsonDocument> filter)
        {
            var coll = GetCollection(collectionName);

            long count = await coll.CountDocumentsAsync(filter);

            return count;
        }


        /// <summary>
        /// Updates documents in the collection that match the filter with the provided update definition.
        /// </summary>
        /// <param name="collectionName">The name of the collection to update documents in.</param>
        /// <param name="filter">The filter to match documents for the update.</param>
        /// <param name="updateDefinition">The update definition to apply to matching documents.</param>
        /// <returns>A task representing the asynchronous update operation.</returns>
        public async Task<UpdateResult> UpdateAsync(string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> updateDefinition)
        {
            var coll = GetCollection(collectionName);

            UpdateResult result = await coll.UpdateManyAsync(filter, updateDefinition);

            return result;
        }


    }
}