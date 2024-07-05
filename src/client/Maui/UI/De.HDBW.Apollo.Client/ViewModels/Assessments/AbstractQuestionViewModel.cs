// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Questions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public abstract partial class AbstractQuestionViewModel<TU, TV> : BaseViewModel
        where TU : AbstractQuestion
        where TV : IAbstractQuestionEntry
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Title))]
        private int _offset;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Title))]
        private int _count;

        [ObservableProperty]
        private TV? _question;

        [ObservableProperty]
        private ObservableCollection<TU>? _questions;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FlowDirection))]
        private CultureInfo _culture = new CultureInfo("de-DE");

        private Action<bool>? _updateStateHandler;

        protected AbstractQuestionViewModel(
            IAssessmentService service,
            IRawDataCacheRepository rawDataCacheRepository,
            IUserSecretsService userSecretsService,
            IAudioPlayerService audioPlayerService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(rawDataCacheRepository);
            ArgumentNullException.ThrowIfNull(userSecretsService);
            ArgumentNullException.ThrowIfNull(audioPlayerService);

            Service = service;
            RawDataCacheRepository = rawDataCacheRepository;
            MediaBasePath = userSecretsService["MediaAssetStorageURL"] ?? string.Empty;
            AudioPlayerService = audioPlayerService;
        }

        public FlowDirection FlowDirection
        {
            get { return Culture?.TextInfo.IsRightToLeft ?? false ? FlowDirection.RightToLeft : FlowDirection.LeftToRight; }
        }

        public virtual string? Title
        {
            get
            {
                if (Culture == null)
                {
                    return string.Empty;
                }

                var formate = this["TxtAssesmentsTitleQuestions"];
                if (string.IsNullOrWhiteSpace(formate) || Count == 0)
                {
                    return string.Empty;
                }

                return string.Format(formate, Offset + 1, Count);
            }
        }

        protected string? ModuleId { get; set; }

        protected string? DessionId { get; set; }

        protected string? Language { get; set; }

        protected string? SessionId { get; set; }

        protected LocalAssessmentSession? Session { get; set; }

        protected IAssessmentService Service { get; }

        protected List<string>? RawDataIds { get; set; }

        protected IRawDataCacheRepository RawDataCacheRepository { get; }

        protected string MediaBasePath { get; } = string.Empty;

        protected IAudioPlayerService AudioPlayerService { get; }

        protected int Density { get; } = int.Max(1, int.Min((int)DeviceDisplay.MainDisplayInfo.Density, 4));

        protected Dictionary<Type, Dictionary<string, int>> ImageSizeConfig { get; } = new Dictionary<Type, Dictionary<string, int>>()
        {
            {
                typeof(ChoiceEntry),
                new Dictionary<string, int>()
                {
                    { nameof(ChoiceEntry.AnswerImages), 132 },
                    { nameof(ChoiceEntry.QuestionImages), 272 },
                }
            },
            {
                typeof(EaconditionEntry),
                new Dictionary<string, int>()
                {
                    { nameof(EaconditionEntry.Images), 204 },
                }
            },
            {
                typeof(EafrequencyEntry),
                new Dictionary<string, int>()
                {
                    { nameof(EafrequencyEntry.Images), 272 },
                }
            },
            {
                typeof(ImagemapEntry),
                new Dictionary<string, int>()
                {
                    { nameof(ImagemapEntry.Image), 272 },
                }
            },
            {
                typeof(AssociateEntry),
                new Dictionary<string, int>()
                {
                    { nameof(AssociateEntry.TargetImages), 132 },
                }
            },
        };

        [IndexerName("Item")]
        public new string this[string key]
        {
            get
            {
                var localizedResource = Resources.Strings.Resources.ResourceManager.GetString($"{key}_{Culture.Name}");
                return localizedResource ?? Resources.Strings.Resources.ResourceManager.GetString(key) ?? string.Empty;
            }
        }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(SessionId))
                    {
                        Logger.LogError($"Session not present in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    Session = await Service.GetSessionAsync(SessionId, Language, worker.Token).ConfigureAwait(false);
                    if (Session == null)
                    {
                        Logger.LogError($"Session not found in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(Session.RawDataOrder) ||
                        string.IsNullOrWhiteSpace(Session.CurrentRawDataId) ||
                        string.IsNullOrWhiteSpace(Session.ModuleId))
                    {
                        Logger.LogError($"Session not valid in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    var rawdataId = Session.CurrentRawDataId;
                    RawDataIds = Session.RawDataOrder.Split(";").ToList();
                    var offset = RawDataIds.IndexOf(rawdataId);
                    var count = RawDataIds.Count;
                    var questions = new List<TU>();
                    var cachedData = await RawDataCacheRepository.GetItemAsync(SessionId, rawdataId, worker.Token).ConfigureAwait(false);
                    if (cachedData == null ||
                        string.IsNullOrWhiteSpace(cachedData.RawDataId) ||
                        string.IsNullOrWhiteSpace(cachedData.ModuleId) ||
                        string.IsNullOrWhiteSpace(cachedData.AssesmentId))
                    {
                        Logger.LogError($"No cached rawdata found in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    var rawData = cachedData.ToRawData();
                    var question = CreateEntry(QuestionFactory.Create<TU>(rawData!, cachedData.RawDataId, cachedData.ModuleId, cachedData.AssesmentId, Culture));
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(questions, question, offset, count), worker.Token);
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

        public override Task OnNavigatingFromAsync()
        {
            AudioPlayerService.Stop(HandlePlaybackCompleted);
            _updateStateHandler = null;
            return base.OnNavigatingFromAsync();
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            ModuleId = navigationParameters.GetValue<string>(NavigationParameter.Id);
            SessionId = navigationParameters.GetValue<string>(NavigationParameter.Data);
            Language = navigationParameters.GetValue<string>(NavigationParameter.Language) ?? "de-DE";
            Culture = new CultureInfo(Language);
            OnPropertyChanged(string.Empty);
        }

        protected abstract TV CreateEntry(TU data);

        [RelayCommand(AllowConcurrentExecutions = false)]
        protected virtual async Task Navigate(CancellationToken cancellationToken)
        {
            using (var worker = ScheduleWork(cancellationToken))
            {
                try
                {
                    if (Offset == (Count - 1))
                    {
                        await Shell.Current.GoToAsync(new ShellNavigationState(".."), true);
                    }
                    else
                    {
                        if (Session?.SessionId == null || Session.CurrentRawDataId == null)
                        {
                            return;
                        }

                        var score = Question?.GetScore() ?? 0d;
                        var cachedData = await Service.AnswerAsync(Session.SessionId, Session.CurrentRawDataId, 0d, worker.Token).ConfigureAwait(false);
                        if (cachedData == null)
                        {
                            throw new NotSupportedException($"Unable to get next question while {nameof(Navigate)} in {GetType().Name}.");
                        }

                        var rawData = cachedData.ToRawData();
                        string? route = rawData?.type.ToRoute();
                        if (string.IsNullOrWhiteSpace(route))
                        {
                            return;
                        }

                        var stack = Shell.Current.Navigation.NavigationStack.ToArray();
                        await ExecuteOnUIThreadAsync(
                        async () =>
                        {
                            Shell.Current.Navigation.RemovePage(stack.Last());
                            var parameters = new NavigationParameters();
                            parameters.AddValue(NavigationParameter.Id, ModuleId);
                            parameters.AddValue(NavigationParameter.Data, SessionId);
                            parameters.AddValue(NavigationParameter.Language, Language);
                            await NavigationService.NavigateAsync(route, worker.Token, parameters);
                        }, worker.Token);
                    }
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

        protected async void OnZoomImageAsync(ZoomableImageEntry entry)
        {
            if (IsBusy)
            {
                return;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    var parameters = new NavigationParameters();
                    parameters.AddValue(NavigationParameter.Data, entry.AbsolutePath);
                    await DialogService.ShowPopupAsync<ImageZoomDialog, NavigationParameters, NavigationParameters>(parameters, worker.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnZoomImageAsync)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnZoomImageAsync)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(OnZoomImageAsync)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        protected async Task<bool> OnToggleAudioPlaybackAsync(AudioEntry entry, Action<bool> updateStateHandler)
        {
            var result = false;
            if (IsBusy)
            {
                return result;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    if (!AudioPlayerService.IsPlaying && !AudioPlayerService.IsPaused)
                    {
                        _updateStateHandler = updateStateHandler;
                        result = await AudioPlayerService.StartAsync(entry.AbsolutePath, HandlePlaybackCompleted, worker.Token).ConfigureAwait(false);
                    }
                    else if (AudioPlayerService.IsPlaying)
                    {
                        AudioPlayerService.Pause();
                        result = AudioPlayerService.IsPlaying;
                    }
                    else if (AudioPlayerService.IsPaused)
                    {
                        AudioPlayerService.Resume();
                        result = AudioPlayerService.IsPlaying;
                    }
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(OnToggleAudioPlaybackAsync)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }

                return result;
            }
        }

        protected async Task<bool> OnRestartAudioAsync(AudioEntry entry, Action<bool> updateStateHandler)
        {
            try
            {
                AudioPlayerService.Stop(HandlePlaybackCompleted);
                _updateStateHandler?.Invoke(AudioPlayerService.IsPlaying);
                _updateStateHandler = null;
                return await OnToggleAudioPlaybackAsync(entry, updateStateHandler);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(OnToggleAudioPlaybackAsync)} in {GetType().Name}.");
            }

            return false;
        }

        private void HandlePlaybackCompleted(object? sender, EventArgs e)
        {
            var player = sender as IAudioPlayer;
            if (player == null)
            {
                return;
            }

            // We need to decouple her
            Task.Run(UpdateStateAsync);
        }

        private async Task UpdateStateAsync()
        {
            await Task.Delay(200);
            await ExecuteOnUIThreadAsync(
                () =>
                {
                    AudioPlayerService.Stop(HandlePlaybackCompleted);
                    _updateStateHandler?.Invoke(AudioPlayerService.IsPlaying);
                    _updateStateHandler = null;
                }, CancellationToken.None);
        }

        private void LoadonUIThread(List<TU> questions, TV? question, int offset, int count)
        {
            Offset = offset;
            Count = count;
            Questions = new ObservableCollection<TU>(questions);
            Question = question;
            OnPropertyChanged(nameof(Title));
        }
    }
}
