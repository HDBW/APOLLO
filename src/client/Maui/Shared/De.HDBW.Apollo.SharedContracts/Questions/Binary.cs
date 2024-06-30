// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using De.HDBW.Apollo.SharedContracts.Models;

namespace De.HDBW.Apollo.SharedContracts.Questions
{
    public class Binary : AbstractQuestion
    {
        public Binary(RawData data, string itemId, string compancyId, string bookletId, CultureInfo cultureInfo)
            : base(data, itemId, compancyId, bookletId, cultureInfo)
        {
            NumberOfChoices = string.IsNullOrWhiteSpace(Data.noprimary) ? 0 : int.TryParse(Data.noprimary, CultureInfo.InvariantCulture, out int _) ? int.Parse(Data.noprimary, CultureInfo.InvariantCulture) : 0;
            CreateAditionalData(data.binary_credit1, data.binary_credit2);
        }

        public Audio? QuestionAudio
        {
            get { return Data.audio; }
        }

        public int NumberOfChoices { get; set; }

        public Dictionary<int, double> Credits { get; } = new Dictionary<int, double>();

        public Dictionary<int, bool> Scores { get; } = new Dictionary<int, bool>();

        private void CreateAditionalData(string credit1, string credit2)
        {
            Scores.Add(0, true);
            Scores.Add(1, false);
            Credits.Add(0, double.Parse(credit1 ?? "0", CultureInfo.InvariantCulture));
            Credits.Add(1, double.Parse(credit2 ?? "0", CultureInfo.InvariantCulture));
        }
    }
}
