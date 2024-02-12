// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.PropertyEditor;
using De.HDBW.Apollo.Client.ViewModels.PropertyEditors;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class SearchFilterSheetViewModel : BaseViewModel
    {
        private Dictionary<Action, IEditor> _mapping = new Dictionary<Action, IEditor>();
        private const string SortingEditor = "Sorting";
        private const string DateRangeEditor = "DateRange";
        private const string IndividualStartDateBoolEditor = "IndividualStartDate";

        private readonly Dictionary<string, long> _mappedId = new Dictionary<string, long>()
        {
            { SortingEditor, 1 },
            { DateRangeEditor, 2 },
            { IndividualStartDateBoolEditor, 3 },
        };

        [ObservableProperty]
        private ObservableCollection<IPropertyEditor> _editorList = new ObservableCollection<IPropertyEditor>();
        private string? _currentFilter;
        private readonly Filter _filter = new Filter();

        public SearchFilterSheetViewModel(
           IDispatcherService dispatcherService,
           INavigationService navigationService,
           IDialogService dialogService,
           ISheetService sheetService,
           IEduProviderItemRepository eduProviderItemRepository,
           ILogger<SearchFilterSheetViewModel> logger)
           : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(sheetService);
            ArgumentNullException.ThrowIfNull(eduProviderItemRepository);
            SheetService = sheetService;
            EduProviderItemRepository = eduProviderItemRepository;
        }

        private ISheetService SheetService { get; }

        private IEduProviderItemRepository EduProviderItemRepository { get; }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var providers = await EduProviderItemRepository.GetItemsAsync(worker.Token).ConfigureAwait(false);
                    providers = providers ?? Array.Empty<EduProviderItem>();
                    var editableProperties = new List<string>();
                    editableProperties.Add(nameof(Training.TrainingType));
                    editableProperties.Add(nameof(Training.CourseProvider));
                    editableProperties.Add(nameof(Training.CourseProvider));
                    editableProperties.Add(nameof(Training.Price));

                    var courseTypes = new List<PickerValue>()
                    {
                        new PickerValue(Resources.Strings.Resources.CourseType_All, CourseType.All),
                        new PickerValue(Resources.Strings.Resources.CourseType_OnAndOffline, CourseType.OnAndOffline),
                        new PickerValue(Resources.Strings.Resources.CourseType_Online, CourseType.Online),
                        new PickerValue(Resources.Strings.Resources.CourseType_InHouse, CourseType.InHouse),
                        new PickerValue(Resources.Strings.Resources.CourseType_InPerson, CourseType.InPerson),
                    };

                    var eduProviders = providers.Select(p => new PickerValue(p.Name, p.Name)).ToList();

                    var editorList = new List<IPropertyEditor>()
                    {
                        PickerPropertyEditor.Import(Resources.Strings.Resources.Filter_CourseType, courseTypes, courseTypes.First()),
                        ComboboxPropertyEditor.Import(Resources.Strings.Resources.Filter_CourseType, courseTypes, courseTypes.First()),
                        DatePropertyEditor.Import(Resources.Strings.Resources.Filter_CourseType, new DateTimeValue(null)),
                        PickerPropertyEditor.Import(Resources.Strings.Resources.Filter_CourseType, courseTypes, courseTypes.First()),
                        RangePropertyEditor.Import(Resources.Strings.Resources.Filter_Price, new DoubleValue(100d), 0d, 2000d),
                        ListPropertyEditor.Import(Resources.Strings.Resources.Filter_EduProviders, eduProviders),
                    };

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(editorList), worker.Token);
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

        protected override void RefreshCommands()
        {
            ApplyFilterCommand?.NotifyCanExecuteChanged();
            ResetFilterCommand?.NotifyCanExecuteChanged();
            CloseCommand?.NotifyCanExecuteChanged();
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            _currentFilter = navigationParameters.GetValue<string>(NavigationParameter.Data);
        }

        private void LoadonUIThread(IEnumerable<IPropertyEditor> editorList)
        {
            EditorList.Clear();
            foreach (var item in editorList)
            {
                EditorList.Add(item);
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanClose), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private async Task Close(CancellationToken token)
        {
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

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanReset), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private Task ResetFilter(CancellationToken token)
        {
            return Task.CompletedTask;
        }

        private bool CanReset()
        {
            return true;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanApplyFilter), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private Task ApplyFilter(CancellationToken token)
        {
            return Task.CompletedTask;
        }

        private bool CanApplyFilter()
        {
            return !IsBusy;
        }
    }
}
