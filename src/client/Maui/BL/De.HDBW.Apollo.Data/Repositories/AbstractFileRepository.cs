// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using De.HDBW.Apollo.Data.JsonConverter;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public abstract class AbstractFileRepository<TU>
    {
        private static readonly object _lockObject = new object();

        private TU? _cache;

        protected AbstractFileRepository(string basePath, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNullOrEmpty(basePath);
            Logger = logger;
            BasePath = basePath;
        }

        protected ILogger Logger { get; }

        private string BasePath { get; }

        protected TU? Cache
        {
            get
            {
                return _cache;
            }

            set
            {
                try
                {
                    Monitor.Enter(_lockObject);
                    _cache = value;
                }
                finally
                {
                    Monitor.Exit(_lockObject);
                }
            }
        }

        public async Task<bool> LoadAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                var profilePath = BasePath;
                if (!File.Exists(profilePath))
                {
                    return false;
                }

                using (var stream = File.OpenRead(profilePath))
                {
                    var options = new JsonSerializerOptions()
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        Converters = { new CultureInfoJsonConverter() },
                    };

                    Cache = await JsonSerializer.DeserializeAsync<TU>(stream, options, token).ConfigureAwait(false);
                    return true;
                }
            }
            catch (OperationCanceledException)
            {
                Logger.LogDebug($"Canceled {nameof(LoadAsync)} in {GetType().Name}.");
            }
            catch (ObjectDisposedException)
            {
                Logger.LogDebug($"Canceled {nameof(LoadAsync)} in {GetType().Name}.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error in {nameof(LoadAsync)} in {GetType().Name}.");
            }

            return false;
        }

        public async Task<bool> SaveAsync(TU data, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                var profilePath = BasePath;
                var path = Path.GetTempFileName();
                using (var stream = File.OpenWrite(path))
                {
                    var options = new JsonSerializerOptions()
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        Converters = { new CultureInfoJsonConverter() },
                    };
                    await JsonSerializer.SerializeAsync<TU>(stream, data, options, token).ConfigureAwait(false);
                    File.Move(path, profilePath);
                    Cache = data;
                    return true;
                }
            }
            catch (OperationCanceledException)
            {
                Logger.LogDebug($"Canceled {nameof(LoadAsync)} in {GetType().Name}.");
            }
            catch (ObjectDisposedException)
            {
                Logger.LogDebug($"Canceled {nameof(LoadAsync)} in {GetType().Name}.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error in {nameof(LoadAsync)} in {GetType().Name}.");
            }

            return false;
        }
    }
}
