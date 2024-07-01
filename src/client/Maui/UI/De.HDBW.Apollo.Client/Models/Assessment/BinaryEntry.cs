// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class BinaryEntry : AbstractQuestionEntry
    {
        [ObservableProperty]
        private AudioEntry? _questionAudio;

        [ObservableProperty]
        private bool _isPlaying;

        private BinaryEntry(Binary data, string basePath)
            : base(data)
        {
            ArgumentNullException.ThrowIfNull(basePath);

            if (data.QuestionAudio == null)
            {
                return;
            }

            QuestionAudio = AudioEntry.Import(data.QuestionAudio, basePath);
        }

        public static BinaryEntry Import(Binary data, string basePath)
        {
            return new BinaryEntry(data, basePath);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private Task TogglePlay(CancellationToken cancellationToken)
        {
            IsPlaying = !IsPlaying;
            return Task.CompletedTask;
        }
    }
}
