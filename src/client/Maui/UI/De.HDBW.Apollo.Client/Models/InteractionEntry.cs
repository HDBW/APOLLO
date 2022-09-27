namespace De.HDBW.Apollo.Client.Models
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    public partial class InteractionEntry : ObservableObject
    {
        [ObservableProperty]
        private string text;

        private Func<InteractionEntry, bool> canNavigateHandle;

        private Func<InteractionEntry, Task> navigateHandler;

        private InteractionEntry(string text, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle)
        {
            this.text = text;
            this.canNavigateHandle = canNavigateHandle;
            this.navigateHandler = navigateHandler;
        }

        public static InteractionEntry Import(string text, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle)
        {
            return new InteractionEntry(text, navigateHandler, canNavigateHandle);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanNavigate))]
        private Task Navigate()
        {
            return this.navigateHandler?.Invoke(this) ?? Task.CompletedTask;
        }

        private bool CanNavigate()
        {
            return this.canNavigateHandle?.Invoke(this) ?? false;
        }
    }
}
