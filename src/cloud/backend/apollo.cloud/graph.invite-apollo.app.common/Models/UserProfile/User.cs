// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class User
    {
        /// <summary>
        /// This is the Unique Identifier set by Apollo for the User.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// This is the Unique Identifier of the User from the Identity Provider.
        /// Object ID
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// The name of the identity provider.
        /// </summary>
        public string IdentityProvicer { get; set; }

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
        public List<Contact> ContactInfos { get; set; } = new List<Contact>();

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

        /// <summary>
        /// The user profile is the Profile of the User.
        /// </summary>
        public Profile? Profile { get; set; }
    }
}
