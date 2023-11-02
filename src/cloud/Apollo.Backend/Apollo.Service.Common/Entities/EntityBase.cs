using Cpdm.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Apollo.Common.Entities
{
    //[JsonConverter(typeof(JsonEntityBaseConverter))]
    public abstract class EntityBase
    {
        public virtual string ObjectId { get; set; }

        public string ObjectNo { get; set; }

        public string BrandNo { get; set; }

        private string documentType;

        public string DocumentType { get => GetType().Name; set { documentType = value; } }

        public DateTime? CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ChangedAt { get; set; }

        public string ChangedBy { get; set; }

        public EntityBase()
        {

        }

        /// <summary>
        /// The user identity who checkedout the document.
        /// NULL if the document is checked in.
        /// </summary>
        public string CheckedOutBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">Array of all key values.</param>
        public EntityBase(params string[] args)
        {
            if (args.Length < 2)
                throw new ArgumentException("Brand and ObjectNumber must be specified.");

            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                    this.BrandNo = args[i];

                if (i == 1)
                    this.ObjectNo = args[i];
            }

            SetId(args);
        }

        public static void CopyStamps(EntityBase src, EntityBase dst)
        {
            dst.BrandNo = src.BrandNo;
            dst.ObjectNo = src.ObjectNo;
            dst.ChangedAt = src.ChangedAt;
            dst.ChangedBy = src.ChangedBy;
            dst.CreatedAt = src.CreatedAt;
            dst.CreatedBy = src.CreatedBy;
            dst.ObjectId = src.ObjectId;
            dst.CheckedOutBy = src.CheckedOutBy;
        }


        protected virtual void SetId(string[] args)
        {
            StringBuilder sb = new StringBuilder(args[0]);//brand

            for (int i = 1; i < args.Length; i++)
            {
                sb.Append('-');
                sb.Append(args[i]);
            }

            this.ObjectId = sb.ToString();
            this.ObjectId = Regex.Replace(this.ObjectId, @"\+", "");
        }

        protected void ValidateObjectId(EntityBase entity = null)
        {
            if (Regex.IsMatch(this.ObjectId, "[^A-Za-z0-9-._/äöüÄÖÜß ]"))
            {
                throw new CpdmException("Object Id cannot contain special character");
            }
            if (entity?.ObjectId != null && entity.ObjectId != this.ObjectId)
            {
                throw new CpdmException("ObjectId is not valid. It should be null or matched the generated object Id.");
            }
        }


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


        /// <summary>
        /// Checks if the docuement is checked out by somebody else. If it is checked out by some other user, this method throws.
        /// </summary>
        /// <param name="brandNo"></param>
        /// <param name="objectId"></param>
        /// <param name="principal"></param>
        /// <param name="dal"></param>
        /// <exception cref="InvalidOperationException">Throws if docuemnt</exception>

        public static async Task ThrowOnCheckedOut(string entityType, string brandNo, string objectId, ClaimsPrincipal principal, ICpdmDataAccessLayer dal)
        {
            var checkedOut = await VaidateCheckOut(entityType, brandNo, objectId, principal, dal);
            // throw if not the same user, throw if checked out by null
            // do nothing if checked out by the same user

            //
            // No instance has been found in the database
            if (checkedOut.user == null)
                return;

            //
            // The document is checked out by the different user
            if (checkedOut.isCheckedOut == false)
            {
                if (checkedOut.user == string.Empty)
                    throw new InvalidOperationException($"The document is checked in and cannot be updated.");

                throw new InvalidOperationException($"The document is checked out by {checkedOut.user} and cannot be updated.");
            }
        }


        /// <summary>
        /// Gets the name of the user who checked out the entity.
        /// </summary>
        /// <param name="brandNo"></param>
        /// <param name="objectId"></param>
        /// <param name="principal"></param>
        /// <param name="dal"></param>
        /// <returns>The name of the user who checked out the document. <see cref="string.Empty"/> if the document is not checked out.
        /// NULL if document is not found.
        /// </returns>
        public static async Task<string> GetCheckOutUser(string entityType, string brandNo, string objectId, ClaimsPrincipal principal, ICpdmDataAccessLayer dal)
        {
            List<string> field = new List<string>();
            field.Add(nameof(EntityBase.CheckedOutBy));

            var expObj = await dal.GetObject(entityType, brandNo, objectId);
            if (expObj != null)
                return GetCheckedOutUser(expObj);
            return null;
        }

        /// <summary>
        /// Get value of property <see cref="EntityBase.CheckedOutBy"/> from <see cref="ExpandoObject"/> as dynamic form of <see cref="EntityBase"/>
        /// </summary>
        /// <param name="expObj"></param>
        /// <returns>checked out user or <see cref="string.Empty"/> if the object is not checked out</returns>
        public static string GetCheckedOutUser(ExpandoObject expObj)
        {
            IDictionary<string, object> obj = expObj;
            object checkOutUser;

            obj.TryGetValue(nameof(CheckedOutBy), out checkOutUser);

            if (checkOutUser != null)
                return checkOutUser.ToString();
            else
                return string.Empty;
        }



        /// <summary>
        /// Validates if the object is already checkedout by somebody who is not me.
        /// </summary>
        /// <param name="brandNo"></param>
        /// <param name="objectId"></param>
        /// <param name="principal"></param>
        /// <param name="dal"></param>
        /// <returns>Tupple with checkedOut flag and the name of the user who checked out the document. <see cref="string.Empty"/> if the document is not checked out.
        /// NULL: If the document does not exist at all, the checkedOut flag will not be considered in this case.
        /// </returns>
        public static async Task<(bool isCheckedOut, string user)> VaidateCheckOut(string entityType, string brandNo, string objectId, ClaimsPrincipal principal, ICpdmDataAccessLayer dal)
        {
            var oid = GetIdClaim(principal);
            string user = await GetCheckOutUser(entityType, brandNo, objectId, principal, dal);
            return (user == oid, user);
            //if (user != oid)
            //    return n;
            //else
            //    return user;
        }
    }
}
