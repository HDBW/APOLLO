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
    /// Used as a general entity to store a list of items of some type.
    /// </summary>
    public class List
    {
        public string Id { get; set; }

        /// <summary>
        /// Specifies the typy of the item like Qualification, Skill, etc.
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// List of items in all languages.
        /// </summary>
        public List<ListItem> Items{ get; set; }
    }


    /// <summary>
    ///  The item stored in the DB.
    ///  Describes the item in the list in the specific language.
    /// </summary>
    public class ListItem
    {
        /// <summary>
        /// The ISO language code.
        /// </summary>
        public string Lng { get; set; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the item.
        /// </summary>
        public string Description { get; set; }
    }
}
