// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Apollo.SemanticSearchWorker
{
    /// <summary>
    /// Defines the formatter which transform the apollo entity instance to a string.
    /// </summary>
    public interface IEntityFormatter
    {
        /// <summary>
        /// Transforms the Apollo Entity to string.
        /// </summary>
        /// <typeparam name="T">Apollo Entity to be exported.</typeparam>
        /// <param name="entityInstance"></param>
        /// <returns>Id|Url|Title|Text</returns>
        IList<string> FormatObject(object entityInstance);
    }

    internal class ProfileFormatter : IEntityFormatter
    {
        public IList<string> FormatObject(object entityInstance) => throw new NotImplementedException();
    }
}
