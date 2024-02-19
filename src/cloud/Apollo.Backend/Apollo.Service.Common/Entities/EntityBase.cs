// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


namespace Apollo.Common.Entities
{
    /// <summary>
    /// The base class for all entities whic are not stored to database collections directl.
    /// Such entities act like subtypes of objects derived from <see cref="ObjectBase"/>."/>
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// This is the Unique Identifier set by Apollo for the entity.
        /// </summary>
        public string? Id { get; set; }


        public EntityBase()
        {

        }

 

        //protected void ValidateObjectId(EntityBase entity = null)
        //{
        //    if (Regex.IsMatch(this.ObjectId, "[^A-Za-z0-9-._/äöüÄÖÜß ]"))
        //    {
        //        throw new CpdmException("Object Id cannot contain special character");
        //    }
        //    if (entity?.ObjectId != null && entity.ObjectId != this.ObjectId)
        //    {
        //        throw new CpdmException("ObjectId is not valid. It should be null or matched the generated object Id.");
        //    }
        //}



    }
}
