// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class ClozeEntry : AbstractQuestionEntry
    {
        [ObservableProperty]
        private string? _clozeHtml;

        private ClozeEntry(Cloze data)
            : base(data)
        {
            ClozeHtml = data.ClozeHtml;
        }

        public static ClozeEntry Import(Cloze data)
        {
            return new ClozeEntry(data);
        }
    }
}
