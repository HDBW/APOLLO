// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Helper
{
    public class ImageFormatHelper
    {
        public static ImageFormat RetrieveImageFormatFromFileExtension(string path) => path switch
        {
            var s when s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                    || s.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                    || s.EndsWith(".jfif", StringComparison.OrdinalIgnoreCase) => ImageFormat.Jpeg,
            var s when s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) => ImageFormat.Png,
            var s when s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) => ImageFormat.Bmp,
            var s when s.EndsWith(".tif", StringComparison.OrdinalIgnoreCase)
                    || s.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) => ImageFormat.Tiff,
            var s when s.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) => ImageFormat.Gif,
            _ => throw new ArgumentException($"Unknown file type of file {path}"),
        };
    }
}
