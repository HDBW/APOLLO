// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class LoanItem : ContactItem
    {
        [ObservableProperty]
        private string? _description;

        private LoanItem(
            Loans loan,
            Func<string?, CancellationToken, Task>? openUrlHandler,
            Func<string?, bool>? canOpenUrlHandler,
            Contact contact,
            Func<string?, CancellationToken, Task>? openMailHandler,
            Func<string?, bool>? canOpenMailHandler,
            Func<string?, CancellationToken, Task>? openDailerHandler,
            Func<string?, bool>? canOpenDailerHandler)
            : base(loan.Name, contact, openMailHandler, canOpenMailHandler, openDailerHandler, canOpenDailerHandler)
        {
            Description = loan.Description;
            if (loan.Url != null)
            {
                Items.Add(InteractiveLineItem.Import(KnownIcons.Web, loan.Url.OriginalString, openUrlHandler, canOpenUrlHandler));
            }
        }

        public static LoanItem Import(
            Loans loan,
            Func<string?, CancellationToken, Task>? openUrlHandler,
            Func<string?, bool>? canOpenUrlHandler,
            Func<string?, CancellationToken, Task>? openMailHandler,
            Func<string?, bool>? canOpenMailHandler,
            Func<string?, CancellationToken, Task>? openDailerHandler,
            Func<string?, bool>? canOpenDailerHandler)
        {
            return new LoanItem(loan, openUrlHandler, canOpenUrlHandler, loan.LoanContact ?? new Contact(), openMailHandler, canOpenMailHandler, openDailerHandler, canOpenDailerHandler);
        }
    }
}
