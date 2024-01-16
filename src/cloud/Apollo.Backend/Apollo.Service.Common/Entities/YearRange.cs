// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// Please note the client uses the following enum
    /// public enum YearRange
    /// {
    ///     Unknown = 0,
    ///     // bis 2 Jahre
    ///     LessThan2 = 1,
    ///     //2 bis 5 Jahre
    ///     Between2And5 = 2,
    ///     //mehr als 5 Jahre
    ///     MoreThan5 = 2,
    /// }
    /// </summary>
    public class YearRange
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public CultureInfo CultureInfo { get; set; }
    }
}
