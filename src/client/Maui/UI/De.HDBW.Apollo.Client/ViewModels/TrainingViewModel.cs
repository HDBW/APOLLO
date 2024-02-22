// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Training;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;
using TrainingModel = Invite.Apollo.App.Graph.Common.Models.Trainings.Training;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class TrainingViewModel : BaseViewModel
    {
        private readonly object _lockObject = new object();

        private readonly ConcurrentDictionary<Uri, List<IProvideImageData>> _loadingCache = new ConcurrentDictionary<Uri, List<IProvideImageData>>();

        private bool _canceled;

        private string? _trainingId;

        private CancellationTokenSource? _loadingCts;

        private List<Task>? _loadingTask;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasProductUrl))]
        private Uri? _productUrl;

        [ObservableProperty]
        private string? _price;

        [ObservableProperty]
        private ObservableCollection<ObservableObject> _sections = new ObservableCollection<ObservableObject>();

        private TrainingModel? _training;

        public TrainingViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ITrainingService trainingService,
            IImageCacheService imageCacheService,
            ILogger<TrainingViewModel> logger)
            : base(
                dispatcherService,
                navigationService,
                dialogService,
                logger)
        {
            ArgumentNullException.ThrowIfNull(trainingService);
            ArgumentNullException.ThrowIfNull(imageCacheService);
            TrainingService = trainingService;
            ImageCacheService = imageCacheService;
        }

        public bool HasProductUrl
        {
            get
            {
                return ProductUrl != null;
            }
        }

        private ITrainingService TrainingService { get; }

        private IImageCacheService ImageCacheService { get; }

        public async override Task OnNavigatedToAsync()
        {
            if (string.IsNullOrWhiteSpace(_trainingId) || _training != null)
            {
                return;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    await Task.Run(
                        async () =>
                    {
                        var training = await TrainingService.GetTrainingAsync(_trainingId!, worker.Token).ConfigureAwait(false);
                        Logger?.LogDebug("StartCreationOfSections");
                        var sections = new List<ObservableObject>();
                        var addedItem = false;
                        if (training != null)
                        {
                            if (TryCreateTrainingsHeader(training, out ObservableObject header))
                            {
                                sections.Add(header);
                                addedItem = true;
                            }

                            if (TryCreateTagItem(Resources.Strings.Resources.Global_Tags, training.Tags, out ObservableObject? tags))
                            {
                                if (addedItem)
                                {
                                    sections.Add(SeperatorItem.Import());
                                    addedItem = false;
                                }

                                sections.Add(tags);
                                addedItem = true;
                            }

                            if (TryCreateTagItem(Resources.Strings.Resources.Global_Categories, training.Categories, out ObservableObject? categories))
                            {
                                if (addedItem)
                                {
                                    sections.Add(SeperatorItem.Import());
                                    addedItem = false;
                                }

                                sections.Add(categories);
                                addedItem = true;
                            }

                            if (TryCreateExpandableItem(Resources.Strings.Resources.Global_Description, training.Description ?? training.ShortDescription, out ObservableObject? description))
                            {
                                if (addedItem)
                                {
                                    sections.Add(SeperatorItem.Import());
                                    addedItem = false;
                                }

                                sections.Add(description);
                                addedItem = true;
                            }

                            if (TryCreateExpandableItem(Resources.Strings.Resources.Global_TargetAudience, training.TargetAudience, out ObservableObject? targetAudience))
                            {
                                if (addedItem)
                                {
                                    sections.Add(SeperatorItem.Import());
                                    addedItem = false;
                                }

                                sections.Add(targetAudience);
                                addedItem = true;
                            }

                            if (TryCreateExpandableListItem(Resources.Strings.Resources.Global_PreRequisites, training.Prerequisites, out ObservableObject? prerequisites))
                            {
                                if (addedItem)
                                {
                                    sections.Add(SeperatorItem.Import());
                                    addedItem = false;
                                }

                                sections.Add(prerequisites);
                                addedItem = true;
                            }

                            if (TryCreateNavigationItem(Resources.Strings.Resources.Global_Contents, Routes.TrainingContentView, training.Content.Serialize<List<string>>(), out ObservableObject? contents))
                            {
                                if (addedItem)
                                {
                                    sections.Add(SeperatorItem.Import());
                                    addedItem = false;
                                }

                                sections.Add(contents);
                                addedItem = true;
                            }

                            if (TryCreateExpandableListItem(Resources.Strings.Resources.Global_Benefits, training.BenefitList, out ObservableObject? benefits))
                            {
                                if (addedItem)
                                {
                                    sections.Add(SeperatorItem.Import());
                                    addedItem = false;
                                }

                                sections.Add(benefits);
                                addedItem = true;
                            }

                            if (TryCreateExpandableListItem(Resources.Strings.Resources.Global_Certificates, training.Certificate, out ObservableObject? certificates))
                            {
                                if (addedItem)
                                {
                                    sections.Add(SeperatorItem.Import());
                                    addedItem = false;
                                }

                                sections.Add(certificates);
                                addedItem = true;
                            }

                            if (TryCreateNavigationItem(Resources.Strings.Resources.TrainingsView_LoanOptions, Routes.LoansView, training.Loans?.Serialize(), out ObservableObject? loans))
                            {
                                if (addedItem)
                                {
                                    sections.Add(SeperatorItem.Import());
                                    addedItem = false;
                                }

                                sections.Add(loans);
                                addedItem = true;
                            }

                            if (TryCreateContactListItem(Resources.Strings.Resources.Global_Contact, training.Contacts, out ObservableObject? contacts))
                            {
                                if (addedItem)
                                {
                                    sections.Add(SeperatorItem.Import());
                                    addedItem = false;
                                }

                                sections.Add(contacts);
                                addedItem = true;
                            }

                            if (TryCreateExpandableItem(Resources.Strings.Resources.TraingsView_PriceDescription, training.PriceDescription, out ObservableObject? priceDescription))
                            {
                                if (addedItem)
                                {
                                    sections.Add(SeperatorItem.Import());
                                    addedItem = false;
                                }

                                sections.Add(priceDescription);
                                addedItem = true;
                            }

                            if (TryCreateContactItem(Resources.Strings.Resources.Global_Contact, training.Contacts, out ObservableObject? contact))
                            {
                                if (addedItem)
                                {
                                    sections.Add(SeperatorItem.Import());
                                    addedItem = false;
                                }

                                sections.Add(contact);
                                addedItem = true;
                            }

                            if (TryCreateNavigationItem(Resources.Strings.Resources.TrainingsView_Appointment, Routes.AppointmentsView, training.Appointment?.Serialize(), out ObservableObject? appointment))
                            {
                                if (addedItem)
                                {
                                    sections.Add(SeperatorItem.Import());
                                    addedItem = false;
                                }

                                sections.Add(appointment);
                                addedItem = true;
                            }

                            if (sections.Any())
                            {
                                sections.Add(SeperatorItem.Import());
                            }
                        }

                        await ExecuteOnUIThreadAsync(() => LoadonUIThread(training, sections), worker.Token).ConfigureAwait(false);
                    }, worker.Token);

                    _loadingCts?.Cancel();
                    _loadingCts = new CancellationTokenSource();
                    _loadingTask = new List<Task>();
                    foreach (var url in _loadingCache.Keys.ToList())
                    {
                        _loadingTask.Add(ImageCacheService.DownloadAsync(url, _loadingCts.Token).ContinueWith(OnApplyImageData));
                    }
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
            OpenProductCommand?.NotifyCanExecuteChanged();
            NavigateBackCommand?.NotifyCanExecuteChanged();
            var contactListSection = Sections?.OfType<ContactListItem>().FirstOrDefault();
            contactListSection?.RefreshCommands();
            var contactItemSection = Sections?.OfType<ContactItem>().FirstOrDefault();
            contactItemSection?.RefreshCommands();
            var navigationItems = Sections?.OfType<NavigationItem>().ToList() ?? new List<NavigationItem>();
            foreach (var naviagtionItem in navigationItems)
            {
                naviagtionItem.RefreshCommands();
            }
        }

        private async void OnApplyImageData(Task<(Uri Uri, string? Data)> task)
        {
            var result = task.Result;
            _loadingTask?.Remove(task);
            if (!(_loadingTask?.Any() ?? false))
            {
                _loadingCache.Clear();
                _loadingCts?.Dispose();
                _loadingCts = null;
            }

            if (string.IsNullOrWhiteSpace(result.Data))
            {
                return;
            }

            if (!_loadingCache.TryGetValue(result.Uri, out List<IProvideImageData>? entries))
            {
                return;
            }

            await ExecuteOnUIThreadAsync(
                () =>
                {
                    foreach (var entry in entries)
                    {
                        entry.ImageData = result.Data;
                    }
                }, CancellationToken.None);
        }

        private async void LoadonUIThread(
            TrainingModel? training, List<ObservableObject> sections)
        {
            _training = training;
            ProductUrl = _training?.ProductUrl;
            Price = $"{_training?.Price ?? 0:0.##} €";
            foreach (var section in sections)
            {
                await DispatcherService.BeginInvokeOnMainThreadAsync(
                    () =>
                    {
                        lock (_lockObject)
                        {
                            if (_canceled)
                            {
                                return;
                            }

                            Sections.Add(section);
                        }
                    },
                    CancellationToken.None);
            }

            Logger?.LogDebug("Finished");
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenProduct))]
        private async Task OpenProduct(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await Browser.Default.OpenAsync(ProductUrl!, BrowserLaunchMode.SystemPreferred);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenProduct)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenProduct)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenProduct)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanNavigateBack))]
        private async Task NavigateBack(CancellationToken token)
        {
            // see https://github.com/dotnet/maui/issues/7237
            lock (_lockObject)
            {
                _canceled = true;
            }

            _loadingCts?.Cancel();
            _loadingCts = null;
            _loadingTask = null;
            _loadingCache.Clear();
            await NavigationService.PopAsync(token);
        }

        private bool CanNavigateBack()
        {
            return !IsBusy;
        }

        private bool CanOpenProduct()
        {
            return !IsBusy && ProductUrl != null;
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

        private bool CanNavigateToRouteHandler(NavigationItem item)
        {
            return !IsBusy;
        }

        private async Task NavigateToRouteHandler(NavigationItem item, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.NavigateAsync(item.Route, token, item.Parameters).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NavigateToRouteHandler)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NavigateToRouteHandler)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(NavigateToRouteHandler)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanOpenDailer(string? phoneNumber)
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(phoneNumber) && PhoneDialer.Default.IsSupported;
        }

        private bool TryCreateContactListItem(string headline, List<Contact>? contacts, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            if (contacts == null || contacts.Count() < 2)
            {
                return false;
            }

            var contactItem = ContactListItem.Import(
                headline,
                contacts,
                OpenMail,
                CanOpenMail,
                OpenDailer,
                CanOpenDailer);

            if (!contactItem.Items.Any())
            {
                return false;
            }

            item = contactItem;
            return true;
        }

        private bool TryCreateContactItem(string headline, List<Contact>? contacts, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            if (contacts == null || contacts.Count() != 1)
            {
                return false;
            }

            var contact = ContactItem.Import(
                headline,
                contacts.First(),
                OpenMail,
                CanOpenMail,
                OpenDailer,
                CanOpenDailer);

            if (!contact.Items.Any())
            {
                return false;
            }

            item = contact;

            return true;
        }

        private bool TryCreateExpandableListItem(string headline, IEnumerable<string>? content, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            if (!(content?.Any() ?? false))
            {
                return false;
            }

            content = content.Select(x => x.Trim());

            item = ExpandableListItem.Import(headline, content, OnToggleState);
            return true;
        }

        private bool TryCreateAppointmentItem(Appointment appointment, out ObservableObject item)
        {
            item = AppointmentItem.Import(appointment);
            return true;
        }

        private bool TryCreateTagItem(string? headline, List<string>? items, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            var validItems = items?.Where(x => !string.IsNullOrWhiteSpace(x)) ?? new List<string>();
            if (!(validItems?.Any() ?? false))
            {
                return false;
            }

            item = TagItem.Import(headline, validItems);
            return true;
        }

        private bool TryCreateExpandableItem(string headline, string? content, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            if (string.IsNullOrWhiteSpace(content))
            {
                return false;
            }

            item = ExpandableItem.Import(headline, content, OnToggleState);
            return true;
        }

        private bool TryCreateTrainingsHeader(TrainingModel training, out ObservableObject item)
        {
            var eduProvider = training.TrainingProvider;
            if (string.IsNullOrWhiteSpace(eduProvider?.Name))
            {
                eduProvider = training.CourseProvider;
            }

            var header = TrainingsHeaderItem.Import(
                     training?.TrainingName,
                     training?.SubTitle,
                     "placeholderinfoevent.png",
                     training?.TrainingType,
                     eduProvider?.Name ?? Resources.Strings.Resources.Global_UnknownProvider,
                     eduProvider?.Image?.OriginalString,
                     training?.AccessibilityAvailable,
                     training?.TrainingMode,
                     training?.IndividualStartDate);

            item = header;

            if (eduProvider?.Image != null)
            {
                if (eduProvider?.Image != null && eduProvider.Image.IsWellFormedOriginalString())
                {
                    if (!_loadingCache.Keys.Any(x => x.OriginalString == eduProvider.Image.OriginalString))
                    {
                        if (!_loadingCache.TryAdd(eduProvider.Image, new List<IProvideImageData>() { header }))
                        {
                            _loadingCache[eduProvider.Image].Add(header);
                        }
                    }
                    else
                    {
                        _loadingCache[eduProvider.Image].Add(header);
                    }
                }
            }

            return true;
        }

        private bool TryCreateNavigationItem(string text, string route, string? data, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            if (string.IsNullOrWhiteSpace(data) || string.Equals(data, "[]"))
            {
                return false;
            }

            var parameters = new NavigationParameters();
            parameters.AddValue(NavigationParameter.Id, _training?.Id);
            parameters.AddValue(NavigationParameter.Data, data!);
            item = NavigationItem.Import(text, route, parameters, NavigateToRouteHandler, CanNavigateToRouteHandler);
            return true;
        }

        private void OnToggleState(ExpandableItem item)
        {
            var index = Sections.IndexOf(item);
            if (item.IsExpanded)
            {
                Sections.Insert(index + 1, ExpandedItemContent.Import(item.Content));
            }
            else
            {
                Sections.RemoveAt(index + 1);
            }
        }

        private void OnToggleState(ExpandableListItem item)
        {
            var index = Sections.IndexOf(item);
            if (item.IsExpanded)
            {
                Sections.Insert(index + 1, ExpandedListContent.Import(item.Content));
            }
            else
            {
                Sections.RemoveAt(index + 1);
            }
        }
    }
}
