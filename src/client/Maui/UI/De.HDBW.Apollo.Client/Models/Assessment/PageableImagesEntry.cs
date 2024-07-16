// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class PageableImagesEntry : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ZoomableImageEntry> _images = new ObservableCollection<ZoomableImageEntry>();

        [ObservableProperty]
        private ZoomableImageEntry? _currentImage = null;

        public PageableImagesEntry(ObservableCollection<ZoomableImageEntry> images)
        {
            Images = images;
        }

        public static PageableImagesEntry Import(ObservableCollection<ZoomableImageEntry> images)
        {
            return new PageableImagesEntry(images);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private Task Zoom(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            CurrentImage?.ZoomCommand?.Execute(null);
            return Task.CompletedTask;
        }
    }
}
