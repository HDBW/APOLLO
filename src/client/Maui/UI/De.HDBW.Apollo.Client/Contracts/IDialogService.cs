﻿using CommunityToolkit.Maui.Views;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IDialogService
    {
        bool ClosePopup(BaseViewModel viewModel);

        bool ClosePopup<TV>(BaseViewModel viewModel, TV result)
            where TV : NavigationParameters;

        Task<TV?> ShowPopupAsync<TU, TV>(CancellationToken token)
            where TU : Popup
            where TV : NavigationParameters;

        Task ShowPopupAsync<TU>(CancellationToken token)
            where TU : Popup;
    }
}