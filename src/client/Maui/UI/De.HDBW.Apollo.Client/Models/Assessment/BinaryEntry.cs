// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class BinaryEntry : AbstractQuestionEntry
    {
        private readonly Func<AudioEntry, Action<bool>, Task<bool>> _togglePlaybackCallback;
        private readonly Func<AudioEntry, Action<bool>, Task<bool>> _restartAudioCallback;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(TogglePlayCommand))]
        private AudioEntry? _questionAudio;

        [ObservableProperty]
        private bool _isPlaying;

        private BinaryEntry(Binary data, string basePath, Func<AudioEntry, Action<bool>, Task<bool>> togglePlaybackCallback, Func<AudioEntry, Action<bool>, Task<bool>> restartAudioCallback)
            : base(data)
        {
            ArgumentNullException.ThrowIfNull(basePath);
            ArgumentNullException.ThrowIfNull(togglePlaybackCallback);
            ArgumentNullException.ThrowIfNull(restartAudioCallback);

            _togglePlaybackCallback = togglePlaybackCallback;
            _restartAudioCallback = restartAudioCallback;
            if (data.QuestionAudio == null)
            {
                return;
            }

            QuestionAudio = AudioEntry.Import(data.QuestionAudio, basePath);
        }

        public static BinaryEntry Import(Binary data, string basePath, Func<AudioEntry, Action<bool>, Task<bool>> togglePlaybackCallback, Func<AudioEntry, Action<bool>, Task<bool>> restartAudioCallback)
        {
            return new BinaryEntry(data, basePath, togglePlaybackCallback, restartAudioCallback);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanTogglePlay))]
        private async Task TogglePlay(CancellationToken cancellationToken)
        {
            IsPlaying = await _togglePlaybackCallback.Invoke(QuestionAudio!, UpdateIsPlaying);
        }

        private bool CanTogglePlay()
        {
            return QuestionAudio != null;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRestartPlay))]
        private async Task RestartPlay(CancellationToken cancellationToken)
        {
            IsPlaying = await _restartAudioCallback.Invoke(QuestionAudio!, UpdateIsPlaying);
        }

        private bool CanRestartPlay()
        {
            return IsPlaying;
        }

        private void UpdateIsPlaying(bool isPlaying)
        {
            IsPlaying = isPlaying;
        }
    }
}
