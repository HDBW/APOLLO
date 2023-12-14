// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

using System;
using System.Collections.Generic;

namespace Apollo.Common.Entities
{
    public record Appointment
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        [Newtonsoft.Json.JsonProperty("id")]
        public string? Id { get; set; }

        //[BsonElement("AppointmentUrl")]
        [Newtonsoft.Json.JsonProperty("appointmentUrl")]
        public Uri AppointmentUrl { get; set; }

        //[BsonElement("AppointmentType")]
        [Newtonsoft.Json.JsonProperty("appointmentType")]
        public string AppointmentType { get; set; }

        //[BsonElement("AppointmentDescription")]
        [Newtonsoft.Json.JsonProperty("appointmentDescription")]
        public string AppointmentDescription { get; set; }

        //[BsonElement("AppointmentLocation")]
        [Newtonsoft.Json.JsonProperty("appointmentLocation")]
        public Contact AppointmentLocation { get; set; }

        [Newtonsoft.Json.JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [Newtonsoft.Json.JsonProperty("endDate")]
        public DateTime EndDate { get; set; }

        [Newtonsoft.Json.JsonProperty("durationDescription")]
        public string DurationDescription { get; set; }

        //Maybe autocalculated by Occurences?
        [Newtonsoft.Json.JsonProperty("duration")]
        public TimeSpan Duration { get; set; }

        //Maybe better as a List?
        [Newtonsoft.Json.JsonProperty("occurences")]
        public List<Occurence> Occurences { get; set; }

        //[BsonElement("IsGuaranteedAppointment")]
        [Newtonsoft.Json.JsonProperty("isGuaranteed")]
        public bool IsGuaranteed { get; set; }

        //[BsonElement("TrainingType")]
        [Newtonsoft.Json.JsonProperty("trainingType")]
        public TrainingType TrainingType { get; set; }

        /// <summary>
        /// Explains the Time Model for the Training
        /// 1UE = 45 Minutes
        /// 1ZS = 60 Minutes
        /// </summary>
        //[BsonElement("TimeInvestAttendee")]
        public TimeSpan TimeInvestAttendee { get; set; }


        //[BsonElement("TimeModel")]
        [Newtonsoft.Json.JsonProperty("timeModel")]
        public TrainingTimeModel TimeModel { get; set; }

        [Newtonsoft.Json.JsonProperty("occurenceNoteOnTime")]
        public string OccurenceNoteOnTime { get; set; }

        /// <summary>
        /// NOT UI RELEVANT
        /// </summary>
        [Newtonsoft.Json.JsonProperty("executionDuration")]
        public string ExecutionDuration { get; set; }

        /// <summary>
        /// Explains the Time Model for the Training
        /// 1UE = 45 Minutes
        /// 1ZS = 60 Minutes
        /// Weeks is also Possible (1W)
        /// NOT UI RELEvANT
        /// </summary>
        [Newtonsoft.Json.JsonProperty("executionDurationUnit")]
        public string ExecutionDurationUnit { get; set; }

        /// <summary>
        /// NOT UI RELEVANT
        /// </summary>
        [Newtonsoft.Json.JsonProperty("executionDurationDescription")]
        public string ExecutionDurationDescription { get; set;}

        //[BsonElement("LessonType")]
        [Newtonsoft.Json.JsonProperty("lessonType")]
        public string LessonType { get; set; }

        //[BsonElement("BookingUrl")]
        [Newtonsoft.Json.JsonProperty("bookingUrl")]
        public Uri BookingUrl { get; set; }
    }
}
