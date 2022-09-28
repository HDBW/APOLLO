namespace De.HDBW.Apollo.Client.Contracts
{
    using CommunityToolkit.Mvvm.Input;

    public interface ISelectableItem<TU>
    {
        bool IsSelected { get; set; }

        IRelayCommand ToggleSelectionCommand { get; }

        void UpdateSelectedState(bool isSelected);
    }
}
