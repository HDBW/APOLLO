// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using De.HDBW.Apollo.SharedContracts.Models;

namespace De.HDBW.Apollo.SharedContracts.Questions
{
    public class Choice : AbstractQuestion
    {
        public Choice(RawData data, string itemId, string compancyId, string bookletId, CultureInfo cultureInfo)
            : base(data, itemId, compancyId, bookletId, cultureInfo)
        {
            NumberOfChoices = string.IsNullOrWhiteSpace(Data.noprimary) ? 0 : int.TryParse(Data.noprimary, CultureInfo.InvariantCulture, out int _) ? int.Parse(Data.noprimary, CultureInfo.InvariantCulture) : 0;
            QuestionImages = new List<Image>() { data.bild1, data.bild2, data.bild3, data.bild4 }.Where(x => x != null).ToList();
            AnswerImages = new List<Image>() { data.primimage1, data.primimage2, data.primimage3, data.primimage4 }.Where(x => x != null).ToList();
            AnswerTexts = new List<string>() { data.primdistractor1, data.primdistractor2, data.primdistractor3, data.primdistractor4 }.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            CreateAditionalData(1, data.m_choice_option1_1, data.m_choice_option1_2, data.m_choice_option1_3, data.m_choice_option1_4, data.m_choice_credit1);
            CreateAditionalData(2, data.m_choice_option2_1, data.m_choice_option2_2, data.m_choice_option2_3, data.m_choice_option2_4, data.m_choice_credit2);
            CreateAditionalData(3, data.m_choice_option3_1, data.m_choice_option3_2, data.m_choice_option3_3, data.m_choice_option3_4, data.m_choice_credit3);
            CreateAditionalData(4, data.m_choice_option4_1, data.m_choice_option4_2, data.m_choice_option4_3, data.m_choice_option4_4, data.m_choice_credit4);
        }

        public int NumberOfChoices { get; set; }

        public IEnumerable<Image> QuestionImages { get; } = new List<Image>();

        public Audio? QuestionAudio
        {
            get { return Data.audio; }
        }

        public IEnumerable<Image> AnswerImages { get; } = new List<Image>();

        public IEnumerable<string> AnswerTexts { get; }

        public Dictionary<int, double> Credits { get; } = new Dictionary<int, double>();

        public Dictionary<int, string> Scores { get; } = new Dictionary<int, string>();

        private void CreateAditionalData(int sourceIndex, string option1, string option2, string option3, string option4, string credit)
        {
            var options = new List<string>() { option1, option2, option3, option4 }.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim());
            if (!options.Any())
            {
                return;
            }

            var score = string.Join(";", options);
            Scores.Add(sourceIndex, score);
            Credits.Add(sourceIndex, double.Parse(credit ?? "0", CultureInfo.InvariantCulture));
        }
    }
}
