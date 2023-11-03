
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daenet.MongoDal.Entitties
{
    /// <summary>
    /// Defines the query operation on the specified field.
    /// For example: FieldName="Name", Operator=Contains, Expression=abc, will lookup all records with the Name that contains 'ABC'.
    /// </summary>
    public class FieldExpression
    {
        /// <summary>
        /// The name of the field.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// The Operator on the field.
        /// </summary>
        public QueryOperator Operator { get; set; }

        /// <summary>
        /// The argument of the operation. Query executes the OR operator over all specified arguments in the collection.
        /// </summary>
        [JsonConverter(typeof(JsonObjectArrayConverter))]
        public ICollection<object> Argument { get; set; }

        /// <summary>
        /// If is set to true, the result is returned as distinct
        /// </summary>
        public bool Distinct { get; set; }
    }
}
