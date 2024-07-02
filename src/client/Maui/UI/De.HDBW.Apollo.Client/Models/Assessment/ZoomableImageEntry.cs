// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using Image = De.HDBW.Apollo.SharedContracts.Models.Image;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class ZoomableImageEntry : ImageEntry
    {
        private Action<ZoomableImageEntry> _zoomImageCallback;

        private ZoomableImageEntry(Image data, string basePath, int density, int size, Action<ZoomableImageEntry> zoomImageCallback)
            : base(data, basePath, density, size)
        {
            ArgumentNullException.ThrowIfNull(zoomImageCallback);
            _zoomImageCallback = zoomImageCallback;
        }

        public static ZoomableImageEntry Import(Image data, string basePath, int density, int size, Action<ZoomableImageEntry> zoomImageCallback)
        {
            return new ZoomableImageEntry(data, basePath, density, size, zoomImageCallback);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private Task Zoom(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            _zoomImageCallback.Invoke(this);
            return Task.CompletedTask;
        }
    }
}
