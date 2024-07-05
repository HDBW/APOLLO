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

        [ObservableProperty]
        private bool _canContinue;

        private TestSessionEntry(int? repeatable, int? rawDataCount, int? awnserCount, Func<CancellationToken, Task> resumeCallback, Func<bool> canResumeCallback)
        {
            ArgumentNullException.ThrowIfNull(resumeCallback);
            ArgumentNullException.ThrowIfNull(canResumeCallback);

            CanContinue = (repeatable ?? 0) == 0 || (rawDataCount != awnserCount);
            _resumeCallback = resumeCallback;
            _canResumeCallback = canResumeCallback;
        }

        public static ObservableObject Import(
            int? repeatable,
            string sessionId,
            int? rawDataCount,
            int? awnserCount,
            Func<CancellationToken, Task> resumeCallback,
            Func<bool> canResumeCallback)
        {
            return new TestSessionEntry(repeatable, rawDataCount, awnserCount, resumeCallback, canResumeCallback);
        }

        public void RefreshCommands()
        {
            ResumeCommand.NotifyCanExecuteChanged();
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
    }
}
