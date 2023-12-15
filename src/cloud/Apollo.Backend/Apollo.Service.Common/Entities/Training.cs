﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

namespace Apollo.Common.Entities
{
    public class Training
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// External Identifier from the Training Providers.
        /// </summary>
        public string ProviderId { get; set; }

        //[Required]
        //[BsonElement("Title")]
        public string TrainingName { get; set; }

        /// <summary>
        /// The image of a training.
        /// </summary>
        public Uri? Image { get; set; }

        //[BsonElement("Description")]
        public string Description { get; set; }

        //[BsonElement("ShortDescription")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Training agenda.
        /// </summary>
        //[BsonElement("Content")]
        public List<string>? Content { get; set; }

        /// <summary>
        /// Specifies the list of benefits of the training.
        /// </summary>
        //[BsonElement("Benefit")]
        public List<string>? BenefitList { get; set; }

        /// <summary>
        /// Specifies the list of certificates that can be obtained after the training.
        /// </summary>
        //[BsonElement("Certificate")]
        public List<string>? Certificate { get; set; }

        //TODO: Maybe Flagged Enum for Certification Type
        //[Flags]
        //public enum Certification;

        /// <summary>
        /// The list of recommended prerequisites for the training  
        /// </summary>
        //[BsonElement("Prerequisites")]
        public List<string>? Prerequisites { get; set; }

        /// <summary>
        /// Financial Aid provided for a Training
        /// </summary>
        //[BsonElement("Loans")]
        public List<Loans>? Loans { get; set; }

        //[BsonElement("TrainingsProvider")]
        public EduProvider TrainingProvider { get; set; }

        //[BsonElement("CourseProvider")]
        public EduProvider CourseProvider { get; set; }

        //[BsonElement("AppointmentUrl")]
       public List<Appointment>? Appointment { get; set; }

        public string? TargetAudience { get; set; }

        /// <summary>
        /// Training Provider Url or Target
        /// </summary>
        //[BsonElement("ProductUrl")]
        public Uri? ProductUrl { get; set; }

        /// <summary>
        /// Defined as City + Contact
        /// </summary>
        // [BsonElement("Contacts")]
        public List<Contact>? Contacts { get; set; }

        /// <summary>
        /// The type of the training.
        /// </summary>
        public TrainingType? TrainingType { get; set; }


        // It should be bool but what do I know about education ofc it is not bool
        //[BsonElement("IndividualStartDate")]
        public string? IndividualStartDate { get; set; }

        //TODO: Review
        /// <summary>
        /// Maybe we should make this a class of its own?
        /// Since comparison is done by more information such as where does it happens, what does it include ...
        /// </summary>
        //[BsonElement("Price")]
        public decimal? Price { get; set; } 

        // [BsonElement("PriceDescription")]
        public string? PriceDescription { get; set; }

        //[BsonElement("Accessibility")]
        public bool AccessibilityAvailable { get; set; }

        #region Metadata

        // [BsonElement("SeoTags")]
        public List<string> Tags { get; set; }

        //[BsonElement("Categories")]
        public List<string> Categories { get; set; }

        //TODO: Review Not to be set by the API, API may Use Id room of Training Provider and ExternalTrainingId
        /// <summary>
        /// These are Apollo Ids not External Ids !!!
        /// </summary>
        public List<Training>? SimilarTrainings { get; set; }

        /// <summary>
        /// Apollo Internal Id only to be set by the Backend!
        /// </summary>
        public List<Training>? RecommendedTrainings { get; set; }
        
        #endregion

        #warning candidate for interface!!
        #region IContentPublising // todo.

        //[BsonElement("PublishingDate")]
        public DateTime? PublishingDate { get; set; }
        //[BsonElement("UnpublishingDate")]
        public DateTime? UnpublishingDate { get; set; }
        //[BsonElement("Successor")]
        public string? Successor { get; set; }
        //[BsonElement("Predecessor")]
        public string? Predecessor { get; set; }

        #endregion


    }
}