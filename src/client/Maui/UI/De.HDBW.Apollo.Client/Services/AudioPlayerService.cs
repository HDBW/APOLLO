// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Enums;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Messages;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;

namespace De.HDBW.Apollo.Client.Services
{
    public class AudioPlayerService : IAudioPlayerService
    {
        private IAudioPlayer? _audioPlayer;
        private bool? _wasPlaying;

        public AudioPlayerService(IAudioManager audioManager, ILogger<AudioPlayerService> logger)
        {
            ArgumentNullException.ThrowIfNull(audioManager);
            ArgumentNullException.ThrowIfNull(logger);
            WeakReferenceMessenger.Default.Register<LiveCycleChangedMessage>(this, OnLifeCycleChanged);
            AudioManager = audioManager;
            Logger = logger;
        }

        public bool IsPaused
        {
            get
            {
                if (_audioPlayer == null)
                {
                    return false;
                }

                return !_audioPlayer.IsPlaying;
            }
        }

        public bool IsPlaying
        {
            get
            {
                if (_audioPlayer == null)
                {
                    return false;
                }

                return _audioPlayer.IsPlaying;
            }
        }

        private IAudioManager AudioManager { get; }

        private ILogger Logger { get; }

        public void Pause()
        {
            if (IsPlaying)
            {
                _wasPlaying = true;
                _audioPlayer?.Pause();
            }
            else
            {
                _wasPlaying = null;
            }
        }

        public void Resume()
        {
            if (IsPaused)
            {
                _audioPlayer?.Play();
            }

            _wasPlaying = null;
        }

        public async Task<bool> StartAsync(string absolutePath, EventHandler handlePlaybackEnded, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
#if IOS
                AVFoundation.AVAudioSession.SharedInstance().SetActive(true);
                AVFoundation.AVAudioSession.SharedInstance().SetCategory(AVFoundation.AVAudioSessionCategory.Playback);
#endif
                string cacheDir = FileSystem.Current.CacheDirectory;
                var path = Path.Combine(cacheDir, Path.GetFileName(absolutePath));
                if (!File.Exists(path))
                {
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            using (var tempFile = new TempFile())
                            {
                                using (var stream = await client.GetStreamAsync(absolutePath, token))
                                {
                                    await tempFile.SaveAsync(stream, token);
                                    tempFile.Move(path, true);
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

                _audioPlayer = AudioManager.CreatePlayer(path);
                _audioPlayer.Play();
                _audioPlayer.PlaybackEnded += handlePlaybackEnded;
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

        public void Stop(EventHandler eventHandler)
        {
#if IOS
            AVFoundation.AVAudioSession.SharedInstance().SetCategory(AVFoundation.AVAudioSessionCategory.Playback);
            AVFoundation.AVAudioSession.SharedInstance().SetActive(false);
#endif
            if (_audioPlayer != null)
            {
                _audioPlayer.PlaybackEnded -= eventHandler;
            }

            _audioPlayer?.Stop();
            _audioPlayer?.Dispose();
            _audioPlayer = null;
            _wasPlaying = false;
        }

        private void OnLifeCycleChanged(object recipient, LiveCycleChangedMessage message)
        {
            switch (message.State)
            {
                case LifeCycleState.Paused:
                    Pause();
                    break;
                case LifeCycleState.Running:
                    if (_wasPlaying == true)
                    {
                        Resume();
                    }

                    break;
            }
        }
    }
}
