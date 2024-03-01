// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class EduProviderItem : ObservableObject, IProvideImageData
    {
        [ObservableProperty]
        private string? _providerImage;

        [ObservableProperty]
        private string? _providerName;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasProviderImage))]
        private string? _imageData;

        private EduProviderItem(
            string? providerName,
            string? providerImage)
        {
            ProviderName = providerName;
            ProviderImage = providerImage;
        }

        public bool HasProviderImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ProviderImage);
            }
        }

        public static EduProviderItem Import(
            string? providerName,
            string? providerImage)
        {
            return new EduProviderItem(providerName, providerImage);
        }
    }
}
