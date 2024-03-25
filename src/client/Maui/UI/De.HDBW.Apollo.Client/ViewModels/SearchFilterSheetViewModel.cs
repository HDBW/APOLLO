// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.PropertyEditor;
using De.HDBW.Apollo.Client.ViewModels.PropertyEditors;
using De.HDBW.Apollo.Data.Helper;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;
using TrainingModel = Invite.Apollo.App.Graph.Common.Models.Trainings.Training;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class SearchFilterSheetViewModel : BaseViewModel
    {
        private readonly List<(IPropertyEditor Editor, Action<IPropertyEditor, Filter> LoadAction, Action<IPropertyEditor, Filter> SaveAction)> _configuration = new List<(IPropertyEditor Editor, Action<IPropertyEditor, Filter> LoadAction, Action<IPropertyEditor, Filter> SaveAction)>();

        private string? _currentFilter;
        private decimal? _maxPrice;

        [ObservableProperty]
        private ObservableCollection<IPropertyEditor> _editorList = new ObservableCollection<IPropertyEditor>();

        private Filter _filter = new Filter();

        public SearchFilterSheetViewModel(
           IDispatcherService dispatcherService,
           INavigationService navigationService,
           IDialogService dialogService,
           ISheetService sheetService,
           ILogger<SearchFilterSheetViewModel> logger)
           : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(sheetService);
            SheetService = sheetService;
        }

        private ISheetService SheetService { get; }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    _filter = new Filter();
                    if (!string.IsNullOrWhiteSpace(_currentFilter))
                    {
                        try
                        {
                            _filter = _currentFilter.Deserialize<Filter>() ?? new Filter();
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, "Unknown error while loading existing filter.");
                        }
                    }

                    var trainingModes = new List<PickerValue>
                    {
                        new PickerValue(Resources.Strings.Resources.TrainingMode_Offline, TrainingMode.Offline),
                        new PickerValue(Resources.Strings.Resources.TrainingMode_Online, TrainingMode.Online),
                        new PickerValue(Resources.Strings.Resources.TrainingMode_Hybrid, TrainingMode.Hybrid),
                        new PickerValue(Resources.Strings.Resources.TrainingMode_OnDemand, TrainingMode.OnDemand),
                    };

                    var trainingTimeModels = new List<PickerValue>
                    {
                        new PickerValue(Resources.Strings.Resources.TimeModel_Block, TrainingTimeModel.Block),
                        new PickerValue(Resources.Strings.Resources.TimeModel_Parttime, TrainingTimeModel.Parttime),
                        new PickerValue(Resources.Strings.Resources.TimeModel_Fulltime, TrainingTimeModel.Fulltime),
                    };

                    IPropertyEditor editor = BooleanPropertyEditor.Import(Resources.Strings.Resources.Filter_Loans, new BooleanValue(null), (e) =>
                    {
                        var field = _filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.LoansFieldName);
                        if (field != null)
                        {
                            _filter.Fields.Remove(field);
                        }

                        _configuration.FirstOrDefault(x => x.Editor == e).LoadAction(e, _filter);
                    });

                    _configuration.Add((editor, LoadLoansFilter, SaveLoansFilter));

                    editor = BooleanPropertyEditor.Import(Resources.Strings.Resources.Filter_IsAccessible, new BooleanValue(null), (e) =>
                    {
                        var field = _filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.AccessibilityAvailableFieldName);
                        if (field != null)
                        {
                            _filter.Fields.Remove(field);
                        }

                        _configuration.FirstOrDefault(x => x.Editor == e).LoadAction(e, _filter);
                    });

                    _configuration.Add((editor, LoadAccessibilityAvailableFilter, SaveAccessibilityAvailableFilter));

                    editor = BooleanPropertyEditor.Import(Resources.Strings.Resources.Filter_HasIndividualStartDate, new BooleanValue(null), (e) =>
                    {
                        var field = _filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.IndividualStartDateFieldName);
                        if (field != null)
                        {
                            _filter.Fields.Remove(field);
                        }

                        _configuration.FirstOrDefault(x => x.Editor == e).LoadAction(e, _filter);
                    });

                    _configuration.Add((editor, LoadIndividualStartDateFilter, SaveIndividualStartDateFilter));

                    editor = DateRangePropertyEditor.Import(Resources.Strings.Resources.Filter_Date, new DateRangeValue((null, null)), (e) =>
                    {
                        var field = _filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.AppointmenStartDateFieldName);
                        if (field != null)
                        {
                            _filter.Fields.Remove(field);
                        }

                        field = _filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.AppointmenEndDateFieldName);
                        if (field != null)
                        {
                            _filter.Fields.Remove(field);
                        }

                        field = _filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.OccurenceStartDateFieldName);
                        if (field != null)
                        {
                            _filter.Fields.Remove(field);
                        }

                        field = _filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.OccurenceEndDateFieldName);
                        if (field != null)
                        {
                            _filter.Fields.Remove(field);
                        }

                        _configuration.FirstOrDefault(x => x.Editor == e).LoadAction(e, _filter);
                    });

                    _configuration.Add((editor, LoadDateFilter, SaveDateFilter));

                    if (_maxPrice > 1)
                    {
                        editor = DecimalRangePropertyEditor.Import(Resources.Strings.Resources.Filter_Price, new DecimalRangeValue((0, _maxPrice.Value)), _maxPrice.Value, (e) =>
                        {
                            var field = _filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.PriceFieldName);
                            if (field != null)
                            {
                                _filter.Fields.Remove(field);
                            }

                            _configuration.FirstOrDefault(x => x.Editor == e).LoadAction(e, _filter);
                        });

                        _configuration.Add((editor, LoadPriceFilter, SavePriceFilter));
                    }

                    editor = ListPropertyEditor.Import(Resources.Strings.Resources.Filter_TrainingsMode, trainingModes, (e) =>
                    {
                        var field = _filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.TrainingsModeFieldName);
                        if (field != null)
                        {
                            _filter.Fields.Remove(field);
                        }

                        field = _filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.AppointmenTrainingsModeFieldName);
                        if (field != null)
                        {
                            _filter.Fields.Remove(field);
                        }

                        _configuration.FirstOrDefault(x => x.Editor == e).LoadAction(e, _filter);
                    });

                    _configuration.Add((editor, LoadTrainingsModeFilter, SaveTrainingModeFilter));

                    editor = ListPropertyEditor.Import(Resources.Strings.Resources.Filter_TrainingTimeModel, trainingTimeModels, (e) =>
                    {
                        var field = _filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.AppointmenTrainingsTimeModelFieldName);
                        if (field != null)
                        {
                            _filter.Fields.Remove(field);
                        }

                        _configuration.FirstOrDefault(x => x.Editor == e).LoadAction(e, _filter);
                    });

                    _configuration.Add((editor, LoadTrainingsTimeFilter, SaveTrainingTimeFilter));

                    var editorList = _configuration.Select(c => c.Editor).ToList();
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
            _maxPrice = navigationParameters.GetValue<decimal>(NavigationParameter.SavedState);
        }

        private void LoadonUIThread(IEnumerable<IPropertyEditor> editorList)
        {
            EditorList.Clear();
            foreach (var item in editorList)
            {
                var config = _configuration.First(x => x.Editor == item);
                config.LoadAction(item, _filter);
                EditorList.Add(item);
            }
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

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanReset), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private async Task ResetFilter(CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(ResetFilterCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork())
            {
                try
                {
                    foreach (var editor in EditorList)
                    {
                        editor.ClearCommand?.Execute(null);
                    }

                    var filter = new Filter();
                    ApplyFilterExpression(filter);
                    await SheetService.CloseAsync(this);
                    WeakReferenceMessenger.Default.Send(new FilterChangedMessage(filter));
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ResetFilter)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ResetFilter)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(ResetFilter)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanReset()
        {
            return true;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanApplyFilter), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private async Task ApplyFilter(CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(ApplyFilterCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork())
            {
                try
                {
                    var filter = new Filter();
                    ApplyFilterExpression(filter);
                    await SheetService.CloseAsync(this);
                    WeakReferenceMessenger.Default.Send(new FilterChangedMessage(filter));
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ApplyFilter)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ApplyFilter)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(ApplyFilter)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private void ApplyFilterExpression(Filter filter)
        {
            foreach (var config in _configuration)
            {
                config.SaveAction.Invoke(config.Editor, filter);
            }
        }

        private bool CanApplyFilter()
        {
            return !IsBusy;
        }

        private void SaveLoansFilter(IPropertyEditor editor, Filter filter)
        {
            var booleanEditor = editor as BooleanPropertyEditor;
            if (booleanEditor == null)
            {
                return;
            }

            editor.Save();

            var value = booleanEditor.Data as BooleanValue;
            if (!(value?.Value ?? false))
            {
                return;
            }

            var queryOperation = QueryOperator.Empty;
            var arguments = new List<object> { value?.Value.Value! };
            filter.Fields.Add(new FieldExpression()
            {
                FieldName = KnownFilters.LoansFieldName,
                Operator = queryOperation,
                Argument = arguments,
            });
        }

        private void LoadLoansFilter(IPropertyEditor editor, Filter filter)
        {
            var field = filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.LoansFieldName);
            var booleanEditor = editor as BooleanPropertyEditor;
            if (booleanEditor == null)
            {
                return;
            }

            bool? isActive = null;
            if (field != null)
            {
                var value = field.Argument?.OfType<JsonElement>().FirstOrDefault();
                isActive = value?.GetBoolean();
            }

            booleanEditor.Update(new BooleanValue(isActive), isActive != null);
        }

        private void SaveAccessibilityAvailableFilter(IPropertyEditor editor, Filter filter)
        {
            var booleanEditor = editor as BooleanPropertyEditor;
            if (booleanEditor == null)
            {
                return;
            }

            editor.Save();

            var value = booleanEditor.Data as BooleanValue;
            if (value?.Value == null)
            {
                return;
            }

            var queryOperation = QueryOperator.Empty;
            var arguments = new List<object> { value?.Value.Value! };
            filter.Fields.Add(new FieldExpression()
            {
                FieldName = KnownFilters.AccessibilityAvailableFieldName,
                Operator = queryOperation,
                Argument = arguments,
            });
        }

        private void LoadAccessibilityAvailableFilter(IPropertyEditor editor, Filter filter)
        {
            var field = filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.AccessibilityAvailableFieldName);
            var booleanEditor = editor as BooleanPropertyEditor;
            if (booleanEditor == null)
            {
                return;
            }

            bool? isActive = null;
            if (field != null)
            {
                var value = field.Argument?.OfType<JsonElement>().FirstOrDefault();
                isActive = value?.GetBoolean();
            }

            booleanEditor.Update(new BooleanValue(isActive), isActive != null);
        }

        private void LoadIndividualStartDateFilter(IPropertyEditor editor, Filter filter)
        {
            var field = filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.IndividualStartDateFieldName);
            var booleanEditor = editor as BooleanPropertyEditor;
            if (booleanEditor == null)
            {
                return;
            }

            bool? isActive = null;
            if (field != null)
            {
                var value = field.Argument?.OfType<JsonElement>().FirstOrDefault();
                isActive = value?.GetBoolean();
            }

            booleanEditor.Update(new BooleanValue(isActive), isActive != null);
        }

        private void SaveIndividualStartDateFilter(IPropertyEditor editor, Filter filter)
        {
            var booleanEditor = editor as BooleanPropertyEditor;
            if (booleanEditor == null)
            {
                return;
            }

            editor.Save();

            var value = booleanEditor.Data as BooleanValue;
            if (value?.Value == null)
            {
                return;
            }

            var queryOperation = QueryOperator.Empty;
            var arguments = new List<object> { value?.Value.Value! };
            filter.Fields.Add(new FieldExpression()
            {
                FieldName = KnownFilters.IndividualStartDateFieldName,
                Operator = queryOperation,
                Argument = arguments,
            });
        }

        private void SavePriceFilter(IPropertyEditor editor, Filter filter)
        {
            var rangeEditor = editor as DecimalRangePropertyEditor;
            if (rangeEditor == null || _maxPrice == null)
            {
                return;
            }

            editor.Save();

            var value = rangeEditor.Data as DecimalRangeValue;
            if (value?.Value == null || !value.WasChanged)
            {
                return;
            }

            var queryOperation = QueryOperator.In;
            var arguments = new List<object> { value.Value.Start, value.Value.End };

            filter.Fields.Add(new FieldExpression()
            {
                FieldName = KnownFilters.PriceFieldName,
                Operator = queryOperation,
                Argument = arguments,
            });
        }

        private void LoadPriceFilter(IPropertyEditor editor, Filter filter)
        {
            var field = filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.PriceFieldName);
            var rangeEditor = editor as DecimalRangePropertyEditor;
            if (rangeEditor == null)
            {
                return;
            }

            decimal? start = null;
            decimal? end = null;
            if (field != null)
            {
                var elements = field.Argument?.OfType<JsonElement>();
                var element = elements?.FirstOrDefault();
                start = element?.GetDecimal();
                element = elements?.LastOrDefault();
                end = element?.GetDecimal();
            }

            rangeEditor.Update(new DecimalRangeValue((start ?? 0, end ?? rangeEditor.RangeValue)), field != null);
        }

        private void SaveTrainingModeFilter(IPropertyEditor editor, Filter filter)
        {
            var listEditor = editor as ListPropertyEditor;
            if (listEditor == null)
            {
                return;
            }

            editor.Save();

            var selectedValues = listEditor.Values.Where(x => x.IsSelected).Select(x => x.Data! as PickerValue).Select(x => (TrainingMode)x!.Data!).OfType<object>().ToList();
            if (!selectedValues.Any())
            {
                return;
            }

            var queryOperation = QueryOperator.Contains;
            var arguments = selectedValues;
            filter.Fields.Add(new FieldExpression()
            {
                FieldName = KnownFilters.TrainingsModeFieldName,
                Operator = queryOperation,
                Argument = arguments,
            });
            filter.Fields.Add(new FieldExpression()
            {
                FieldName = KnownFilters.AppointmenTrainingsModeFieldName,
                Operator = queryOperation,
                Argument = arguments,
            });
        }

        private void LoadTrainingsModeFilter(IPropertyEditor editor, Filter filter)
        {
            var field = filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.TrainingsModeFieldName);
            var listEditor = editor as ListPropertyEditor;
            if (listEditor == null)
            {
                return;
            }

            var flags = field?.Argument?.OfType<JsonElement>().Select(x => (TrainingMode)x.GetInt32()).ToList() ?? new List<TrainingMode>();

            var trainingModes = new Dictionary<PickerValue, bool>()
            {
                { new PickerValue(Resources.Strings.Resources.TrainingMode_Offline, TrainingMode.Offline), flags.Contains(TrainingMode.Offline) },
                { new PickerValue(Resources.Strings.Resources.TrainingMode_Online, TrainingMode.Online), flags.Contains(TrainingMode.Online) },
                { new PickerValue(Resources.Strings.Resources.TrainingMode_Hybrid, TrainingMode.Hybrid), flags.Contains(TrainingMode.Hybrid) },
                { new PickerValue(Resources.Strings.Resources.TrainingMode_OnDemand, TrainingMode.OnDemand), flags.Contains(TrainingMode.OnDemand) },
            };

            listEditor.Update(trainingModes, field != null);
        }

        private void SaveTrainingTimeFilter(IPropertyEditor editor, Filter filter)
        {
            var listEditor = editor as ListPropertyEditor;
            if (listEditor == null)
            {
                return;
            }

            editor.Save();

            var selectedValues = listEditor.Values.Where(x => x.IsSelected).Select(x => x.Data! as PickerValue).Select(x => (TrainingTimeModel)x!.Data!).OfType<object>().ToList();
            if (!selectedValues.Any())
            {
                return;
            }

            var queryOperation = QueryOperator.Equals;
            var arguments = selectedValues;
            filter.Fields.Add(new FieldExpression()
            {
                FieldName = KnownFilters.AppointmenTrainingsTimeModelFieldName,
                Operator = queryOperation,
                Argument = arguments,
            });
        }

        private void LoadTrainingsTimeFilter(IPropertyEditor editor, Filter filter)
        {
            var field = filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.AppointmenTrainingsTimeModelFieldName);
            var listEditor = editor as ListPropertyEditor;
            if (listEditor == null)
            {
                return;
            }

            var flags = field?.Argument?.OfType<JsonElement>().Select(x => (TrainingTimeModel)x.GetInt32()).ToList() ?? new List<TrainingTimeModel>();

            var trainingTimeModels = new Dictionary<PickerValue, bool>()
            {
                { new PickerValue(Resources.Strings.Resources.TimeModel_Block, TrainingTimeModel.Block), flags.Contains(TrainingTimeModel.Block) },
                { new PickerValue(Resources.Strings.Resources.TimeModel_Parttime, TrainingTimeModel.Parttime), flags.Contains(TrainingTimeModel.Parttime) },
                { new PickerValue(Resources.Strings.Resources.TimeModel_Fulltime, TrainingTimeModel.Fulltime), flags.Contains(TrainingTimeModel.Fulltime) },
            };

            listEditor.Update(trainingTimeModels, field != null);
        }

        private void SaveDateFilter(IPropertyEditor editor, Filter filter)
        {
            var rangeEditor = editor as DateRangePropertyEditor;
            if (rangeEditor == null)
            {
                return;
            }

            editor.Save();

            var value = rangeEditor.Data as DateRangeValue;
            if (value?.Value == null || (!value.Value.Start.HasValue && !value.Value.End.HasValue))
            {
                return;
            }

            if (value.Value.Start.HasValue)
            {
                var queryOperation = QueryOperator.GreaterThanEqualTo;
                var arguments = new List<object> { value.Value.Start.Value.ToDTODate() };

                filter.Fields.Add(new FieldExpression()
                {
                    FieldName = KnownFilters.AppointmenStartDateFieldName,
                    Operator = queryOperation,
                    Argument = arguments,
                });

                filter.Fields.Add(new FieldExpression()
                {
                    FieldName = KnownFilters.OccurenceStartDateFieldName,
                    Operator = queryOperation,
                    Argument = arguments,
                });
            }

            if (value.Value.End.HasValue)
            {
                var queryOperation = QueryOperator.LessThanEqualTo;
                var arguments = new List<object> { value.Value.End.Value.ToDTODate() };

                filter.Fields.Add(new FieldExpression()
                {
                    FieldName = KnownFilters.AppointmenEndDateFieldName,
                    Operator = queryOperation,
                    Argument = arguments,
                });

                filter.Fields.Add(new FieldExpression()
                {
                    FieldName = KnownFilters.OccurenceEndDateFieldName,
                    Operator = queryOperation,
                    Argument = arguments,
                });
            }
        }

        private void LoadDateFilter(IPropertyEditor editor, Filter filter)
        {
            var startField = filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.AppointmenStartDateFieldName);
            var endField = filter.Fields.FirstOrDefault(x => x.FieldName == KnownFilters.AppointmenEndDateFieldName);
            var rangeEditor = editor as DateRangePropertyEditor;
            if (rangeEditor == null)
            {
                return;
            }

            DateTime? start = null;
            DateTime? end = null;
            if (startField != null)
            {
                var elements = startField.Argument?.OfType<JsonElement>();
                var element = elements?.FirstOrDefault();
                start = element?.GetDateTime().ToUIDate();
            }

            if (endField != null)
            {
                var elements = endField.Argument?.OfType<JsonElement>();
                var element = elements?.FirstOrDefault();
                end = element?.GetDateTime().ToUIDate();
            }

            rangeEditor.Update(new DateRangeValue((start, end)), startField != null || endField != null);
        }
    }
}
