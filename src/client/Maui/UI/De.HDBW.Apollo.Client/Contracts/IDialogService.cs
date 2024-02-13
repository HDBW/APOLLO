// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IDialogService
    {
        bool ClosePopup<TV>(BaseViewModel viewModel, TV result)
            where TV : NavigationParameters;

        Task<TV?> ShowPopupAsync<TU, TV, TW>(TW parameters, CancellationToken token)
            where TU : Dialog
            where TV : NavigationParameters
            where TW : NavigationParameters?;

        Task<TV?> ShowPopupAsync<TU, TV>(CancellationToken token)
            where TU : Dialog
            where TV : NavigationParameters;
    }
}
