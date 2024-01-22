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
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        protected AbstractFileRepository(string basePath, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNullOrEmpty(basePath);
            Logger = logger;
            BasePath = basePath;
        }

        protected ILogger Logger { get; }

        protected string BasePath { get; }

        public async Task<TU?> LoadAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                await _semaphore.WaitAsync(token).ConfigureAwait(false);
                if (!File.Exists(BasePath))
                {
                    return default;
                }

                using (var stream = File.OpenRead(BasePath))
                {
                    var options = new JsonSerializerOptions()
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        Converters = { new CultureInfoJsonConverter() },
                    };

                    var profile = await JsonSerializer.DeserializeAsync<TU>(stream, options, token).ConfigureAwait(false);
                    return profile;
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
            finally
            {
                _semaphore.Release(1);
            }

            return default;
        }

        public async Task<bool> SaveAsync(TU data, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                await _semaphore.WaitAsync(token).ConfigureAwait(false);
                var path = Path.GetTempFileName();
                using (var stream = File.OpenWrite(path))
                {
                    var options = new JsonSerializerOptions()
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        Converters = { new CultureInfoJsonConverter() },
                    };
                    await JsonSerializer.SerializeAsync<TU>(stream, data, options, token).ConfigureAwait(false);
                    File.Move(path, BasePath, true);
                    return true;
                }
            }
            catch (OperationCanceledException)
            {
                Logger.LogDebug($"Canceled {nameof(SaveAsync)} in {GetType().Name}.");
            }
            catch (ObjectDisposedException)
            {
                Logger.LogDebug($"Canceled {nameof(SaveAsync)} in {GetType().Name}.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error in {nameof(SaveAsync)} in {GetType().Name}.");
            }
            finally
            {
                _semaphore.Release(1);
            }

            return false;
        }
    }
}
