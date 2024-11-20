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
        private readonly Func<CancellationToken, Task> _cancelCallback;
        private readonly Func<bool> _canCancelCallback;
        private readonly Func<CancellationToken, Task> _showResultCallback;
        private readonly Func<bool> _canShowResultCallback;
        private readonly string _progressTextFormat;
        private readonly string _repeatTextFormat;
        private readonly int _rawDataCount;
        private readonly int _answerCount;

        [ObservableProperty]
        private bool _canContinue;

        [ObservableProperty]
        private int _repeatable;

        private TestSessionEntry(
            int? repeatable,
            int? rawDataCount,
            int? answerCount,
            string progressTextFormat,
            string repeatTextFormat,
            Func<CancellationToken, Task> resumeCallback,
            Func<bool> canResumeCallback,
            Func<CancellationToken, Task> cancelCallback,
            Func<bool> canCancelCallback,
            Func<CancellationToken, Task> showResultCallback,
            Func<bool> canShowResultCallback)
        {
            ArgumentNullException.ThrowIfNull(progressTextFormat);
            ArgumentNullException.ThrowIfNull(repeatTextFormat);
            ArgumentNullException.ThrowIfNull(resumeCallback);
            ArgumentNullException.ThrowIfNull(canResumeCallback);
            ArgumentNullException.ThrowIfNull(cancelCallback);
            ArgumentNullException.ThrowIfNull(canCancelCallback);
            ArgumentNullException.ThrowIfNull(showResultCallback);
            ArgumentNullException.ThrowIfNull(canShowResultCallback);
            Repeatable = repeatable ?? 0;
            CanContinue = (repeatable ?? 0) == 0 && (rawDataCount != answerCount);
            _rawDataCount = rawDataCount ?? 0;
            _answerCount = answerCount ?? 0;
            _resumeCallback = resumeCallback;
            _canResumeCallback = canResumeCallback;
            _cancelCallback = cancelCallback;
            _canCancelCallback = canCancelCallback;
            _showResultCallback = showResultCallback;
            _canShowResultCallback = canShowResultCallback;
            _progressTextFormat = progressTextFormat;
            _repeatTextFormat = repeatTextFormat;
        }

        public string RepeatText
        {
            get
            {
                return string.Format(_repeatTextFormat, Repeatable);
            }
        }

        public string ProgressText
        {
            get
            {
                return string.Format(_progressTextFormat, _answerCount, _rawDataCount);
            }
        }

        public double Progress
        {
            get
            {
                if (_rawDataCount == 0)
                {
                    return 0d;
                }

                return Math.Round((double)((double)_answerCount / (double)_rawDataCount), 2);
            }
        }

        public static ObservableObject Import(
            int? repeatable,
            string sessionId,
            int? rawDataCount,
            int? answerCount,
            string progressTextFormat,
            string repeatTextFormat,
            Func<CancellationToken, Task> resumeCallback,
            Func<bool> canResumeCallback,
            Func<CancellationToken, Task> cancelCallback,
            Func<bool> canCancelCallback,
            Func<CancellationToken, Task> showResultCallback,
            Func<bool> canShowResultCallback)
        {
            return new TestSessionEntry(
                repeatable,
                rawDataCount,
                answerCount,
                progressTextFormat,
                repeatTextFormat,
                resumeCallback,
                canResumeCallback,
                cancelCallback,
                canCancelCallback,
                showResultCallback,
                canShowResultCallback);
        }

        public void RefreshCommands()
        {
            ResumeCommand.NotifyCanExecuteChanged();
            CancelCommand.NotifyCanExecuteChanged();
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

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanCancel))]
        private Task Cancel(CancellationToken token)
        {
            return _cancelCallback.Invoke(token);
        }

        private bool CanCancel()
        {
            return _canCancelCallback.Invoke();
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
