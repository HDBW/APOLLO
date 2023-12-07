// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.SharedContracts.Enums;

namespace De.HDBW.Apollo.Client.Models
{
    public partial class UseCaseEntry : ObservableObject, ISelectableEntry
    {
        private readonly UseCase _useCase;
        private bool _isSelected;

        private UseCaseEntry(UseCase useCase, Action<UseCaseEntry> selectionChangedHandler)
        {
            _useCase = useCase;
            SelectionChangedHandler = selectionChangedHandler;
        }

        public UseCase UseCase
        {
            get
            {
                return _useCase;
            }
        }

        public string DisplayUseCaseName
        {
            get
            {
                switch (UseCase)
                {
                    case UseCase.A:
                        return Resources.Strings.Resources.UseCase_A_Name;

                    case UseCase.B:
                        return Resources.Strings.Resources.UseCase_B_Name;

                    case UseCase.C:
                        return Resources.Strings.Resources.UseCase_C_Name;
                    case UseCase.D:
                        return Resources.Strings.Resources.UseCase_D_Name;
                    default:
                        return string.Empty;
                }
            }
        }

        public string ImagePath
        {
            get
            {
                switch (UseCase)
                {
                    case UseCase.A:
                        return "usecase1deco.png";

                    case UseCase.B:
                        return "usecase2deco.png";

                    case UseCase.C:
                        return "usecase3deco.png";

                    case UseCase.D:
                        return "usecase4deco.png";

                    default:
                        return string.Empty;
                }
            }
        }

        public string DisplayUseCaseDescription
        {
            get
            {
                switch (UseCase)
                {
                    case UseCase.A:
                        return Resources.Strings.Resources.UseCase_A_Description;

                    case UseCase.B:
                        return Resources.Strings.Resources.UseCase_B_Description;

                    case UseCase.C:
                        return Resources.Strings.Resources.UseCase_C_Description;

                    case UseCase.D:
                        return Resources.Strings.Resources.UseCase_D_Description;
                    default:
                        return string.Empty;
                }
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                if (SetProperty(ref _isSelected, value))
                {
                    SelectionChangedHandler?.Invoke(this);
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
            _isSelected = isSelected;
            OnPropertyChanged(nameof(IsSelected));
        }

        [RelayCommand]
        private void ToggleSelection()
        {
            IsSelected = !IsSelected;
        }
    }
}
