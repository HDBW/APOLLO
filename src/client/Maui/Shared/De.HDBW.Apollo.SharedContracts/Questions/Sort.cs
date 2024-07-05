// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace De.HDBW.Apollo.SharedContracts.Questions
{
    public class Sort : AbstractQuestion, ICalculateScore<string>
    {
        public Sort(RawData data, string rawDataId, string modulId, string assessmentId, CultureInfo cultureInfo)
            : base(data, rawDataId, assessmentId, assessmentId, cultureInfo)
        {
            NumberOfChoices = string.IsNullOrWhiteSpace(Data.noprimary) ? 0 : int.TryParse(Data.noprimary, CultureInfo.InvariantCulture, out int _) ? int.Parse(Data.noprimary, CultureInfo.InvariantCulture) : 0;
            SortTexts = new List<string>() { data.primdistractor1, data.primdistractor2, data.primdistractor3, data.primdistractor4 }.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var pattern1 = new List<string> { data.sort_option1_1, data.sort_option1_2, data.sort_option1_3, data.sort_option1_4 };
            var pattern2 = new List<string> { data.sort_option2_1, data.sort_option2_2, data.sort_option2_3, data.sort_option2_4 };
            var pattern3 = new List<string> { data.sort_option3_1, data.sort_option3_2, data.sort_option3_3, data.sort_option3_4 };
            CreateAditionalData(1, pattern1, data.sort_credit1);
            CreateAditionalData(2, pattern2, data.sort_credit2);
            CreateAditionalData(3, pattern3, data.sort_credit3);
        }

        public List<string> SortTexts { get; } = new List<string>();

        public Dictionary<int, double> Credits { get; } = new Dictionary<int, double>();

        public Dictionary<int, string> Scores { get; } = new Dictionary<int, string>();

        public int NumberOfChoices { get; set; }

        public double CalculateScore(string selection)
        {
            var score = Scores.FirstOrDefault(x => x.Value == selection);
            return Credits.TryGetValue(score.Key, out double value) ? value : 0;
        }

        private void CreateAditionalData(int index, List<string> pattern, string credit)
        {
            if (string.IsNullOrWhiteSpace(credit))
            {
                return;
            }

            Credits.Add(index, double.Parse(credit ?? "0", CultureInfo.InvariantCulture));
            pattern = pattern.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (pattern.Any())
            {
                Scores.Add(index, string.Join(";", pattern));
            }
        }
    }
}
