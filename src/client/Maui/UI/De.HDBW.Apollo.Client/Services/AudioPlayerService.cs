// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Enums;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.Models.Assessment;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;

namespace De.HDBW.Apollo.Client.Services
{
    public class AudioPlayerService : IAudioPlayerService
    {
        private IAudioPlayer? _audioPlayer;

        public AudioPlayerService(IAudioManager audioManager, ILogger<AudioPlayerService> logger)
        {
            ArgumentNullException.ThrowIfNull(audioManager);
            ArgumentNullException.ThrowIfNull(logger);
            AudioManager = audioManager;
            Logger = logger;
        }

        private IAudioManager AudioManager { get; }

        private ILogger Logger { get; }

        public async Task<bool> StartAsync(AudioEntry audio, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
#if IOS
                AVFoundation.AVAudioSession.SharedInstance().SetActive(true);
                AVFoundation.AVAudioSession.SharedInstance().SetCategory(AVFoundation.AVAudioSessionCategory.Playback);
#endif
                string cacheDir = FileSystem.Current.CacheDirectory;
                var path = Path.Combine(cacheDir, Path.GetFileName(audio.AbsolutePath));
                if (!File.Exists(path))
                {
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            using (var tempFile = new TempFile())
                            {
                                using (var stream = await client.GetStreamAsync(audio.AbsolutePath, token).ConfigureAwait(false))
                                {
                                    await tempFile.SaveAsync(stream);
                                    tempFile.FileInfo.MoveTo(path, true);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger?.LogError(ex, $"Unknown error in {nameof(StartAsync)} from {GetType().Name}.");
                        throw;
                    }
                }

                _audioPlayer = AudioManager.CreatePlayer(File.Open(path, FileMode.Open, FileAccess.Read));
                _audioPlayer.Play();
                return true;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error in {nameof(StartAsync)} from {GetType().Name}.");

#if IOS
                AVFoundation.AVAudioSession.SharedInstance().SetCategory(AVFoundation.AVAudioSessionCategory.Playback);
                AVFoundation.AVAudioSession.SharedInstance().SetActive(false);
#endif
                throw;
            }
        }

        public void Stop()
        {
#if IOS
            AVFoundation.AVAudioSession.SharedInstance().SetCategory(AVFoundation.AVAudioSessionCategory.Playback);
            AVFoundation.AVAudioSession.SharedInstance().SetActive(false);
#endif
            _audioPlayer?.Stop();
            _audioPlayer?.Dispose();
            _audioPlayer = null;
        }

        private void OnLifeCycleChanged(object recipient, LiveCycleChangedMessage message)
        {
            switch (message.State)
            {
                case LifeCycleState.Paused:
                    _audioPlayer?.Pause();
                    break;
                case LifeCycleState.Running:
                    _audioPlayer?.Play();
                    break;
            }
        }
    }
}
