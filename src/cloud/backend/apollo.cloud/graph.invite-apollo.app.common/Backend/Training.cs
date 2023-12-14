// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;

namespace Apollo.Common.Entities
{
    public class Training
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        [Newtonsoft.Json.JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// External Identifier from the Training Providers.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("providerId")]
        public string ProviderId { get; set; }

        //[Required]
        //[BsonElement("Title")]
        [Newtonsoft.Json.JsonProperty("trainingName")]
        public string TrainingName { get; set; }

        /// <summary>
        /// The image of a training.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("image")]
        public Uri? Image { get; set; }

        //[BsonElement("Description")]
        [Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }

        //[BsonElement("ShortDescription")]
        [Newtonsoft.Json.JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Training agenda.
        /// </summary>
        //[BsonElement("Content")]
        [Newtonsoft.Json.JsonProperty("content")]
        public List<string> Content { get; set; }

        /// <summary>
        /// Specifies the list of benefits of the training.
        /// </summary>
        //[BsonElement("Benefit")]
        [Newtonsoft.Json.JsonProperty("benefitList")]
        public List<string> BenefitList { get; set; }

        /// <summary>
        /// Specifies the list of certificates that can be obtained after the training.
        /// </summary>
        //[BsonElement("Certificate")]
        [Newtonsoft.Json.JsonProperty("certificate")]
        public List<string> Certificate { get; set; }

        //TODO: Maybe Flagged Enum for Certification Type
        //[Flags]
        //public enum Certification;

        /// <summary>
        /// The list of recommended prerequisites for the training  
        /// </summary>
        //[BsonElement("Prerequisites")]
        [Newtonsoft.Json.JsonProperty("prerequisites")]
        public List<string> Prerequisites { get; set; }

        /// <summary>
        /// Financial Aid provided for a Training
        /// </summary>
        //[BsonElement("Loans")]
        [Newtonsoft.Json.JsonProperty("loans")]
        public List<Loans>? Loans { get; set; }

        //[BsonElement("TrainingsProvider")]
        [Newtonsoft.Json.JsonProperty("trainingProvider")]
        public EduProvider TrainingProvider { get; set; }

        //[BsonElement("CourseProvider")]
        [Newtonsoft.Json.JsonProperty("courseProvider")]
        public EduProvider CourseProvider { get; set; }

        //[BsonElement("AppointmentUrl")]
        [Newtonsoft.Json.JsonProperty("appointment")]
        public List<Appointment> Appointment { get; set; }

        [Newtonsoft.Json.JsonProperty("targetAudience")]
        public string TargetAudience { get; set; }

        /// <summary>
        /// Training Provider Url or Target
        /// </summary>
        //[BsonElement("ProductUrl")]
        [Newtonsoft.Json.JsonProperty("productUrl")]
        public Uri ProductUrl { get; set; }

        /// <summary>
        /// Defined as City + Contact
        /// </summary>
        // [BsonElement("Contacts")]
        [Newtonsoft.Json.JsonProperty("contacts")]
        public Dictionary<string, Contact> Contacts { get; set; }

        /// <summary>
        /// The type of the training.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("trainingType")]
        public TrainingType TrainingType { get; set; }


        // It should be bool but what do I know about education ofc it is not bool
        //[BsonElement("IndividualStartDate")]
        [Newtonsoft.Json.JsonProperty("individualStartDate")]
        public string? IndividualStartDate { get; set; }

        //TODO: Review
        /// <summary>
        /// Maybe we should make this a class of its own?
        /// Since comparison is done by more information such as where does it happens, what does it include ...
        /// </summary>
        //[BsonElement("Price")]
        [Newtonsoft.Json.JsonProperty("price")]
        public decimal? Price { get; set; }

        // [BsonElement("PriceDescription")]
        [Newtonsoft.Json.JsonProperty("priceDescription")]
        public string PriceDescription { get; set; }

        //[BsonElement("Accessibility")]
        [Newtonsoft.Json.JsonProperty("accessibilityAvailable")]
        public bool AccessibilityAvailable { get; set; }

        #region Metadata

        // [BsonElement("SeoTags")]
        [Newtonsoft.Json.JsonProperty("tags")]
        public List<string> Tags { get; set; }

        //[BsonElement("Categories")]
        [Newtonsoft.Json.JsonProperty("categories")]
        public List<string> Categories { get; set; }

        //TODO: Review Not to be set by the API, API may Use Id room of Training Provider and ExternalTrainingId
        /// <summary>
        /// These are Apollo Ids not External Ids !!!
        /// </summary>
        [Newtonsoft.Json.JsonProperty("similarTrainings")]
        public List<Training> SimilarTrainings { get; set; }

        /// <summary>
        /// Apollo Internal Id only to be set by the Backend!
        /// </summary>
        [Newtonsoft.Json.JsonProperty("recommendedTrainings")]
        public List<Training> RecommendedTrainings { get; set; }

        #endregion

#warning candidate for interface!!
        #region IContentPublising // todo.

        //[BsonElement("PublishingDate")]
        [Newtonsoft.Json.JsonProperty("publishingDate")]
        public DateTime PublishingDate { get; set; }
        //[BsonElement("UnpublishingDate")]
        [Newtonsoft.Json.JsonProperty("unpublishingDate")]
        public DateTime UnpublishingDate { get; set; }
        //[BsonElement("Successor")]
        [Newtonsoft.Json.JsonProperty("successor")]
        public string? Successor { get; set; }
        //[BsonElement("Predecessor")]
        [Newtonsoft.Json.JsonProperty("predecessor")]
        public string? Predecessor { get; set; }

        #endregion


    }
}
