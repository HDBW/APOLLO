﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.SharedContracts.Enums;

namespace De.HDBW.Apollo.Client.Models
{
    public partial class UseCaseEntry : ObservableObject, ISelectableItem<UseCaseEntry>
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
                switch (UseCase)
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