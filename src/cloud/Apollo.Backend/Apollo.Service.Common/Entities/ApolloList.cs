// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Apollo.Common.Entities
{

    /// <summary>
    /// Used as a name for dealing with Apollo Lists.
    /// The type we use here is called <see cref="ApolloList"/>. However we do not use this type as th ename of
    /// the cllection inside MongoDb. This is why we introduced this type.
    /// </summary>
    public class List
    {

    }

    /// <summary>
    /// Used as a general entity to store a list of items of some type.
    /// </summary>
    [DataContract]
    public class ApolloList
    {
        /// <summary>
        /// List identifier. Example: <see cref="ApolloApi.CreateListId"/>"/>
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Specifies the type  of the item like Qualification, Skill, etc.
        /// Also used as a partition key.
        /// </summary>
        [DataMember]
        public required string ItemType { get; set; }

        /// <summary>
        /// Description of the item, optional.
        /// </summary>
        [DataMember]
        public string? Description { get; set; }

        /// <summary>
        /// List of items in all languages.
        /// </summary>
        [DataMember]
        public  List<ApolloListItem>? Items{ get; set; }
    }


    /// <summary>
    ///  The item stored in the DB.
    ///  Describes the item in the list in the specific language.
    /// </summary>
    [DataContract]
    public class ApolloListItem
    {
        /// <summary>
        /// Identifier of the item in the list.
        /// If set on -1, the item is new and has to be created.
        /// </summary>
        [DataMember]
        public required int ListItemId { get; set; }

        /// <summary>
        /// The ISO language code.
        /// </summary>
        [DataMember]
        public string? Lng { get; set; }

        /// <summary>
        /// Optionl description of the values.
        /// </summary>
        [DataMember]
        public string? Description { get; set; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        [DataMember]
        public required string Value { get; set; }
   
    }


    /// <summary>
    /// User can enter new values.
    /// </summary>
    public class ExtendableApolloListItem : ApolloListItem
    {

    }
}
