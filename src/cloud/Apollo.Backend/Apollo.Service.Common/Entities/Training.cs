// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

namespace Apollo.Common.Entities
{
    public class Training
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ProviderId { get; set; }

        //Note we need a longid for mobile clients should we convert to longid in mongo?
        // I guess there is a converation from objectid to type long?
        //[BsonElement("MobileId")]
        //public long? MobileId { get; set; }

        //[Required]
        //[BsonElement("Title")]
        public string TrainingName { get; set; }

        /// <summary>
        /// External Identifier for the Training Providers.
        /// </summary>
        //[Required]
        //[BsonElement("Identifier")]
        //public string Identifier { get; set; }


        //[BsonElement("Description")]
        public string Description { get; set; }

        //[BsonElement("ShortDescription")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Training agenda.
        /// </summary>
        //[BsonElement("Content")]
        public List<string> Content { get; set; }

        /// <summary>
        /// Specifies the list of benefits of the training.
        /// </summary>
        //[BsonElement("Benefit")]
        public List<string> BenefitList { get; set; }


        /// <summary>
        /// Specifies the list of certificates that can be obtained after the training.
        /// </summary>
        //[BsonElement("Certificate")]
        public List<string> Certificate { get; set; }

        //TODO: Maybe Flagged Enum for Certification Type
        //[Flags]
        //public enum Certification;

        /// <summary>
        /// The list of recommended prerequisites for the training  
        /// </summary>
        //[BsonElement("Prerequisites")]
        public List<string> Prerequisites { get; set; }

        /// <summary>
        /// Financial Aid provided for a Training
        /// </summary>
        //[BsonElement("Loans")]
        public List<Loans> Loans { get; set; }

        //[BsonElement("TrainingsProvider")]
        public EduProvider TrainingProvider { get; set; }

        //[BsonElement("CourseProvider")]
        public EduProvider CourseProvider { get; set; }

        //[BsonElement("Appointments")]
        public Appointments Appointments { get; set; }

        /// <summary>
        /// Training Provider Url or Target
        /// </summary>
        //[BsonElement("ProductUrl")]
        public Uri ProductUrl { get; set; }

        /// <summary>
        /// Defined as City + Contact
        /// </summary>
        // [BsonElement("Contacts")]
        Dictionary<string, Contact> Contacts { get; set; }

        /// <summary>
        /// The type of the training.
        /// </summary>
        public TrainingType TrainingType { get; set; }


        // It should be bool but what do I know about education ofc it is not bool
        //[BsonElement("IndividualStartDate")]
        public string IndividualStartDate { get; set; }

        //TODO: Review
        /// <summary>
        /// Maybe we should make this a class of its own?
        /// Since comparison is done by more information such as where does it happens, what does it include ...
        /// </summary>
        //[BsonElement("Price")]
        public decimal? Price { get; set; }

        // [BsonElement("PriceDescription")]
        public string PriceDescription { get; set; }

        //[BsonElement("Accessibility")]
        public bool AccessibilityAvailable { get; set; }

        #region Metadata

        // [BsonElement("SeoTags")]
        public List<string> Tags { get; set; }

        //[BsonElement("Categories")]
        public List<string> Categories { get; set; }

        #endregion

#warning candidate for interface!!
        #region IContentPublising // todo.

        //[BsonElement("PublishingDate")]
        public DateTime PublishingDate { get; set; }
        //[BsonElement("UnpublishingDate")]
        public DateTime UnpublishingDate { get; set; }
        //[BsonElement("Successor")]
        public string? Successor { get; set; }
        //[BsonElement("Predecessor")]
        public string? Predecessor { get; set; }

        #endregion


    }
}
