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
    /// Defines the base type for all types that are implemented as ApolloList.
    /// </summary>
    public class ApolloListBase
    {
        /// <summary>
        /// Identifier of the item in the list.
        /// </summary>
        public int ListItemId{ get; set; }

        /// <summary>
        /// The value which is used to identify the item in the list.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The ISO language code.
        /// </summary>
        public required string Lng { get; set; }

    }
}
