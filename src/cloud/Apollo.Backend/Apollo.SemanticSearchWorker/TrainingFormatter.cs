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
        /// Returns:
        /// Trainings:Subtitle, TrainingName, ShortDescription, Description, Content, BenefitList and Prerequisites
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        public IList<string> FormatObject(object instance)
        {
            Training? training = instance as Training;
            if (training == null)
                throw new ArgumentNullException(nameof(training));

            List<string> list = new List<string>();

            list.Add($"{training?.Id}{_cDelimiter}/api/teaining/{training?.Id}{_cDelimiter}{training?.TrainingName}/Subtitle{_cDelimiter}{training?.SubTitle?.Replace(_cDelimiter, " ")}");
            list.Add($"{training?.Id}{_cDelimiter}/api/teaining/{training?.Id}{_cDelimiter}{training?.TrainingName}/ShortDescription{_cDelimiter}{training?.ShortDescription?.Replace(_cDelimiter, " ")}");
            list.Add($"{training?.Id}{_cDelimiter}/api/teaining/{training?.Id}{_cDelimiter}{training?.TrainingName}/Description{_cDelimiter}{training?.Description?.Replace(_cDelimiter, " ")}");
            list.Add($"{training?.Id}{_cDelimiter}/api/teaining/{training?.Id}{_cDelimiter}{training?.TrainingName}/TrainingName{_cDelimiter}{training?.TrainingName?.Replace(_cDelimiter, " ")}");
            list.Add($"{training?.Id}{_cDelimiter}/api/teaining/{training?.Id}{_cDelimiter}{training?.TrainingName}/Content{_cDelimiter}{String.Join(' ', training?.Content?.Select(s=>s.Replace(_cDelimiter!, " "))!)}");
            list.Add($"{training?.Id}{_cDelimiter}/api/teaining/{training?.Id}{_cDelimiter}{training?.TrainingName}/BenefitList{_cDelimiter}{String.Join(' ', training?.BenefitList?.Select(s => s.Replace(_cDelimiter!, " "))!)}");
            list.Add($"{training?.Id}{_cDelimiter}/api/teaining/{training?.Id}{_cDelimiter}{training?.TrainingName}/Prerequisites{_cDelimiter}{String.Join(' ', training?.Prerequisites?.Select(s => s.Replace(_cDelimiter!, " "))!)}");

            return list;
        }
    }
}
