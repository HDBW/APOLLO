// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

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
        Equals = 0,

        /// <summary>
        /// For strings only
        /// </summary>
        Contains = 1,

        /// <summary>
        /// For strings only
        /// </summary>
        StartsWith = 2,

        /// <summary>
        /// 
        /// </summary>
        GreaterThan = 3,

        /// <summary>
        /// 
        /// </summary>
        LessThan = 4,

        /// <summary>
        /// 
        /// </summary>
        NotEquals = 5,

        /// <summary>
        /// FieldXY IN ("A", "B", "C")
        /// </summary>
        In = 6,

        /// <summary>
        /// Target empty value for string only
        /// </summary>
        Empty = 7,

        GreaterThanEqualTo = 8,

        LessThanEqualTo = 9,

    }
}
