// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using De.HDBW.Apollo.SharedContracts.Models;

namespace De.HDBW.Apollo.SharedContracts.Questions
{
    public class Associate : AbstractQuestion, ICalculateScore<string>
    {
        public Associate(RawData data, string rawDataId, string modulId, string assessmentId, CultureInfo cultureInfo)
            : base(data, rawDataId, assessmentId, assessmentId, cultureInfo)
        {
            NumberOfChoices = string.IsNullOrWhiteSpace(Data.noprimary) ? 0 : int.TryParse(Data.noprimary, CultureInfo.InvariantCulture, out int _) ? int.Parse(Data.noprimary, CultureInfo.InvariantCulture) : 0;
            SourceTexts = new List<string>() { data.secdistractor1, data.secdistractor2, data.secdistractor3, data.secdistractor4 }.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            TargetImages = new List<Image>() { data.primimage1, data.primimage2, data.primimage3, data.primimage4 }.Where(x => x != null).ToList();

            CreateAditionalData(1, data.associate_option1_1, data.associate_credit1);
            CreateAditionalData(2, data.associate_option1_2, data.associate_credit1);
            CreateAditionalData(3, data.associate_option1_3, data.associate_credit1);
            CreateAditionalData(4, data.associate_option1_4, data.associate_credit1);
        }

        public int NumberOfChoices { get; set; }

        public List<string> SourceTexts { get; } = new List<string>();

        public List<Image> TargetImages { get; } = new List<Image>();

        public Dictionary<int, double> Credits { get; } = new Dictionary<int, double>();

        public Dictionary<int, string> Scores { get; } = new Dictionary<int, string>();

        public double CalculateScore(string selection)
        {
            var isValid = true;
            var values = selection.Split(';');
            foreach (var value in values)
            {
                var score = Scores.FirstOrDefault(x => x.Value == value);
                if (!Credits.TryGetValue(score.Key, out double credit))
                {
                    isValid = false;
                    continue;
                }
            }

            return isValid ? Credits[1] : 0d;
        }

        private void CreateAditionalData(int sourceIndex, string targetIndex, string credit)
        {
            if (string.IsNullOrWhiteSpace(targetIndex))
            {
                return;
            }

            var score = $"{sourceIndex}:{targetIndex}";
            Scores.Add(sourceIndex, score);
            Credits.Add(sourceIndex, double.Parse(credit ?? "0", CultureInfo.InvariantCulture));
        }
    }
}
