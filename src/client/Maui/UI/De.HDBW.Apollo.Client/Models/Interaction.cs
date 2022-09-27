namespace De.HDBW.Apollo.Client.Models
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    public partial class Interaction : ObservableObject
    {
        [ObservableProperty]
        private string text;

        private Func<Interaction, bool> canNavigateHandle;

        private Func<Interaction, Task> navigateHandler;

        private Interaction(string text, Func<Interaction, Task> navigateHandler, Func<Interaction, bool> canNavigateHandle)
        {
            this.text = text;
            this.canNavigateHandle = canNavigateHandle;
            this.navigateHandler = navigateHandler;
        }

        public static Interaction Import(string text, Func<Interaction, Task> navigateHandler, Func<Interaction, bool> canNavigateHandle)
        {
            return new Interaction(text, navigateHandler, canNavigateHandle);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanNavigate))]
        private Task Navigate()
        {
            return this.navigateHandler?.Invoke(this);
        }

        private bool CanNavigate()
        {
            return this.canNavigateHandle?.Invoke(this) ?? false;
        }
    }
}
