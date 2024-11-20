// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class BinaryEntry : AbstractQuestionEntry<Binary>
    {
        private readonly Func<AudioEntry, Action<bool>, Task<bool>> _togglePlaybackCallback;
        private readonly Func<AudioEntry, Action<bool>, Task<bool>> _restartAudioCallback;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(TogglePlayCommand))]
        private AudioEntry? _questionAudio;

        [ObservableProperty]
        private bool _isPlaying;

        [ObservableProperty]
        private bool _isTrueSelected;

        [ObservableProperty]
        private bool _isFalseSelected;

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

        public override bool DidInteract { get; protected set; }

        public static BinaryEntry Import(Binary data, string basePath, Func<AudioEntry, Action<bool>, Task<bool>> togglePlaybackCallback, Func<AudioEntry, Action<bool>, Task<bool>> restartAudioCallback)
        {
            return new BinaryEntry(data, basePath, togglePlaybackCallback, restartAudioCallback);
        }

        public override double? GetScore()
        {
            bool? choice = null;
            if (IsTrueSelected)
            {
                choice = true;
            }
            else if (IsFalseSelected)
            {
                choice = false;
            }

            if (choice == null)
            {
                return null;
            }

            return Data.CalculateScore(choice.Value);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(IsTrueSelected) ||
                e.PropertyName == nameof(IsFalseSelected))
            {
                DidInteract = true;
            }
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
