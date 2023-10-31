﻿namespace Daenet.MongoDal.Entitties
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
