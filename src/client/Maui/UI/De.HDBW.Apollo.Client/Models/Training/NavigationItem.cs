// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class NavigationItem : ObservableObject
    {
        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        private string _route;

        private NavigationItem(string text, string route, NavigationParameters? parameters, Func<NavigationItem, CancellationToken, Task>? navigateHander, Func<NavigationItem, bool>? canNavigateHander)
        {
            Text = text;
            Route = route;
            CanNavigateHander = canNavigateHander;
            NavigateHander = navigateHander;
            Parameters = parameters;
        }

        public NavigationParameters? Parameters { get; }

        private Func<NavigationItem, bool>? CanNavigateHander { get; }

        private Func<NavigationItem, CancellationToken, Task>? NavigateHander { get; }

        public static NavigationItem Import(string text, string route, NavigationParameters? prameters, Func<NavigationItem, CancellationToken, Task>? navigateHander, Func<NavigationItem, bool>? canNavigateHander)
        {
            return new NavigationItem(text, route, prameters, navigateHander, canNavigateHander);
        }

        public void RefreshCommands()
        {
            NavigateCommand.NotifyCanExecuteChanged();
        }

        private bool CanNavigate()
        {
            return CanNavigateHander?.Invoke(this) ?? false;
        }

        [RelayCommand(CanExecute = nameof(CanNavigate), AllowConcurrentExecutions =false)]
        private Task Navigate(CancellationToken token)
        {
            return NavigateHander?.Invoke(this, token) ?? Task.CompletedTask;
        }
    }
}
