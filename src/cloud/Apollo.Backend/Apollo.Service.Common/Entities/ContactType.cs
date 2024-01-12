// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace Apollo.Common.Entities
{
    public class ContactType
    {
        /// <summary>
        /// Unique Identifier of the Contact Type
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Contact Type Name - Client uses enum
        /// Unknown,
        /// Professional,
        /// Private,
        ///
        /// I would argue we use the same contact in the backend for training and user.
        /// So maybe we add the contact type:
        /// TrainingContact,
        /// </summary>
        public string Name { get; set; }

        ///// <summary>
        ///// Defines the Culture of the Contact Type
        ///// </summary>
        //public CultureInfo CultureInfo { get; set; }
    }
}
