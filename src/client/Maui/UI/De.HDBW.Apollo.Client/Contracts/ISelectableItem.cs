using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface ISelectableItem<TU>
    {
        bool IsSelected { get; set; }

        IRelayCommand ToggleSelectionCommand { get; }

        void UpdateSelectedState(bool isSelected);
    }
}
