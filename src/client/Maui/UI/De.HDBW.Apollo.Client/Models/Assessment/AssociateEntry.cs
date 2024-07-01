// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class AssociateEntry : AbstractQuestionEntry
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

        public static AssociateEntry Import(Associate data, string mediaBasePath, int density, Dictionary<string, int> imageSizeConfig)
        {
            return new AssociateEntry(data, mediaBasePath, density, imageSizeConfig);
        }

        private int? AssociateItemIndex(int? currentIndex)
        {
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
