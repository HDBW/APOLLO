// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using De.HDBW.Apollo.SharedContracts.Models;

namespace De.HDBW.Apollo.SharedContracts.Questions
{
    public class Eacondition : AbstractQuestion, ICalculateScore<int>
    {
        public Eacondition(RawData data, string rawDataId, string modulId, string assessmentId, CultureInfo cultureInfo)
            : base(data, rawDataId, assessmentId, assessmentId, cultureInfo)
        {
            NumberOfChoices = string.IsNullOrWhiteSpace(Data.noprimary) ? 1 : int.Parse(Data.noprimary, CultureInfo.InvariantCulture);
            if (data.reliant != null)
            {
                Links = new List<Reliant>()
                {
                    data.reliant.reliant_0,
                    data.reliant.reliant_1,
                    data.reliant.reliant_2,
                    data.reliant.reliant_4,
                    data.reliant.reliant_5,
                    data.reliant.reliant_6,
                    data.reliant.reliant_7,
                    data.reliant.reliant_8,
                    data.reliant.reliant_9,
                    data.reliant.reliant_10,
                }.Where(x => x != null).ToList();
            }

            Images = new List<Image>() { data.bild1, data.bild2, data.bild3, data.bild4 }.Where(x => x != null).ToList();
            CreateAditionalData(1, data.eacond_credit1);
            CreateAditionalData(2, data.eacond_credit2);
        }

        public string? Beispielberufe
        {
            get { return Data.beispielberufe; }
        }

        public string? Voraussetzungen
        {
            get { return Data.voraussetzungen; }
        }

        public string? Infotext
        {
            get { return Data.infotext; }
        }

        public string? Bezeichnung
        {
            get { return Data.bezeichnung; }
        }

        public string? Situation
        {
            get { return Data.situation; }
        }

        public List<Reliant> Links { get; } = new List<Reliant>();

        public List<Image> Images { get; } = new List<Image>();

        public int NumberOfChoices { get; set; }

        public Dictionary<int, double> Credits { get; } = new Dictionary<int, double>();

        public double CalculateScore(int selection)
        {
            return Credits.TryGetValue(selection, out double value) ? value : 0d;
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
