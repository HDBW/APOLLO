// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class InteractiveLineItem : LineItem
    {
        private InteractiveLineItem(string? icon, string text, Func<string?, CancellationToken, Task>? executeHander, Func<string?, bool>? canExecuteHander)
            : base(icon, text)
        {
            CanExecuteHander = canExecuteHander;
            ExecuteHander = executeHander;
        }

        private Func<string?, bool>? CanExecuteHander { get; }

        private Func<string?, CancellationToken, Task>? ExecuteHander { get; }

        public static InteractiveLineItem Import(string? icon, string text, Func<string?, CancellationToken, Task>? executeHander, Func<string?, bool>? canExecuteHander)
        {
            return new InteractiveLineItem(icon, text, executeHander, canExecuteHander);
        }

        private bool CanInteract()
        {
            return CanExecuteHander?.Invoke(Text) ?? false;
        }

        [RelayCommand(CanExecute = nameof(CanInteract), AllowConcurrentExecutions =false)]
        private Task Interact(CancellationToken token)
        {
            return ExecuteHander?.Invoke(Text, token) ?? Task.CompletedTask;
        }
    }
}
