// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

using System;
using System.Collections.Generic;

namespace Invite.Apollo.App.Graph.Common.Models.Trainings
{
    public record Appointment
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        //[BsonElement("AppointmentUrl")]
        public Uri AppointmentUrl { get; set; }

        //[BsonElement("AppointmentType")]
        public string AppointmentType { get; set; }

        //[BsonElement("AppointmentDescription")]
        public string AppointmentDescription { get; set; }

        //[BsonElement("AppointmentLocation")]
        public Contact AppointmentLocation { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string DurationDescription { get; set; }

        //Maybe autocalculated by Occurences?
        public TimeSpan Duration { get; set; }

        //Maybe better as a List?
        public List<Occurence> Occurences { get; set; }

        //[BsonElement("IsGuaranteedAppointment")]
        public bool IsGuaranteed { get; set; }

        //[BsonElement("TrainingType")]
        public TrainingMode TrainingMode { get; set; }

        /// <summary>
        /// Explains the Time Model for the Training
        /// 1UE = 45 Minutes
        /// 1ZS = 60 Minutes
        /// </summary>
        //[BsonElement("TimeInvestAttendee")]
        public TimeSpan TimeInvestAttendee { get; set; }


        //[BsonElement("TimeModel")]
        public TrainingTimeModel TimeModel { get; set; }

        public string OccurenceNoteOnTime { get; set; }

        /// <summary>
        /// NOT UI RELEVANT
        /// </summary>
        public string ExecutionDuration { get; set; }

        /// <summary>
        /// Explains the Time Model for the Training
        /// 1UE = 45 Minutes
        /// 1ZS = 60 Minutes
        /// Weeks is also Possible (1W)
        /// NOT UI RELEvANT
        /// </summary>
        public string ExecutionDurationUnit { get; set; }

        /// <summary>
        /// NOT UI RELEVANT
        /// </summary>
        public string ExecutionDurationDescription { get; set; }

        //[BsonElement("LessonType")]
        public string LessonType { get; set; }


        //[BsonElement("BookingUrl")]
        public Uri BookingUrl { get; set; }
    }
}
