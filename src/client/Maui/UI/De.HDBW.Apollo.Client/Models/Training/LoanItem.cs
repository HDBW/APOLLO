// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class LoanItem : ContactItem
    {
        private LoanItem(
            Loans loan,
            Func<Uri?, CancellationToken, Task>? openUrlHandler,
            Func<Uri?, bool>? canOpenUrlHandler,
            Contact contact,
            Func<string?, CancellationToken, Task>? openMailHandler,
            Func<string?, bool>? canOpenMailHandler,
            Func<string?, CancellationToken, Task>? openDailerHandler,
            Func<string?, bool>? canOpenDailerHandler)
            : base(loan.Name, contact, openMailHandler, canOpenMailHandler, openDailerHandler, canOpenDailerHandler)
        {
            if (!string.IsNullOrWhiteSpace(loan.Description))
            {
                Items.Insert(0, LineItem.Import(null, loan.Description));
            }

            if (loan.Url != null)
            {
                Items.Add(InteractiveLineItem.Import(KnonwIcons.Web, loan.Url.OriginalString));
            }
        }

        public static LoanItem Import(
            Loans loan,
            Func<Uri?, CancellationToken, Task>? openUrlHandler,
            Func<Uri?, bool>? canOpenUrlHandler,
            Func<string?, CancellationToken, Task>? openMailHandler,
            Func<string?, bool>? canOpenMailHandler,
            Func<string?, CancellationToken, Task>? openDailerHandler,
            Func<string?, bool>? canOpenDailerHandler)
        {
            return new LoanItem(loan, openUrlHandler, canOpenUrlHandler, loan.LoanContact ?? new Contact(), openMailHandler, canOpenMailHandler, openDailerHandler, canOpenDailerHandler);
        }
    }
}
