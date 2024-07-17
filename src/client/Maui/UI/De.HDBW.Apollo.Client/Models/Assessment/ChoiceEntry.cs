// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class ChoiceEntry : AbstractQuestionEntry<Choice>
    {
        private readonly Func<AudioEntry, Action<bool>, Task<bool>> _togglePlaybackCallback;
        private readonly Func<AudioEntry, Action<bool>, Task<bool>> _restartAudioCallback;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(TogglePlayCommand))]
        private AudioEntry? _questionAudio;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RestartPlayCommand))]
        private bool _isPlaying;

        [ObservableProperty]
        private ObservableCollection<SelectableTextEntry> _answerTexts = new ObservableCollection<SelectableTextEntry>();

        [ObservableProperty]
        private ObservableCollection<SelectableImageEntry> _answerImages = new ObservableCollection<SelectableImageEntry>();

        [ObservableProperty]
        private ObservableCollection<ZoomableImageEntry> _questionImages = new ObservableCollection<ZoomableImageEntry>();

        [ObservableProperty]
        private ObservableCollection<object> _questionImageView = new ObservableCollection<object>();

        private ChoiceEntry(Choice data, string basePath, int density, Dictionary<string, int> imageSizeConfig, Action<ZoomableImageEntry> zoomImageCallback, Func<AudioEntry, Action<bool>, Task<bool>> togglePlaybackCallback, Func<AudioEntry, Action<bool>, Task<bool>> restartAudioCallback)
            : base(data)
        {
            ArgumentNullException.ThrowIfNull(basePath);
            ArgumentNullException.ThrowIfNull(zoomImageCallback);
            ArgumentNullException.ThrowIfNull(togglePlaybackCallback);
            ArgumentNullException.ThrowIfNull(restartAudioCallback);

            _togglePlaybackCallback = togglePlaybackCallback;
            _restartAudioCallback = restartAudioCallback;
            QuestionImages = new ObservableCollection<ZoomableImageEntry>(data.QuestionImages.Select(x => ZoomableImageEntry.Import(x, basePath, density, imageSizeConfig[nameof(data.QuestionImages)], zoomImageCallback)));
            AnswerImages = new ObservableCollection<SelectableImageEntry>(data.AnswerImages.Select(x => SelectableImageEntry.Import(x, basePath, density, imageSizeConfig[nameof(data.AnswerImages)], () => { DidInteract = true; })));
            AnswerTexts = new ObservableCollection<SelectableTextEntry>(data.AnswerTexts.Select(x => SelectableTextEntry.Import(x, () => { DidInteract = true; })));
            switch (QuestionImages.Count())
            {
                case 1:
                    QuestionImageView.Add(QuestionImages[0]);
                    break;
                case 2:
                    QuestionImageView.Add(PageableImagesEntry.Import(QuestionImages));
                    break;
            }

            if (data.QuestionAudio != null)
            {
                QuestionAudio = AudioEntry.Import(data.QuestionAudio, basePath);
            }

            OnPropertyChanged(nameof(HasQuestionImageView));
        }

        public bool HasQuestionImageView
        {
            get
            {
                return QuestionImageView.Count() > 0;
            }
        }

        public bool HasAudioView
        {
            get
            {
                return QuestionAudio != null;
            }
        }

        public override bool DidInteract { get; protected set; }

        public static ChoiceEntry Import(Choice data, string basePath, int density, Dictionary<string, int> imageSizeConfig, Action<ZoomableImageEntry> zoomImageCallback, Func<AudioEntry, Action<bool>, Task<bool>> togglePlaybackCallback, Func<AudioEntry, Action<bool>, Task<bool>> restartAudioCallback)
        {
            return new ChoiceEntry(data, basePath, density, imageSizeConfig, zoomImageCallback, togglePlaybackCallback, restartAudioCallback);
        }

        public override double? GetScore()
        {
            string selection = string.Empty;
            if (AnswerImages.Any())
            {
                selection = string.Join(";", AnswerImages.Select(x => x.IsSelected ? "1" : "0"));
            }
            else if (AnswerTexts.Any())
            {
                selection = string.Join(";", AnswerTexts.Select(x => x.IsSelected ? "1" : "0"));
            }

            return Data.CalculateScore(selection);
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
            return true;
        }

        private void UpdateIsPlaying(bool isPlaying)
        {
            IsPlaying = isPlaying;
        }
    }
}
