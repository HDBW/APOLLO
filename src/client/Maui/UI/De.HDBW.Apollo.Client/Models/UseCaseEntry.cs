namespace De.HDBW.Apollo.Client.Models
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using De.HDBW.Apollo.Client.Contracts;
    using De.HDBW.Apollo.SharedContracts.Enums;

    public class UseCaseEntry : ObservableObject, ISelectableItem<UseCaseEntry>
    {
        private readonly UseCase useCase;
        private bool isSelected;

        private UseCaseEntry(UseCase useCase, Action<UseCaseEntry> selectionChangedHandler)
        {
            this.useCase = useCase;
            this.SelectionChangedHandler = selectionChangedHandler;
        }

        public UseCase UseCase
        {
            get
            {
                return this.useCase;
            }
        }

        public string DisplayUseCaseName
        {
            get
            {
                switch (this.UseCase)
                {
                    case UseCase.A:
                        return Resources.Strings.Resource.UseCase_A_Name;

                    case UseCase.B:
                        return Resources.Strings.Resource.UseCase_B_Name;

                    case UseCase.C:
                        return Resources.Strings.Resource.UseCase_C_Name;

                    default:
                        return string.Empty;
                }
            }
        }

        public string DisplayUseCaseDescription
        {
            get
            {
                switch (this.UseCase)
                {
                    case UseCase.A:
                        return Resources.Strings.Resource.UseCase_A_Description;

                    case UseCase.B:
                        return Resources.Strings.Resource.UseCase_B_Description;

                    case UseCase.C:
                        return Resources.Strings.Resource.UseCase_C_Description;

                    default:
                        return string.Empty;
                }
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                if (this.SetProperty(ref this.isSelected, value))
                {
                    this.SelectionChangedHandler?.Invoke(this);
                }
            }
        }

        private Action<UseCaseEntry> SelectionChangedHandler { get; }

        public static UseCaseEntry Import(UseCase useCase, Action<UseCaseEntry> selectionChangedHandler)
        {
            return new UseCaseEntry(useCase, selectionChangedHandler);
        }

        public void UpdateSelectedState(bool isSelected)
        {
            this.isSelected = isSelected;
            this.OnPropertyChanged(nameof(this.IsSelected));
        }
    }
}
