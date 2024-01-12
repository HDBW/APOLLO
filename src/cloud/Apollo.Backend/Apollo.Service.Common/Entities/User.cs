﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Runtime.Serialization;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Apollo.Common.Entities
{
    public class User
    {
        /// <summary>
        /// This is the Unique Identifier set by Apollo for the User.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// This is the Unique Identifier of the User from the Identity Provider.
        /// Object ID
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// This is the User principal name given by AADB2C.
        /// </summary>
        public string? Upn { get; set; }

        /// <summary>
        /// This is the email address given by the AADB2C claim.
        /// Register user Email Claim - That is the only way for the user to persist the Account if Phone Provider changes.
        /// 
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// The Name is the Display Name of the User
        /// It is aquired via the Identity Provider and can be changed by the User during Registration as well as in the Profile.
        /// PII
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Contact> ContactInfos { get; set; }

        /// <summary>
        /// Indicates the Birthdate of the User
        /// Optional Information
        /// PII
        /// </summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// This indicates if the User has a Disability
        /// We do not classify disabilities but only indicate if the User has one or not.
        /// This is relevant for the User to get the right Information and for the Admins to know if the User needs special treatment.
        /// </summary>
        public bool? Disabilities { get; set; }

        public Profile? Profile { get; set; }
    }
}
