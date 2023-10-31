using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daenet.MongoDal.Entitties
{
    /// <summary>
    /// Used in the query methods to specifiy the operator.
    /// </summary>
    public class Query
    {
        /// <summary>
        /// Specifies if FilterExpressions will be OR-ed or AND-ed.
        /// </summary>
        public bool IsOrOperator { get; set; } 

        /// <summary>
        /// List of fields joined in the query operation. Currentlly the AND operation across all fields is supported only.
        /// </summary>
        public List<FieldExpression> Fields { get; set; } = new List<FieldExpression>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Fields != null || Fields.Count == 0)
            {
                foreach (var item in this.Fields)
                {
                    if (item.Argument == null)
                        continue;

                    sb.Append($"[{item.FieldName} {item.Operator} {string.Join(',', item.Argument)}]");
                    if (item != this.Fields.Last())
                    {
                        if (IsOrOperator)
                        {
                            sb.Append(" OR ");
                        }
                        else
                        {
                            sb.Append(" AND ");
                        }
                    }
                }
            }
            else
                return "All items";

            return sb.ToString();
        }

        /// <summary>
        /// Create a query with single fieldName
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValues"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static Query CreateQuery(string fieldName, IList<object> fieldValues, QueryOperator op = QueryOperator.Equals, bool distinct = false)
        {
            return new Query
            {
                Fields = new List<FieldExpression>
                {
                    new FieldExpression
                    {
                        FieldName = fieldName, Operator = op, Argument = fieldValues, Distinct = distinct
                    }
                }
            };
        }

        //[Obsolete("Use AddExpression instead as it offers more flexibility")]
        /// <summary>
        /// Add new AND-conditional field expression to the query
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="op">compare operator</param>
        /// <returns>The query to support concatnation.</returns>
        //public Query AddAndExpression(string fieldName, object fieldValue, QueryOperator op = QueryOperator.Equals)
        //{
        //    this.Fields.Add(
        //         new FieldExpression
        //         {
        //             FieldName = fieldName,
        //             Operator = op,
        //             Argument = new List<object> { fieldValue },

        //         });

        //    return this;
        //}

        /// <summary>
        /// Add new AND-conditional field expression across all other fields and OR-conditional field expression within a fieldName to the query
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValues">list of all values with or condition</param>
        /// <param name="op"></param>
        /// <returns>The query to support concatnation.</returns>
        public Query AddExpression(string fieldName, ICollection<object> fieldValues, QueryOperator op = QueryOperator.Equals, bool distinct = false)
        {
            this.Fields.Add(new FieldExpression
            {
                FieldName = fieldName,
                Operator = op,
                Argument = fieldValues,
                Distinct = distinct
            });

            return this;
        }
    }

    public class QueryExpression
    {

    }
}
