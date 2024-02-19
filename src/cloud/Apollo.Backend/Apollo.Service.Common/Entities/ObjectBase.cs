// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// The base type for all types that are used as collections in the database.
    /// For example: User, Training, Profile.
    /// </summary>
    public class ObjectBase
    {
        /// <summary>
        /// This is the Unique Identifier set by Apollo for the object.
        /// </summary>
        public string? Id { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ChangedAt { get; set; }

        public string? ChangedBy { get; set; }


        /// <summary>
        /// Returns the principal identifier. We use currentlly OID claim, which is the ObjectId of the user in AAD.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetIdClaim(ClaimsPrincipal principal)
        {
            var oid = principal.Claims.Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").FirstOrDefault()?.Value;
            if (oid == null)
                oid = principal.Identity.Name;

            return oid;
        }
    }
}
