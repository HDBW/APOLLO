using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using Amazon.Runtime.Internal.Util;
using Daenet.MongoDal.Entitties;
using Daenet.MongoDal.Functions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;
using SharpCompress.Common;

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

            var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
            ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);

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

        internal async Task<IAsyncCursor<BsonDocument>> ExecuteQueryInternal(string collectionName, ICollection<string>? fields, /*string partitionKey,*/ Query query, int top, int skip, SortExpression sortExpression = null, DateTime? dateTime = null)
        {
            if (skip < 0)
                skip = 0;

            var coll = GetCollection(collectionName);

            FilterDefinition<BsonDocument> filter;

            //
            // Firts, match everything
            if (query == null || query.IsOrOperator == false)
                filter = FilterDefinition<BsonDocument>.Empty;
            else
                filter = Builders<BsonDocument>.Filter.Eq("_id", "should not match anything");

            if (query != null)
            {
                filter = BuildFilterFind(query, filter);
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

            if (fields != null)
            {
                projection.Add(new BsonElement("_id", 1));

                foreach (var field in fields)
                {
                    projection.Add(new BsonElement(field, 1));
                }

                documents = await coll.Find(filter).Sort(sort).Skip(skip).Limit(top).Project(projection).ToCursorAsync();
            }
            else
            {
                documents = await coll.Find(filter).Sort(sort).Skip(skip).Limit(top).ToCursorAsync();
            }

            return documents;
        }

        /// <summary>
        /// Returns documents that match the specified criteria.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="fields"></param>
        /// <param name="query"></param>
        /// <param name="top"></param>
        /// <param name="skip"></param>
        /// <param name="sortExpression"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<IList<T>> ExecuteQuery<T>(string collectionName, ICollection<string>? fields, /*string partitionKey,*/ Query query, int top, int skip, SortExpression sortExpression = null, DateTime? dateTime = null)
        {
            var documents = await ExecuteQueryInternal(collectionName, fields, query, top, skip, sortExpression, dateTime);

            var results = new List<T>();

            foreach (var bsonDoc in documents.ToEnumerable())
            {
                T? doc = BsonSerializer.Deserialize<T>(bsonDoc);

                if (bsonDoc.Contains("Id"))
                    // This is required, because the default mapper of the Mongo C# Driver does not correctlly map BsonDoc._id to T.Id.
                    ((dynamic)doc!).Id = bsonDoc["Id"].ToString();
                else if (bsonDoc.Contains("_id"))
                    // This is required, because the default mapper of the Mongo C# Driver does not correctlly map BsonDoc._id to T.Id.
                    ((dynamic)doc!).Id = bsonDoc["_id"].ToString();

                results.Add(doc);
            }

            return results;
        }


        /// <summary>
        ///  Looks up the set of documents.  
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="fields"></param>
        /// <param name="query"></param>
        /// <param name="top"></param>
        /// <param name="skip"></param>
        /// <param name="sortExpression"></param>
        /// <param name="dateTime"></param>
        /// <returns>TODO..</returns>
        public async Task<IList<ExpandoObject>> ExecuteQuery(string collectionName, ICollection<string>? fields, /*string partitionKey,*/ Query query, int top, int skip, SortExpression? sortExpression = null, DateTime? dateTime = null)
        {
            var documents = await ExecuteQueryInternal(collectionName, fields, query, top, skip, sortExpression, dateTime);

            var results = new List<ExpandoObject>();

            foreach (var doc in documents.ToEnumerable())
            {
                dynamic dynRow = BsonSerializer.Deserialize<dynamic>(doc);

                ExpandoObject expando = dynRow;

                results.Add(expando);
            }

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
            var nestedQueries = query.Fields.Where(f => f.FieldName.Contains('.')).GroupBy(f => f.FieldName.Split('.')[0]);

            if (query.IsOrOperator)
            {
                foreach (var field in query.Fields)
                {
                    if (IsExpressionForNestedProperty(nestedQueries, field))
                        continue;

                    if (field.FieldName.ToLower() == "id")
                        field.FieldName = "_id";

                    filter |= BuildOrFilter(field);
                }

                filter = BuildORFilterForNestedProps(filter, nestedQueries);
            }
            else
            {
                foreach (var field in query.Fields)
                {
                    if (field.FieldName.ToLower() == "id")
                        field.FieldName = "_id";

                    if (IsExpressionForNestedProperty(nestedQueries, field))
                        continue;

                    filter &= BuildAndFilter(field);
                }

                filter = BuildANDFilterForNestedProps(filter, nestedQueries);
            }

            return filter;
        }


        /// <summary>
        /// Field NumOfElements > 0
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private static FilterDefinition<BsonDocument> BuildNumOfElementsForOR(FieldExpression q)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");

            foreach (var arg in q.Argument)
            {
                orFilter |= Builders<BsonDocument>.Filter.AnyLt(q.FieldName, arg);
            }

            return orFilter;
        }

        /// <summary>
        /// Field NumOfElements > 0
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private static FilterDefinition<BsonDocument> BuildNumOfElementsForAND(FieldExpression q)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");
            //Filter = Builders<MySchema>.Filter.AnyIn("Entries.$[].Categories", CategoryFilters);
            foreach (var arg in q.Argument)
            {
                orFilter &= Builders<BsonDocument>.Filter.Lte(q.FieldName, arg);
            }

            return orFilter;
        }


        private static FilterDefinition<BsonDocument> BuildANDFilterForNestedProps(FilterDefinition<BsonDocument> filter, IEnumerable<IGrouping<string, FieldExpression>> nestedQueries)
        {
            foreach (var nestedQuery in nestedQueries)
            {
                if (nestedQuery.Count() == 1)
                    continue;

                FilterDefinition<BsonDocument> andFilter = FilterDefinition<BsonDocument>.Empty;
                foreach (var nestedExpr in nestedQuery)
                {
                    var f = new FieldExpression
                    {
                        FieldName = nestedExpr.FieldName.Split('.')[1],
                        Argument = nestedExpr.Argument,
                        Operator = nestedExpr.Operator
                    };

                    andFilter &= BuildAndFilter(f);
                }

                filter &= Builders<BsonDocument>.Filter.ElemMatch<BsonDocument>(nestedQuery.Key, andFilter);
            }

            return filter;
        }

        private static FilterDefinition<BsonDocument> BuildORFilterForNestedProps(FilterDefinition<BsonDocument> filter, IEnumerable<IGrouping<string, FieldExpression>> specialQueries)
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

        private static bool IsExpressionForNestedProperty(IEnumerable<IGrouping<string, FieldExpression>> specialQueries, FieldExpression q)
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
            if (q.FieldName.StartsWith("Count("))
            {
                return CountFncBuilder.BuildAndFilter(q);
            }
            else
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
                    return BuildNotEqANDFilter(q);
                else if (q.Operator == QueryOperator.Empty)
                    return BuildOrEmptyFilter(q);
                else if (q.Operator == QueryOperator.GreaterThanEqualTo)
                    return BuildOrGteFilter(q);
                else if (q.Operator == QueryOperator.LessThanEqualTo)
                    return BuildOrLteFilter(q);
                else
                    throw new NotSupportedException($"The operator {q.Operator} is not supported.");
            }
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
            if (q.FieldName.StartsWith("Count("))
            {
                return CountFncBuilder.BuildOrFilter(q);
            }
            else
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
                    return BuildNotEqORFilter(q);
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
        }

        private static FilterDefinition<BsonDocument> BuildOrContainsFilter(FieldExpression qField)
        {
            var orFilter = Builders<BsonDocument>.Filter.In(qField.FieldName, qField.Argument.Select(arg =>
            {
                var text = arg == null ? string.Empty : arg.ToString();

                return new BsonRegularExpression(GetCorrectRegexPattern(text), "i");
            }));

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildOrEqFilter(FieldExpression qField)
        {
            var emptyValues = new List<object> { "", null, new List<object>() };

            var arguments = qField.Argument;

            if (qField.Argument.Intersect(emptyValues).Any())
            {
                arguments = qField.Argument.Union(emptyValues).ToList();
            }

            var orFilter = Builders<BsonDocument>.Filter.In(qField.FieldName, arguments);

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

            orFilter |= Builders<BsonDocument>.Filter.In(qField.FieldName, qField.Argument.Select(arg => new BsonRegularExpression($"^{GetCorrectRegexPattern(arg.ToString()!)}", "i")));

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildNotEqORFilter(FieldExpression qField)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");

            var emptyValues = new List<object> { "", null!, new List<object>() };

            var arguments = qField.Argument;

            if (qField.Argument.Intersect(emptyValues).Any())
            {
                arguments = qField.Argument.Union(emptyValues).ToList();
            }
            orFilter |= Builders<BsonDocument>.Filter.Nin(qField.FieldName, arguments);

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildNotEqANDFilter(FieldExpression qField)
        {
            //FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match"); ;

            var emptyValues = new List<object> { "", null!, new List<object>() };

            var arguments = qField.Argument;

            if (qField.Argument.Intersect(emptyValues).Any())
            {
                arguments = qField.Argument.Union(emptyValues).ToList();
            }

            var orFilter = Builders<BsonDocument>.Filter.Nin(qField.FieldName, arguments);

            return orFilter;
        }

        internal static string GetCorrectRegexPattern(string inputPattern)
        {
            var pattern = Regex.Replace(inputPattern, "(?=[\\.\\^\\$\\*\\+\\-\\?\\(\\)\\[\\]\\{\\}\\\\\\|\\—\\/])", "\\");
            return pattern;
        }


        /// <summary>
        /// Gets the document by id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="id"></param>
        /// <param name="idEntityName">Default value _id. Additionally any other alternative property can be used as id.</param>
        /// <returns>The single entity.</returns>
        public async Task<T> GetByIdAsync<T>(string collectionName, string id, string? idEntityName = "_id")
        {
            var coll = GetCollection(collectionName);

            var filter = Builders<BsonDocument>.Filter.Eq(idEntityName, id);

            var document = await coll.Find(filter).FirstOrDefaultAsync();

            if (document == null)
            {
                // Document not found, return null or throw an exception as needed.
                return default(T);
            }

            // Deserialize the BsonDocument to the desired type T.
            return BsonSerializer.Deserialize<T>(document);
        }

        /// <summary>
        /// Checks if the document ecists in the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="id"></param>
        /// <param name="idEntityName">Default value _id. Additionally any other alternative property can be used as id.</param>
        /// <returns>True if document exists.</returns>
        public async Task<bool> IsExistAsync<T>(string collectionName, string id, string? idEntityName = "_id")
        {
            var coll = GetCollection(collectionName);

            var filter = Builders<BsonDocument>.Filter.Eq(idEntityName, id);

            var projection = new BsonDocument();

            projection.Add(new BsonElement("_id", 0));

            var document = await coll.Find(filter).Limit(1).Project(projection).ToCursorAsync();

            if (document == null)
            {
                return false;
            }
            else
            {
                return true;
            }
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
private async Task<UpdateResult> UpdateAsync(string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> updateDefinition)
{
    var coll = GetCollection(collectionName);

    UpdateResult result = await coll.UpdateManyAsync(filter, updateDefinition);

    return result;
}


        /// <summary>
        /// Finds a user in the 'users' collection by their ObjectId.
        /// </summary>
        /// <param name="objectId">The ObjectId of the user to be found.</param>
        /// <returns>A task representing the asynchronous operation, which upon completion returns the User if found, or null if not found.</returns>
        public async Task<Apollo.Common.Entities.User> FindUserByObjectIdAsync(string objectId)
        {
            var coll = GetCollection("users");

            var filter = Builders<BsonDocument>.Filter.Eq("ObjectId", objectId);

            var document = await coll.Find(filter).FirstOrDefaultAsync();

            if (document == null)
            {
                // User not found with given ObjectId, return null or handle as needed.
                return null;
            }

            // Deserialize the BsonDocument to User type.
            return BsonSerializer.Deserialize<Apollo.Common.Entities.User>(document);
        }


        /// <summary>
        /// Finds a user in the 'users' collection by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to be found.</param>
        /// <returns>A task representing the asynchronous operation, which upon completion returns the User if found, or null if not found.</returns>
        public async Task<Apollo.Common.Entities.User> FindUserByEmailAsync(string email)
        {
            var coll = GetCollection("users"); 

            var filter = Builders<BsonDocument>.Filter.Eq("Email", email);

            var document = await coll.Find(filter).FirstOrDefaultAsync();

            if (document == null)
            {
                // User not found with given email, return null or handle as needed.
                return null;
            }

            // Deserialize the BsonDocument to User type.
            return BsonSerializer.Deserialize<Apollo.Common.Entities.User>(document);
        }
    }
}
