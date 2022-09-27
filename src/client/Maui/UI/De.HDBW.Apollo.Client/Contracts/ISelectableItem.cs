namespace De.HDBW.Apollo.Client.Contracts
{
    public interface ISelectableItem<TU>
    {
        bool IsSelected { get; set; }

        void UpdateSelectedState(bool isSelected);
    }
}
