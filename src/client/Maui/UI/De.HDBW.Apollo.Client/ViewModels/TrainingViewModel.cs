// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Converter;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Course;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class TrainingViewModel : BaseViewModel
    {
        private string? _trainingId;

        [ObservableProperty]
        private string? _trainingName;

        [ObservableProperty]
        private string? _subTitle;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasImage))]
        private string? _imagePath;

        [ObservableProperty]
        private string? _trainingType;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasProviderImage))]
        private string? _providerImage;

        [ObservableProperty]
        private string? _providerName;

        public TrainingViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ITrainingService trainingService,
            ILogger<TrainingViewModel> logger)
            : base(
                dispatcherService,
                navigationService,
                dialogService,
                logger)
        {
            ArgumentNullException.ThrowIfNull(trainingService);
            TrainingService = trainingService;
        }

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath);
            }
        }

        public bool HasProviderImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ProviderImage);
            }
        }

        private ITrainingService TrainingService { get; }

        public async override Task OnNavigatedToAsync()
        {
            if (string.IsNullOrWhiteSpace(_trainingId))
            {
                return;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    var training = await TrainingService.GetTrainingAsync(_trainingId!, worker.Token).ConfigureAwait(false);
                    if (training == null)
                    {
                        return;
                    }

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(training), worker.Token);
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
            _trainingId = navigationParameters.GetValue<string?>(NavigationParameter.Id);
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            ShowLoanOptionsCommand?.NotifyCanExecuteChanged();
            OpenCourseCommand?.NotifyCanExecuteChanged();
            OpenMailCommand?.NotifyCanExecuteChanged();
            OpenDailerCommand?.NotifyCanExecuteChanged();
        }

        private void LoadonUIThread(
            Training? training)
        {
            TrainingName = training?.TrainingName;
            SubTitle = training?.SubTitle;
            ImagePath = "placeholderinfoevent.png";
            TrainingType = training?.TrainingType;
            var eduProvider = training?.TrainingProvider;
            if (string.IsNullOrWhiteSpace(eduProvider?.Name))
            {
                eduProvider = training?.CourseProvider;
            }

            ProviderName = eduProvider?.Name;
            ProviderImage = eduProvider?.Image?.OriginalString;
        }

        private IEnumerable<(string Link, string Text)> ParseSkills(string? skills)
        {
            var result = new List<(string Link, string Text)>();
            try
            {
                if (!string.IsNullOrWhiteSpace(skills))
                {
                    var xml = new XmlDocument();
                    skills = System.Text.RegularExpressions.Regex.Unescape(skills);
                    xml.LoadXml(skills);
                    foreach (var node in xml.FirstChild?.ChildNodes.OfType<XmlNode>() ?? new List<XmlNode>())
                    {
                        var content = $"<html>{node.InnerText.Trim()}</html>";
                        var linkList = new XmlDocument();
                        linkList.LoadXml(content);
                        var links = linkList.FirstChild?.ChildNodes.OfType<XmlElement>() ?? new List<XmlElement>();
                        foreach (var link in links)
                        {
                            var url = link.Attributes["href"]?.Value;
                            var text = link.InnerText;
                            if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(text))
                            {
                                result.Add((url, text));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error in {nameof(ParseSkills)} in {GetType().Name}.");
            }

            return result;
        }

        private bool CanOpenSkill(InteractionEntry entry)
        {
            return true;
        }

        private async Task OpenSkill(InteractionEntry entry)
        {
            try
            {
                Logger?.LogDebug($"Try to opening url {entry.Data?.ToString()}.");
                var uri = new Uri(entry.Data?.ToString() ?? string.Empty, UriKind.Absolute);
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error in {nameof(OpenSkill)} in {GetType().Name}.");
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenCourse))]
        private async Task OpenCourse(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    //await Browser.Default.OpenAsync(CourseUrl!, BrowserLaunchMode.SystemPreferred);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenCourse)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenCourse)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenCourse)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanOpenCourse()
        {
            return !IsBusy;// && CourseUrl != null;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanShowLoanOptions))]
        private async Task ShowLoanOptions(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var parameters = new NavigationParameters();
                    //parameters.AddValue(NavigationParameter.Data, LoanOptions);
                    await DialogService.ShowPopupAsync<MessageDialog, NavigationParameters, NavigationParameters>(parameters, worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ShowLoanOptions)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ShowLoanOptions)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(ShowLoanOptions)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanShowLoanOptions()
        {
            return !IsBusy;//&& !string.IsNullOrWhiteSpace(LoanOptions);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenMail))]
        private async Task OpenMail(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    //if (Email.Default.IsComposeSupported)
                    //{
                    //    string[] recipients = new[] { Contact!.ContactMail };

                    //    var message = new EmailMessage
                    //    {
                    //        Subject = string.Empty,
                    //        Body = string.Empty,
                    //        BodyFormat = EmailBodyFormat.PlainText,
                    //        To = new List<string>(recipients),
                    //    };

                    //    await Email.Default.ComposeAsync(message);
                    //}
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

        private bool CanOpenMail()
        {
            return !IsBusy;// && !string.IsNullOrWhiteSpace(Contact?.ContactMail);
        }

        [RelayCommand(CanExecute = nameof(CanOpenDailer))]
        private void OpenDailer()
        {
            try
            {
                if (PhoneDialer.Default.IsSupported)
                {
                    //PhoneDialer.Default.Open(Contact!.ContactPhone);
                }
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
        }

        private bool CanOpenDailer()
        {
            return !IsBusy;// && !string.IsNullOrWhiteSpace(Contact?.ContactPhone);
        }
    }
}
