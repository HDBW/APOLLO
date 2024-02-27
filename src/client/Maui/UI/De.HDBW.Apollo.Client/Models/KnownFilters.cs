// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Trainings;
using TrainingModel = Invite.Apollo.App.Graph.Common.Models.Trainings.Training;

namespace De.HDBW.Apollo.Client.Models
{
    public static class KnownFilters
    {
        public const string LoansFieldName = nameof(TrainingModel.Loans);

        public const string AccessibilityAvailableFieldName = nameof(TrainingModel.AccessibilityAvailable);

        public const string IndividualStartDateFieldName = nameof(TrainingModel.IndividualStartDate);

        public const string PriceFieldName = nameof(TrainingModel.Price);

        public const string TrainingsModeFieldName = nameof(TrainingModel.TrainingMode);

        public const string AppointmenTrainingsModeFieldName = $"{nameof(TrainingModel.Appointment)}.{nameof(Appointment.TrainingMode)}";

        public const string AppointmenTrainingsTimeModelFieldName = $"{nameof(TrainingModel.Appointment)}.{nameof(Appointment.TimeModel)}";

        public const string AppointmenStartDateFieldName = $"{nameof(TrainingModel.Appointment)}.{nameof(Appointment.StartDate)}";

        public const string AppointmenEndDateFieldName = $"{nameof(TrainingModel.Appointment)}.{nameof(Appointment.EndDate)}";

        public const string OccurenceStartDateFieldName = $"{nameof(TrainingModel.Appointment)}.{nameof(Appointment.Occurences)}.{nameof(Occurence.StartDate)}";

        public const string OccurenceEndDateFieldName = $"{nameof(TrainingModel.Appointment)}.{nameof(Appointment.Occurences)}.{nameof(Occurence.EndDate)}";
    }
}
