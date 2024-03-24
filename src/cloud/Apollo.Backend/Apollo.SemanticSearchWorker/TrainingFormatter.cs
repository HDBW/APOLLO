// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;

namespace Apollo.SemanticSearchWorker
{
    /// <summary>
    /// Converts the training instance into the set of strings.
    /// </summary>
    internal class TrainingFormatter : IEntityFormatter
    {
        Training? t = new Training();

        /// <summary>
        /// Returns Subtitle, TrainingName, ShortDescription and Description of the training.
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        public IList<string> FormatObject(object instance)
        {
            Training? training = instance as Training;
            if (training == null)
                throw new ArgumentNullException(nameof(training));

            List<string> list = new List<string>();

            list.Add($"{training?.Id}|{training?.Id}|{training?.TrainingName}|{training?.SubTitle}");
            list.Add($"{training?.Id}|{training?.Id}|{training?.TrainingName}|{training?.ShortDescription}");
            list.Add($"{training?.Id}|{training?.Id}|{training?.TrainingName}|{training?.Description}");
            list.Add($"{training?.Id}|{training?.Id}|{training?.TrainingName}|{training?.TrainingName}");

            return list;
        }
    }
}
