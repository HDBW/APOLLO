// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class HeaderItem : ObservableObject
    {
        [ObservableProperty]
        private string? _trainingName;

        [ObservableProperty]
        private string? _subTitle;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasImage))]
        private string? _imagePath;

        [ObservableProperty]
        private string? _trainingType;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasProviderImage))]
        private string? _providerImage;

        [ObservableProperty]
        private string? _providerName;

        private HeaderItem(
            string? trainingName,
            string? subTitle,
            string imagePath,
            string? trainingType,
            string? providerName,
            string? providerImage)
        {
            TrainingName = trainingName;
            SubTitle = subTitle;
            ImagePath = imagePath;
            TrainingType = trainingType;
            ProviderName = providerName;
            ProviderImage = providerImage;
        }

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath);
            }
        }

        public bool HasProviderImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ProviderImage);
            }
        }

        public static HeaderItem Import(
            string? trainingName,
            string? subTitle,
            string imagePath,
            string? trainingType,
            string? providerName,
            string? providerImage)
        {
            return new HeaderItem(trainingName, subTitle, imagePath, trainingType, providerName, providerImage);
        }
    }
}
