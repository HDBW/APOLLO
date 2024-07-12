// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.T4;

namespace De.HDBW.Apollo.SharedContracts.Questions
{
    public class Cloze : AbstractQuestion, ICalculateScore<Dictionary<int, string?>>
    {
        public Cloze(RawData data, string rawDataId, string modulId, string assessmentId, CultureInfo cultureInfo)
            : base(data, rawDataId, modulId, assessmentId, cultureInfo)
        {
            NumberOfChoices = string.IsNullOrWhiteSpace(Data.noprimary) ? 0 : int.TryParse(Data.noprimary, CultureInfo.InvariantCulture, out int _) ? int.Parse(Data.noprimary, CultureInfo.InvariantCulture) : 0;
            var id1Inputs = new List<string>() { data.cloze_id1_1, data.cloze_id1_2, data.cloze_id1_3, data.cloze_id1_4 };
            var id1Scores = new List<string>() { data.cloze_scoringid1_1, data.cloze_scoringid1_2, data.cloze_scoringid1_3, data.cloze_scoringid1_4 };
            var id2Inputs = new List<string>() { data.cloze_id2_1, data.cloze_id2_2, data.cloze_id2_3, data.cloze_id2_4 };
            var id2Scores = new List<string>() { data.cloze_scoringid2_1, data.cloze_scoringid2_2, data.cloze_scoringid2_3, data.cloze_scoringid2_4 };
            var id3Inputs = new List<string>() { data.cloze_id3_1, data.cloze_id3_2, data.cloze_id3_3, data.cloze_id3_4 };
            var id3Scores = new List<string>() { data.cloze_scoringid3_1, data.cloze_scoringid3_2, data.cloze_scoringid3_3, data.cloze_scoringid3_4 };
            var id4Inputs = new List<string>() { data.cloze_id4_1, data.cloze_id4_2, data.cloze_id4_3, data.cloze_id4_4 };
            var id4Scores = new List<string>() { data.cloze_scoringid4_1, data.cloze_scoringid4_2, data.cloze_scoringid4_3, data.cloze_scoringid4_4 };
            CreateAditionalData(1, id1Inputs, id1Scores, data.cloze_credit1);
            CreateAditionalData(2, id2Inputs, id2Scores, data.cloze_credit2);
            CreateAditionalData(3, id3Inputs, id3Scores, data.cloze_credit3);
            CreateAditionalData(4, id4Inputs, id4Scores, data.cloze_credit4);
            var cloze = new ClozeHtml(RawCloze ?? string.Empty, InputModes, new Dictionary<int, string>());
            ClozeHtml = cloze.TransformText();
            Ids = cloze.Ids;
            Tokens = cloze.Tokens;
        }

        public string ClozeHtml { get; }

        public IList<string> Ids { get; }

        public IList<string> Tokens { get; }

        public string? RawCloze
        {
            get { return Data.cloze; }
        }

        public int NumberOfChoices { get; set; }

        public Dictionary<int, IEnumerable<string>> Inputs { get; } = new Dictionary<int, IEnumerable<string>>();

        public Dictionary<int, InputMode> InputModes { get; } = new Dictionary<int, InputMode>();

        public Dictionary<int, string> Scores { get; } = new Dictionary<int, string>();

        public Dictionary<int, double> Credits { get; } = new Dictionary<int, double>();

        public double? CalculateScore(Dictionary<int, string?> selection)
        {
            double? result = null;
            foreach (var currentValue in selection)
            {
                if (string.IsNullOrWhiteSpace(currentValue.Value))
                {
                    continue;
                }

                var patter = string.Join(";", Inputs[currentValue.Key].Select(x => x.Split(";").Contains(currentValue.Value) ? "1" : "0"));
                if (patter != Scores[currentValue.Key])
                {
                    continue;
                }

                var credit = Credits[currentValue.Key];
                if (result == null)
                {
                    result = 0;
                }

                result += credit;
            }

            return result;
        }

        private void CreateAditionalData(int id, List<string> inputs, List<string> scores, string credit)
        {
            inputs = inputs.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            scores = scores.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (inputs.Any())
            {
                Inputs.Add(id, inputs);
                var mode = inputs.Count > 1 ? InputMode.Choice : InputMode.TextInput;
                InputModes.Add(id, mode);
                Scores.Add(id, mode == InputMode.Choice ? string.Join(";", scores) : scores[0]);
                Credits.Add(id, double.Parse(credit ?? "0"));
            }
        }
    }
}
