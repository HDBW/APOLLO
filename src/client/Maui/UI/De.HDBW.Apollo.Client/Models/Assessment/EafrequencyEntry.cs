// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class EafrequencyEntry : AbstractQuestionEntry<Eafrequency>
    {
        [ObservableProperty]
        private ObservableCollection<ImageEntry> _images = new ObservableCollection<ImageEntry>();

        [ObservableProperty]
        private string? _situation;

        private EafrequencyEntry(Eafrequency data, string basePath, int density, Dictionary<string, int> imageSizeConfig)
            : base(data)
        {
            ArgumentNullException.ThrowIfNull(basePath);
            Images = new ObservableCollection<ImageEntry>(data.Images.Select(x => ImageEntry.Import(x, basePath, density, imageSizeConfig[nameof(data.Images)])));
            Situation = data.Situation;
        }

        public static EafrequencyEntry Import(Eafrequency data, string basePath, int density, Dictionary<string, int> imageSizeConfig)
        {
            return new EafrequencyEntry(data, basePath, density, imageSizeConfig);
        }

        public override double GetScore()
        {
            //TODO
            return Data.CalculateScore(0);
        }
    }
}
