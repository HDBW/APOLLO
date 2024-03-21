// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// Is a Single Appointment that can be booked by a User.
    /// </summary>
    public record Appointment
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        //[BsonElement("AppointmentUrl")]
        public Uri? AppointmentUrl { get; set; }

        //TODO: Review if this is needed
        //[BsonElement("AppointmentType")]
        public string? AppointmentType { get; set; }

        //[BsonElement("AppointmentDescription")]
        public string? AppointmentDescription { get; set; }

        //[BsonElement("AppointmentLocation")]
        public Contact? AppointmentLocation { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? DurationDescription { get; set; }

        //Maybe autocalculated by Occurences?
        public int? Duration { get; set; }

        //Maybe better as a List?
        public List<Occurence>? Occurences { get; set; }

        //[BsonElement("IsGuaranteedAppointment")]
        public bool IsGuaranteed { get; set; }

        //[BsonElement("TrainingMode")]
        public TrainingMode? TrainingMode { get; set; }

        /// <summary>
        /// Explains the Time Model for the Training
        /// 1UE = 45 Minutes
        /// 1ZS = 60 Minutes
        /// </summary>
        //[BsonElement("TimeInvestAttendee")]
        public int? TimeInvestAttendee { get; set; }


        //[BsonElement("TimeModel")]
        public TrainingTimeModel? TimeModel { get; set; }

        public string? OccurenceNoteOnTime { get; set; }

        /// <summary>
        /// NOT UI RELEVANT
        /// </summary>
        public string? ExecutionDuration { get; set; }

        /// <summary>
        /// Explains the Time Model for the Training
        /// 1UE = 45 Minutes
        /// 1ZS = 60 Minutes
        /// Weeks is also Possible (1W)
        /// NOT UI RELEvANT
        /// </summary>
        public string? ExecutionDurationUnit { get; set; }

        /// <summary>
        /// NOT UI RELEVANT
        /// </summary>
        public string? ExecutionDurationDescription { get; set; }

        //TODO: Review if this is needed
        //[BsonElement("LessonType")]
        public string? LessonType { get; set; }


        //[BsonElement("BookingUrl")]
        public Uri? BookingUrl { get; set; }

        public string? DurationUnit { get; set; }
        public string? Comment { get; set; }
    }
}
