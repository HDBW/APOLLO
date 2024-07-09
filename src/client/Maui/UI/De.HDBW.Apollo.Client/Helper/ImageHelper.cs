// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text;
using De.HDBW.Apollo.Client.Contracts;

namespace De.HDBW.Apollo.Client.Helper
{
    public class ImageHelper
    {
        private static Dictionary<string, SizeF>? loadedOriginalSizes = null;
        private static string originalSizesFilePath = string.Empty;
        private static string originalSizesMetaFilePath = string.Empty;
        private static SemaphoreSlim originalSizeLoaderSemaphore = new SemaphoreSlim(1);

        static ImageHelper()
        {
            string cacheDir = FileSystem.Current.CacheDirectory;
            originalSizesFilePath = Path.Combine(cacheDir, Path.GetFileName("images.bin"));
            originalSizesMetaFilePath = Path.Combine(cacheDir, Path.GetFileName("images.bin.meta"));
        }

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

        public static (string Id, float Size, float Scale) ParseFileName(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            var parts = name.Split('@', '_');
            if (parts.Length != 3 || !float.TryParse(parts[1], out var size) || !float.TryParse(parts[2], out var scale))
            {
                throw new ArgumentException($"{path} is ends not with a valid image file-name like id_size@scale.jpg.");
            }

            var id = parts[0];
            return (id, size, scale);
        }

        public static SizeF GetOriginalSize(string id)
        {
            try
            {
                return loadedOriginalSizes!.TryGetValue(id, out var size) ? size : default;
            }
            catch
            {
            }

            return default;
        }

        public static async Task EnsureOriginalSizesAreLoaded()
        {
            if (loadedOriginalSizes == null)
            {
                await originalSizeLoaderSemaphore.WaitAsync().ConfigureAwait(false);
                if (loadedOriginalSizes != null)
                {
                    return;
                }

                loadedOriginalSizes = new Dictionary<string, SizeF>();

                await DownloadOrUpdateOriginalSizes();
                LoadOriginalSizesFromFile();

                originalSizeLoaderSemaphore.Release();
            }
        }

        private static async Task DownloadOrUpdateOriginalSizes()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string? lastModified = string.Empty;
                    if (File.Exists(originalSizesMetaFilePath))
                    {
                        lastModified = File.ReadAllText(originalSizesMetaFilePath);
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(lastModified))
                        {
                            client.DefaultRequestHeaders.Add("If-Modified-Since", lastModified);
                        }
                    }
                    catch
                    {
                    }

                    var userSecretsService = Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<IUserSecretsService>();
                    var url = (userSecretsService?["MediaAssetStorageURL"] ?? string.Empty) + "images.bin";

                    using (var response = await client.GetAsync(url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var info = new FileInfo(originalSizesFilePath);

                            if (!info.Exists || info.Length == 0)
                            {
                                using (var tempFile = new TempFile())
                                {
                                    using (var stream = await response.Content.ReadAsStreamAsync())
                                    {
                                        await tempFile.SaveAsync(stream);
                                        tempFile.Move(originalSizesFilePath, true);
                                    }
                                }
                            }

                            string? newLastModified;
                            if (response.Content.Headers.TryGetValues("Last-Modified", out var lastModifiedResponse)
                              && (newLastModified = lastModifiedResponse.FirstOrDefault()) != null
                              && newLastModified != lastModified)
                            {
                                File.WriteAllText(originalSizesMetaFilePath, newLastModified);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private static void LoadOriginalSizesFromFile()
        {
            using (var binaryReader = new BinaryReader(File.OpenRead(originalSizesFilePath), Encoding.Default, false))
            {
                try
                {
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        var idBytes = binaryReader.ReadBytes(32);
                        var width = binaryReader.ReadInt32();
                        var height = binaryReader.ReadInt32();
                        var idAsHexString = ByteArrayToString(idBytes);
                        if (!loadedOriginalSizes.ContainsKey(idAsHexString))
                        {
                            loadedOriginalSizes.Add(idAsHexString, new SizeF(width, height));
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (var b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }
    }
}
