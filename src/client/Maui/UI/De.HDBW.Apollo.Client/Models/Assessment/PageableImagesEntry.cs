// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class PageableImagesEntry : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ImageEntry> _images = new ObservableCollection<ImageEntry>();

        public PageableImagesEntry(ObservableCollection<ImageEntry> images)
        {
            Images = images;
        }

        public static PageableImagesEntry Import(ObservableCollection<ImageEntry> images)
        {
            return new PageableImagesEntry(images);
        }
    }
}
