// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class AssociateEntry : AbstractQuestionEntry<Associate>
    {
        [ObservableProperty]
        private ObservableCollection<AssociateTextEntry> _sourceTexts = new ObservableCollection<AssociateTextEntry>();

        [ObservableProperty]
        private ObservableCollection<AssociateImageEntry> _targetImages = new ObservableCollection<AssociateImageEntry>();

        private AssociateEntry(Associate data, string mediaBasePath, int density, Dictionary<string, int> imageSizeConfig)
            : base(data)
        {
            SourceTexts = new ObservableCollection<AssociateTextEntry>(data.SourceTexts.Select(x => AssociateTextEntry.Import(x, (item) => { return SourceTexts.IndexOf(item); })));
            TargetImages = new ObservableCollection<AssociateImageEntry>(data.TargetImages.Select(x => AssociateImageEntry.Import(x, mediaBasePath, density, imageSizeConfig[nameof(AssociateEntry.TargetImages)], (item) => { return TargetImages.IndexOf(item) + 1; }, AssociateItemIndex)));
        }

        public override bool DidInteract { get; protected set; }

        public static AssociateEntry Import(Associate data, string mediaBasePath, int density, Dictionary<string, int> imageSizeConfig)
        {
            return new AssociateEntry(data, mediaBasePath, density, imageSizeConfig);
        }

        public override double? GetScore()
        {
            var values = new List<string>();
            foreach (var text in SourceTexts)
            {
                var sourceIndex = SourceTexts.IndexOf(text);
                var target = TargetImages.FirstOrDefault(x => x.AssociatedIndex == sourceIndex);
                var targetIndex = target != null ? TargetImages.IndexOf(target) : -1;
                values.Add($"{sourceIndex + 1}:{targetIndex + 1}");
            }

            return Data.CalculateScore(string.Join(";", values));
        }

        private int? AssociateItemIndex(int? currentIndex)
        {
            DidInteract = true;
            if (currentIndex == null && !SourceTexts.Any())
            {
                return null;
            }

            if (currentIndex == null)
            {
                return 0;
            }

            var result = currentIndex! + 1;
            if (result == SourceTexts.Count())
            {
                result = 0;
            }

            return result;
        }
    }
}
