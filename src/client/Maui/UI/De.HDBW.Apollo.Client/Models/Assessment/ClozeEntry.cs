// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class ClozeEntry : AbstractQuestionEntry<Cloze>
    {
        [ObservableProperty]
        private string? _clozeHtml;

        [ObservableProperty]
        private IList<string> _ids;

        private Dictionary<int, string?> CurrentValues { get; } = new Dictionary<int, string?>();

        private ClozeEntry(Cloze data)
            : base(data)
        {
            ClozeHtml = data.ClozeHtml;
            Ids = data.Ids;
            for (var i = 1; i <= Ids.Count; i++)
            {
                CurrentValues[i] = null;
            }
        }

        public static ClozeEntry Import(Cloze data)
        {
            return new ClozeEntry(data);
        }

        public override double GetScore()
        {
            return Data.CalculateScore(CurrentValues);
        }

        public void OnSetValue(string id, string? value)
        {
            if (!Ids.Contains(id))
            {
                return;
            }

            CurrentValues[Ids.IndexOf(id) + 1] = value;
        }
    }
}
