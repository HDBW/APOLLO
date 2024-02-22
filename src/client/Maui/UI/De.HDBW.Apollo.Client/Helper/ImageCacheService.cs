// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using AndroidX.Browser.Trusted;
using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Helper
{
    public class ImageCacheService : IImageCacheService
    {
        private static readonly string CacheDirectory = Path.Combine(FileSystem.CacheDirectory, "de.hdbw.apollo", "SVGUriImages");

        public ImageCacheService(ILogger<ImageCacheService> logger)
        {
            Logger = logger;
        }

        private bool CachingEnabled { get; set; } = true;

        private TimeSpan CacheValidity { get; set; } = TimeSpan.FromHours(5);

        private ILogger Logger { get; }

        public async Task<(Uri Uri, string? Data)> DownloadAsync(Uri uri, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            string? imageData = null;
            try
            {
                imageData = await DownloadAndCacheImageAsync(uri, token).ConfigureAwait(false);
            }
            catch
            {
            }

            return (uri, imageData);
        }

        private void CacheImage(Stream imageData, string path)
        {
            var directory = Path.GetDirectoryName(path);

            if (string.IsNullOrEmpty(directory))
            {
                throw new InvalidOperationException($"Unable to get directory path name '{path}'.");
            }

            Directory.CreateDirectory(directory);

            var result = true;
            try
            {
                using (var stream = File.Open(path, FileMode.Create))
                {
                    imageData.Seek(0, SeekOrigin.Begin);
                    imageData.CopyTo(stream);
                }
            }
            catch
            {
                result = false;
            }

            if (result == false)
            {
                throw new InvalidOperationException($"Unable to cache image at '{path}'.");
            }
        }

        private bool IsImageCached(string path)
        {
            if (File.Exists(path) && new FileInfo(path).Length > 0)
            {
                var created = File.GetCreationTimeUtc(path);
                if (created.Add(CacheValidity) < DateTime.UtcNow)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        private Stream GetCachedImage(string path)
        {
            var imageData = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            if ((imageData?.Length ?? 0) == 0)
            {
                throw new InvalidOperationException($"Unable to load image stream data from '{path}'.");
            }

            return imageData!;
        }

        private async Task<string> DownloadAndCacheImageAsync(Uri uri, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Stream? imageData = null;
            try
            {
                var hash = Crc64.ComputeHashString(uri.OriginalString);
                var ext = Path.GetExtension(uri.OriginalString);
                var filename = $"{hash}{ext}";
                var pathToImageCache = Path.Combine(CacheDirectory, filename);

                Logger.LogDebug($"DownloadAndCacheImageAsync: {uri}");
                if (CachingEnabled && IsImageCached(pathToImageCache))
                {
                    Logger.LogDebug($"Return cached item");
                    imageData = GetCachedImage(pathToImageCache);
                }
                else
                {
                    Logger.LogDebug($"Download uncached image.");
                    imageData = await DownloadImageAsync(uri, token).ConfigureAwait(false);
                    if (imageData != null && CachingEnabled)
                    {
                        Logger.LogDebug($"Add to cache");
                        CacheImage(imageData, pathToImageCache);
                    }
                }

                if ((imageData?.Length ?? 0) < 1)
                {
                    Logger.LogDebug($"ImageData was empty");
                    return string.Empty;
                }

                imageData!.Seek(0, SeekOrigin.Begin);
                using (StreamReader streamReader = new StreamReader(imageData))
                {
                    return await streamReader.ReadToEndAsync(token).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error while {nameof(DownloadAndCacheImageAsync)}.");
                return string.Empty;
            }
            finally
            {
                imageData?.Dispose();
            }
        }

        private async Task<Stream?> DownloadImageAsync(Uri uri, CancellationToken cancellationToken)
        {
            Stream? imageData = null;
            try
            {
                using (var client = new HttpClient())
                {
                    using (var stream = await client.GetStreamAsync(uri, cancellationToken).ConfigureAwait(false))
                    {
                        imageData = new MemoryStream();
                        await stream.CopyToAsync(imageData);
                        imageData.Seek(0, SeekOrigin.Begin);
                        //Logger.LogDebug($"stream length {stream.Length}");
                        //if (stream.Length > 0)
                        //{

                        //
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Exception during DownloadImageAsync");
            }

            return imageData;
        }
    }
}
