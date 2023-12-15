// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Daenet.MongoDal.Entitties;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Daenet.MongoDal.Functions
{
    /// <summary>
    /// Implements the expression builder for the Count function.
    /// </summary>
    internal class CountFncBuilder
    {

        /// <summary>
        /// Builds the filter for Count function for OR operator between arguments.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static FilterDefinition<BsonDocument> BuildOrFilter(FieldExpression q)
        {
            if (q.FieldName.StartsWith("Count("))
            {
                string pattern = @"^Count\((\w+)\)$";

                // Use Regex.Match to find the matches
                Match match = Regex.Match(q.FieldName, pattern);

                // Check if a match is found
                if (match.Success)
                {
                    // Extract the field name from the match
                    string fieldName = match.Groups[1].Value;

                    if (q.Operator == QueryOperator.Equals)
                        return BuildCountEqualsForOR(fieldName, q.Argument);
                    else if (q.Operator == QueryOperator.LessThan)
                        return BuildCountLTForORAND(fieldName, q.Argument);
                    else if (q.Operator == QueryOperator.GreaterThan)
                        return BuildCountGTForORAND(fieldName, q.Argument);
                    else if (q.Operator == QueryOperator.Contains)
                        return BuildCountContainsOR(fieldName, q.Argument);
                    else
                        throw new NotSupportedException($"Unsuported Operator {q.Operator}!");
                }
                else
                    throw new NotSupportedException($"The function Count() cannot be parsed!");
            }
            else
                throw new NotSupportedException($"The function Count() cannot be found!");
        }


        /// <summary>
        /// Builds the filter for Count function for AND operator between arguments.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static FilterDefinition<BsonDocument> BuildAndFilter(FieldExpression q)
        {
            if (q.FieldName.StartsWith("Count("))
            {
                string pattern = @"^Count\((\w+)\)$";

                // Use Regex.Match to find the matches
                Match match = Regex.Match(q.FieldName, pattern);

                // Check if a match is found
                if (match.Success)
                {
                    // Extract the field name from the match
                    string fieldName = match.Groups[1].Value;

                    if (q.Operator == QueryOperator.Equals)
                        return BuildCountEqualsForAnd(fieldName, q.Argument);
                    else if (q.Operator == QueryOperator.LessThan)
                        return BuildCountLTForORAND(fieldName, q.Argument);
                    else if (q.Operator == QueryOperator.GreaterThan)
                        return BuildCountGTForORAND(fieldName, q.Argument);
                    else if (q.Operator == QueryOperator.Contains)
                        return BuildCountContainsAND(fieldName, q.Argument);
                    else
                        throw new NotSupportedException($"Unsuported Operator {q.Operator}!");
                }
                else
                    throw new NotSupportedException($"The function Count() cannot be parsed!");
            }
            else
                throw new NotSupportedException($"The function Count() cannot be found!");
        }


        /// <summary>
        /// Field NumOfElements > 0
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private static FilterDefinition<BsonDocument> BuildCountEqualsForOR(string fieldName, ICollection<object> args)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");

            foreach (var arg in args)
            {
                orFilter |= Builders<BsonDocument>.Filter.AnyEq(fieldName, arg);
            }

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildCountLTForORAND(string fieldName, ICollection<object> args)
        {
            if (args.Count == 0)
                throw new ArgumentException($"The Count() function supports only one argument for the operators {QueryOperator.LessThan} and {QueryOperator.GreaterThan}!");

            if (args.Count > 1)
                throw new ArgumentException($"The Count() function supports only one argument for the operators {QueryOperator.LessThan} and {QueryOperator.GreaterThan}!");

            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.SizeGt(fieldName, (int)args.First());

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildCountGTForORAND(string fieldName, ICollection<object> args)
        {
            if (args.Count == 0)
             throw new ArgumentException($"The Count() function supports only one argument for the operators {QueryOperator.LessThan} and {QueryOperator.GreaterThan}!");

            if (args.Count > 1)
                throw new ArgumentException($"The Count() function supports only one argument for the operators {QueryOperator.LessThan} and {QueryOperator.GreaterThan}!");

            var val = (int)args.First();

            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.SizeGt(d => d["Loans"], val);

            // FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.SizeGt(fieldName, (int)args.First());

           // FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Where(doc => doc["Loans"].AsBsonArray.Count > val);

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildCountContainsOR(string fieldName, ICollection<object> args)
        {
            var orFilter = Builders<BsonDocument>.Filter.AnyIn(fieldName, args.Select(arg =>
            {
                var text = arg == null ? string.Empty : arg.ToString();

                return new BsonRegularExpression(MongoDataAccessLayer.GetCorrectRegexPattern(text), "i");
            }));

            return orFilter;
        }


        /// <summary>
        /// Field NumOfElements > 0
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private static FilterDefinition<BsonDocument> BuildCountEqualsForAnd(string fieldName, ICollection<object> args)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");

            foreach (var arg in args)
            {
                orFilter |= Builders<BsonDocument>.Filter.AnyEq(fieldName, arg);
            }

            return orFilter;
        }

        private static FilterDefinition<BsonDocument> BuildCountContainsAND(string fieldName, ICollection<object> args)
        {
            FilterDefinition<BsonDocument> orFilter = Builders<BsonDocument>.Filter.Eq("_id", "should not match");

            foreach (var arg in args)
            {
                var text = arg == null ? string.Empty : arg.ToString();

                orFilter &= Builders<BsonDocument>.Filter.AnyEq(fieldName, MongoDataAccessLayer.GetCorrectRegexPattern(text!));
            }

            return orFilter;
        }

    }
}
