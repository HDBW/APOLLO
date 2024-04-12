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
    /// Used in the query methods to specifiy the operator.
    /// </summary>
    public enum QueryOperator
    {
        /// <summary>
        /// Equalty operator.
        /// </summary>
        Equals,

        /// <summary>
        /// For strings only
        /// </summary>
        Contains,

        /// <summary>
        /// For strings only
        /// </summary>
        StartsWith,

        /// <summary>
        /// 
        /// </summary>
        GreaterThan,

        /// <summary>
        /// 
        /// </summary>
        LessThan,

        /// <summary>
        /// 
        /// </summary>
        NotEquals,

        /// <summary>
        /// FieldXY IN ("A", "B", "C")
        /// </summary>
        In,

        /// <summary>
        /// Target empty value for string only
        /// </summary>
        Empty,

        GreaterThanEqualTo,

        LessThanEqualTo


    }
}
