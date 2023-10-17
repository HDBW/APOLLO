// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

namespace Apollo.Service.Entities
{
    public record Appointments
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        //[BsonElement("Appointment")]
        public Uri Appointment { get; set; }

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

        //TODO: Maybe this is flagged as well not sure?
        //[BsonElement("TrainingType")]
        public Enum TrainingType { get; set; }

        //[BsonElement("TimeInvestAttendee")]
        public TimeSpan TimeInvestAttendee { get; set; }

        /// <summary>
        /// Explains the Time Model for the Training
        /// 1UE = 45 Minutes
        /// 1ZS = 60 Minutes
        /// </summary>
        //[BsonElement("TimeModel")]
        public string TimeModel { get; set; }

        //[BsonElement("LessonType")]
        // public LessonType LessonType { get; set; }
    }
}
