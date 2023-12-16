using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Editors;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class SearchFilterSheetViewModel : BaseViewModel
    {
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

        public SearchFilterSheetViewModel(
           IDispatcherService dispatcherService,
           INavigationService navigationService,
           IDialogService dialogService,
           ITrainingService trainingService,
           ILogger<SearchFilterSheetViewModel> logger)
           : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(trainingService);
            TrainingService = trainingService;
        }

        private ITrainingService TrainingService { get; }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sortValues = new List<OptionValue>();
                    var defaultSortValue = new OptionValue(1, Resources.Strings.Resources.FiltersSheet_RelevanceDescending, "Desc");
                    sortValues.Add(defaultSortValue);
                    sortValues.Add(new OptionValue(2, Resources.Strings.Resources.FiltersSheet_RelevanceAscending, "Asc"));

                    var editorList = new List<IPropertyEditor>()
                    {
                        PickerPropertyEditor.Import(new OptionValueList(_mappedId[SortingEditor], Resources.Strings.Resources.FiltersSheet_Sorting, defaultSortValue, defaultSortValue, sortValues)),
                        DateRangePropertyEditor.Import(
                            new DateRangeValue(
                                _mappedId[DateRangeEditor],
                                string.Empty,
                                new DateRange() { StartDate = DateTime.Now.AddDays(15), EndDate = DateTime.Now.AddYears(1) },
                                new DateRange() { StartDate = DateTime.Now.AddDays(15), EndDate = DateTime.Now.AddYears(1) })),
                        BooleanPropertyEditor.Import(new BoolenValue(_mappedId[IndividualStartDateBoolEditor], Resources.Strings.Resources.FiltersSheet_IndividualStartDate, true, true)),
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

        private void LoadonUIThread(IEnumerable<IPropertyEditor> editorList)
        {
            EditorList.Clear();
            foreach (var item in editorList)
            {
                EditorList.Add(item);
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanBack), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private Task Back(CancellationToken token)
        {
            DialogService.ClosePopup(this);
            return Task.CompletedTask;
        }

        private bool CanBack()
        {
            return true;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanReset), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private Task Reset(CancellationToken token)
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
            var dateRange = EditorList.OfType<DateRangePropertyEditor>().FirstOrDefault(x => x.Data.Id == _mappedId[DateRangeEditor])?.Value;
            var individualStartDateBool = EditorList.OfType<BooleanPropertyEditor>().FirstOrDefault(x => x.Data.Id == _mappedId[IndividualStartDateBoolEditor])?.Value;
            var sorting = EditorList.OfType<PickerPropertyEditor>().FirstOrDefault(x => x.Data.Id == _mappedId[SortingEditor])?.Value;
            return Task.CompletedTask;
        }

        private bool CanApplyFilter()
        {
            return !IsBusy;
        }
    }
}
