// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using De.HDBW.Apollo.SharedContracts.Models;

namespace De.HDBW.Apollo.SharedContracts.Questions
{
    public class Eafrequency : AbstractQuestion
    {
        public Eafrequency(RawData data, CultureInfo cultureInfo)
            : base(data, cultureInfo)
        {
            NumberOfChoices = string.IsNullOrWhiteSpace(Data.noprimary) ? 1 : int.Parse(Data.noprimary, CultureInfo.InvariantCulture);
            Images = new List<Image>() { data.bild1, data.bild2, data.bild3, data.bild4 }.Where(x => x != null).ToList();
            CreateAditionalData(1, data.frequency_credit1);
            CreateAditionalData(2, data.frequency_credit2);
            CreateAditionalData(3, data.frequency_credit3);
            CreateAditionalData(4, data.frequency_credit4);
        }

        public int NumberOfChoices { get; }

        public IEnumerable<Image> Images { get; } = new List<Image>();

        public Dictionary<int, double> Credits { get; } = new Dictionary<int, double>();

        public string Situation
        {
            get { return Data.situation; }
        }

        private void CreateAditionalData(int index, string credit)
        {
            if (index > NumberOfChoices)
            {
                return;
            }

            Credits.Add(index, double.Parse(credit ?? "0", CultureInfo.InvariantCulture));
        }
    }
}
