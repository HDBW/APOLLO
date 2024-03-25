// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class ButtonLineItem : InteractiveLineItem
    {
        [ObservableProperty]
        private string? _content;

        private ButtonLineItem(string content, string? icon, string text, Func<string?, CancellationToken, Task>? executeHander, Func<string?, bool>? canExecuteHander)
            : base(icon, text, executeHander, canExecuteHander)
        {
            Content = content;
        }

        public static ButtonLineItem Import(string content, string? icon, string text, Func<string?, CancellationToken, Task>? executeHander, Func<string?, bool>? canExecuteHander)
        {
            return new ButtonLineItem(content, icon, text, executeHander, canExecuteHander);
        }
    }
}
