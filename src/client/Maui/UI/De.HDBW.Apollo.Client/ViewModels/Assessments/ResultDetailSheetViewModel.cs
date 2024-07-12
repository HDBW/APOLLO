// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.Client.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessments;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class ResultDetailSheetViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ObservableObject> _sections = new ObservableCollection<ObservableObject>();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FlowDirection))]
        private CultureInfo _culture = new CultureInfo("de-DE");

        private string? _data;
        private string _language;
        private AssessmentType _type = AssessmentType.Unknown;

        public ResultDetailSheetViewModel(
            ISheetService sheetService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ResultDetailSheetViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(nameof(sheetService));
            SheetService = sheetService;
        }

        public FlowDirection FlowDirection
        {
            get { return Culture?.TextInfo.IsRightToLeft ?? false ? FlowDirection.RightToLeft : FlowDirection.LeftToRight; }
        }

        private ISheetService SheetService { get; }

        [IndexerName("Item")]
        public new string this[string key]
        {
            get
            {
                var localizedResource = string.IsNullOrWhiteSpace(_language) ? null : Resources.Strings.Resources.ResourceManager.GetString($"{key}_{_language}");
                return localizedResource ?? Resources.Strings.Resources.ResourceManager.GetString(key) ?? string.Empty;
            }
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sections = new List<ObservableObject>();
                    if (!string.IsNullOrWhiteSpace(_data))
                    {
                        var quantity_Patter = "Quantity_{0}";
                        var items = JsonSerializer.Deserialize<List<SegmentScore>>(_data) ?? new List<SegmentScore>();
                        switch (_type)
                        {
                            case AssessmentType.Sk:
                                foreach (var item in items)
                                {
                                    sections.Add(SegmentScoreEntry.Import(item, this[string.Format(quantity_Patter, item.Quantity)], _type));
                                }

                                break;
                            case AssessmentType.So:
                                foreach (var item in items)
                                {
                                    sections.Add(TextEntry.Import(item.ResultDetail));
                                }

                                break;
                        }
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(sections), worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
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
            _data = navigationParameters.GetValue<string?>(NavigationParameter.Data);
            _language = navigationParameters.GetValue<string?>(NavigationParameter.Language) ?? "de-DE";
            var type = navigationParameters.GetValue<string?>(NavigationParameter.Type) ?? nameof(AssessmentType.Unknown);
            _type = Enum.Parse<AssessmentType>(type, true);
            Culture = new CultureInfo(_language);
            OnPropertyChanged(string.Empty);
        }

        protected override void RefreshCommands()
        {
            CloseCommand?.NotifyCanExecuteChanged();
        }

        private void LoadonUIThread(List<ObservableObject> sections)
        {
            Sections = new ObservableCollection<ObservableObject>(sections);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanClose), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private async Task Close(CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(CloseCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork())
            {
                try
                {
                    await SheetService.CloseAsync(this);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Close)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Close)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(Close)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanClose()
        {
            return true;
        }
    }
}
