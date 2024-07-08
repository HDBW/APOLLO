// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessments;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    //await Shell.Current.GoToAsync(new ShellNavigationState(".."), true);
    public partial class ResultOverViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ObservableObject> _sections = new ObservableCollection<ObservableObject>();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FlowDirection))]
        private CultureInfo _culture = new CultureInfo("de-DE");
        private string? _moduleId;
        private string? _assessmentId;
        private string? _sessionId;
        private string? _language;

        public ResultOverViewModel(
            IAssessmentService assessmentService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ResultOverViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(assessmentService);
            AssessmentService = assessmentService;
        }

        public FlowDirection FlowDirection
        {
            get { return Culture?.TextInfo.IsRightToLeft ?? false ? FlowDirection.RightToLeft : FlowDirection.LeftToRight; }
        }

        private IAssessmentService AssessmentService { get; }

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
                    Module? module = null;
                    if (!string.IsNullOrWhiteSpace(_moduleId))
                    {
                        module = await AssessmentService.GetModuleAsync(_moduleId, _language, worker.Token).ConfigureAwait(false);
                    }

                    _assessmentId = module?.AssessmentId;
                    _sessionId = module?.SessionId;
                    if (module != null)
                    {
                        switch (module.Type)
                        {
                            case AssessmentType.Sk:
                                break;
                            case AssessmentType.Ea:
                                break;
                            case AssessmentType.So:
                                break;
                            case AssessmentType.Gl:
                                break;
                            case AssessmentType.Be:
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
            _moduleId = navigationParameters.GetValue<string?>(NavigationParameter.Id);
            _language = navigationParameters.GetValue<string?>(NavigationParameter.Language) ?? "de-DE";

            Culture = new CultureInfo(_language);
            OnPropertyChanged(string.Empty);
        }

        private void LoadonUIThread(
            List<ObservableObject> sections)
        {
            Sections = new ObservableCollection<ObservableObject>(sections);
        }
    }
}
