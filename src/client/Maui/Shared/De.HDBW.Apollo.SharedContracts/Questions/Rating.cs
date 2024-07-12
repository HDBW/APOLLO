// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace De.HDBW.Apollo.SharedContracts.Questions
{
    public class Rating : AbstractQuestion, ICalculateScore<int>
    {
        public Rating(RawData data, string rawDataId, string modulId, string assessmentId, CultureInfo cultureInfo)
            : base(data, rawDataId, modulId, assessmentId, cultureInfo)
        {
            NumberOfChoices = int.Parse(Data.noprimary, CultureInfo.InvariantCulture);
            CreateAditionalData(1, Data.rating_credit1);
            CreateAditionalData(2, Data.rating_credit2);
            CreateAditionalData(3, Data.rating_credit3);
            CreateAditionalData(4, Data.rating_credit4);
            CreateAditionalData(5, Data.rating_credit5);
            CreateAditionalData(6, Data.rating_credit6);
        }

        public int NumberOfChoices { get; }

        public Dictionary<int, double> Credits { get; } = new Dictionary<int, double>();

        public Dictionary<int, bool> Scores { get; } = new Dictionary<int, bool>();

        public double? CalculateScore(int selection)
        {
            return Credits.TryGetValue(selection, out double value) ? value : null;
        }

        private void CreateAditionalData(int index, string credit)
        {
            if (string.IsNullOrWhiteSpace(credit))
            {
                return;
            }

            Credits.Add(index, double.Parse(credit ?? "0", CultureInfo.InvariantCulture));
            Scores.Add(index, true);
        }
    }
}
