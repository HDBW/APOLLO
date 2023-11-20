// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// Define the sorting operation over a certain field, order ascending: 1, descending: -1
    /// </summary>
    public class SortExpression
    {
        /// <summary>
        /// Field name to be sort
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// sort order
        /// </summary>
        public SortOrder Order { get; set; }

        public override string ToString()
        {
            return $"Field={FieldName}-{Enum.GetName(Order)}";
        }
    }

    /// <summary>
    /// Define the possible sort order
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// Ascending order
        /// </summary>
        Ascending = 1,

        /// <summary>
        /// Descending order
        /// </summary>
        Descending = -1
    }
}
