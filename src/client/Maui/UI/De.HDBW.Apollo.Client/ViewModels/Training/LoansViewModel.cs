// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Training;
using De.HDBW.Apollo.Data.Helper;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Training
{
    public partial class LoansViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<LoanItem> _sections = new ObservableCollection<LoanItem>();

        private List<Loans>? _loans;

        public LoansViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<LoansViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sections = new List<LoanItem>();
                    var loanItems = _loans?.Select(x => LoanItem.Import(x, OpenLink, CanOpenLink, OpenMail, CanOpenMail, OpenDailer, CanOpenDailer)) ?? new List<LoanItem>();
                    sections.AddRange(loanItems);
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(sections), worker.Token).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            var data = navigationParameters.GetValue<string?>(NavigationParameter.Data);
            _loans = data.Deserialize<List<Loans>>();
        }

        private void LoadonUIThread(List<LoanItem> sections)
        {
            Sections = new ObservableCollection<LoanItem>(sections);
        }

        private async Task OpenLink(Uri? uri, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await Browser.Default.OpenAsync(uri!, BrowserLaunchMode.SystemPreferred);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenLink)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenLink)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenLink)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanOpenLink(Uri? uri)
        {
            return !IsBusy && uri != null;
        }

        private async Task OpenMail(string? email, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    string[] recipients = new[] { email! };

                    var message = new EmailMessage
                    {
                        Subject = string.Empty,
                        Body = string.Empty,
                        BodyFormat = EmailBodyFormat.PlainText,
                        To = new List<string>(recipients),
                    };

                    await Email.Default.ComposeAsync(message);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenMail)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenMail)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenMail)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanOpenMail(string? email)
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(email) && Email.Default.IsComposeSupported;
        }

        private Task OpenDailer(string? phoneNumber, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    PhoneDialer.Default.Open(phoneNumber!);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenDailer)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenDailer)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenDailer)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }

                return Task.CompletedTask;
            }
        }

        private bool CanOpenDailer(string? phoneNumber)
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(phoneNumber) && PhoneDialer.Default.IsSupported;
        }
    }
}
