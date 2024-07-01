// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Enums;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Questions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class EaconditionViewModel : AbstractQuestionViewModel<Eacondition, EaconditionEntry>
    {
        [ObservableProperty]
        private ObservableCollection<SelectableEaconditionEntry> _currentChoices = new ObservableCollection<SelectableEaconditionEntry>();

        [ObservableProperty]
        private ObservableCollection<SelectableEaconditionEntry> _selectedChoices = new ObservableCollection<SelectableEaconditionEntry>();

        public EaconditionViewModel(
            IAssessmentService service,
            ILocalAssessmentSessionRepository sessionRepository,
            IRawDataCacheRepository repository,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<EaconditionViewModel> logger)
            : base(service, sessionRepository,repository, dispatcherService, navigationService, dialogService, logger)
        {
        }

        public string SelectedChoicesCount
        {
            get
            {
                return string.Format(this["TxtAssesmentsEaConditionFav"], SelectedChoices.Count);
            }
        }

        private List<string> FilterIds { get; set; } = new List<string>();

        private DrillDownMode DrillDownMode { get; set; }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    if (DrillDownMode == DrillDownMode.Detail)
                    {
                        return;
                    }

                    //items = FilterIds.Count() == 0
                    //    ? Questions?.Where(x => x.BookletId == "21e800e3-14c8-49d3-a066-edc77dd8cbd7").ToList()
                    //    : Questions?.Where(x => FilterIds.Contains(x.ItemId)).ToList();

                    //items = items ?? new List<Eacondition>();

                    //CurrentChoices = new ObservableCollection<SelectableEaconditionEntry>(items.Select(i => CreateSelectableEntry(i)));
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

        protected override EaconditionEntry CreateEntry(Eacondition data)
        {
            return EaconditionEntry.Import(data, MediaBasePath, Density, ImageSizeConfig[typeof(EaconditionEntry)]);
        }

        private SelectableEaconditionEntry CreateSelectableEntry(Eacondition data)
        {
            return SelectableEaconditionEntry.Import(data, SelectionChangedHandler, MediaBasePath, Density, ImageSizeConfig[typeof(EaconditionEntry)]);
        }

        private void SelectionChangedHandler(SelectableEaconditionEntry entry)
        {
            if (entry.IsSelected)
            {
                SelectedChoices.Add(entry);
                if (SelectedChoices.Count > 3)
                {
                    var item = SelectedChoices[0];
                    item.IsSelected = false;
                    SelectedChoices.Remove(item);
                }
            }
            else
            {
                SelectedChoices.Remove(entry);
            }

            OnPropertyChanged(nameof(SelectedChoicesCount));
            if (DrillDownMode == DrillDownMode.Filtered)
            {
                NavigateCommand.Execute(null);
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == string.Empty)
            {
                OnPropertyChanged(nameof(SelectedChoicesCount));
            }
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            //if (query.ContainsKey(nameof(FilterIds)))
            //{
            //    FilterIds = query[nameof(FilterIds)]?.ToString()?.Split(";").ToList() ?? new List<string>();
            //}

            //DrillDownMode = query.ContainsKey(nameof(DrillDownMode))
            //    ? Enum.Parse<DrillDownMode>(query[nameof(DrillDownMode)]?.ToString() ?? nameof(DrillDownMode.Unknown))
            //    : DrillDownMode.Unknown;
        }

        protected async override Task Navigate(CancellationToken cancellationToken)
        {
            try
            {
                if (DrillDownMode == DrillDownMode.Detail)
                {
                    await Shell.Current.GoToAsync(new ShellNavigationState(".."), true);
                }
                else
                {
                    var parameter = new Dictionary<string, object>
                    {
                        { "Culture", Culture.Name },
                    };

                    var filterIds = SelectedChoices.SelectMany(x => x.Links.Select(l => l.id));
                    parameter.Add(nameof(FilterIds), string.Join(";", filterIds));
                    DrillDownMode nextDrillDownMode = DrillDownMode.Unknown;
                    switch (DrillDownMode)
                    {
                        case DrillDownMode.Unknown:
                            nextDrillDownMode = DrillDownMode.Filtered;
                            parameter.Add(nameof(DrillDownMode), nextDrillDownMode.ToString());
                            break;
                        case DrillDownMode.Filtered:
                            nextDrillDownMode = DrillDownMode.Detail;
                            parameter.Add(nameof(DrillDownMode), nextDrillDownMode.ToString());
                            var item = SelectedChoices[0].Export() as Eacondition;
                            if (item != null)
                            {
                                parameter.Add("Index", Questions?.ToList().IndexOf(item) ?? 0);
                            }

                            break;
                    }

                    await Shell.Current.GoToAsync(new ShellNavigationState($"//{typeof(Eacondition).Name}{nextDrillDownMode}"), true, parameter);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
