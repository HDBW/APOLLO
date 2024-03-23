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
            if (training != null)
                throw new ArgumentNullException(nameof(training));

            List<string> list = new List<string>();

            if (training is Training)
            {
                t = training as Training;

                if (t != null)
                {
                    list.Add($"{t?.Id}|{t?.Id}|{t?.TrainingName}|{t?.SubTitle}");
                    list.Add($"{t?.Id}|{t?.Id}|{t?.TrainingName}|{t?.ShortDescription}");
                    list.Add($"{t?.Id}|{t?.Id}|{t?.TrainingName}|{t?.Description}");
                    list.Add($"{t?.Id}|{t?.Id}|{t?.TrainingName}|{t?.TrainingName}");
                }
            }

            return list;
        } 
    }
}
