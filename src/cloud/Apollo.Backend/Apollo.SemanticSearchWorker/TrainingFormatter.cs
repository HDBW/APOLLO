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
        private string _cDelimiter = ";";
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

            list.Add($"{training?.Id}{_cDelimiter}{training?.Id}{_cDelimiter}{training?.TrainingName}{_cDelimiter}{training?.SubTitle?.Replace(_cDelimiter, " ")}");
            list.Add($"{training?.Id}{_cDelimiter}{training?.Id}{_cDelimiter}{training?.TrainingName}{_cDelimiter}{training?.ShortDescription?.Replace(_cDelimiter, " ")}");
            list.Add($"{training?.Id}{_cDelimiter}{training?.Id}{_cDelimiter}{training?.TrainingName}{_cDelimiter}{training?.Description?.Replace(_cDelimiter, " ")}");
            list.Add($"{training?.Id}{_cDelimiter}{training?.Id}{_cDelimiter}{training?.TrainingName}{_cDelimiter}{training?.TrainingName?.Replace(_cDelimiter, " ")}");

            return list;
        }
    }
}
