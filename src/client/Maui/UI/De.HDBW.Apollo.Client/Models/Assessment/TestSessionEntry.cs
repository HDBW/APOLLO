// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class TestSessionEntry : ObservableObject
    {
        private readonly Func<CancellationToken, Task> _resumeCallback;
        private readonly Func<bool> _canResumeCallback;
        private readonly Func<CancellationToken, Task> _showResultCallback;
        private readonly Func<bool> _canShowResultCallback;

        [ObservableProperty]
        private bool _canContinue;

        private TestSessionEntry(
            int? repeatable,
            int? rawDataCount,
            int? awnserCount,
            Func<CancellationToken, Task> resumeCallback,
            Func<bool> canResumeCallback,
            Func<CancellationToken, Task> showResultCallback,
            Func<bool> canShowResultCallback)
        {
            ArgumentNullException.ThrowIfNull(resumeCallback);
            ArgumentNullException.ThrowIfNull(canResumeCallback);
            ArgumentNullException.ThrowIfNull(showResultCallback);
            ArgumentNullException.ThrowIfNull(canShowResultCallback);

            CanContinue = (repeatable ?? 0) == 0 || (rawDataCount != awnserCount);
            _resumeCallback = resumeCallback;
            _canResumeCallback = canResumeCallback;
            _showResultCallback = showResultCallback;
            _canShowResultCallback = canShowResultCallback;
        }

        public static ObservableObject Import(
            int? repeatable,
            string sessionId,
            int? rawDataCount,
            int? awnserCount,
            Func<CancellationToken, Task> resumeCallback,
            Func<bool> canResumeCallback,
            Func<CancellationToken, Task> showResultCallback,
            Func<bool> canShowResultCallback)
        {
            return new TestSessionEntry(
                repeatable,
                rawDataCount,
                awnserCount,
                resumeCallback,
                canResumeCallback,
                showResultCallback,
                canShowResultCallback);
        }

        public void RefreshCommands()
        {
            ResumeCommand.NotifyCanExecuteChanged();
            ShowResultCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanResume))]
        private Task Resume(CancellationToken token)
        {
            return _resumeCallback.Invoke(token);
        }

        private bool CanResume()
        {
            return _canResumeCallback.Invoke();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanShowResult))]
        private Task ShowResult(CancellationToken token)
        {
            return _showResultCallback.Invoke(token);
        }

        private bool CanShowResult()
        {
            return _canShowResultCallback.Invoke();
        }
    }
}
