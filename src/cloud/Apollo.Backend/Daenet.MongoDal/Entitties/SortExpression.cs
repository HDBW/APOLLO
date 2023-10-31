using System;


namespace Daenet.MongoDal.Entitties
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
