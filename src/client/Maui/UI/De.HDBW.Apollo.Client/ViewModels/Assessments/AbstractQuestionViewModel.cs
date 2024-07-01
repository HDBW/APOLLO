// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Questions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public abstract partial class AbstractQuestionViewModel<TU, TV> : BaseViewModel
        where TU : AbstractQuestion
        where TV : AbstractQuestionEntry
    {
        [ObservableProperty]
        private TV? _question;

        [ObservableProperty]
        private IReadOnlyCollection<TU>? _questions;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FlowDirection))]
        private CultureInfo _culture = new CultureInfo("de-DE");

        protected AbstractQuestionViewModel(
            IAssessmentService service,
            IRawDataCacheRepository rawDataCache,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(rawDataCache);
            Service = service;
            RawDataCache = rawDataCache;
        }

        public FlowDirection FlowDirection
        {
            get { return Culture?.TextInfo.IsRightToLeft ?? false ? FlowDirection.RightToLeft : FlowDirection.LeftToRight; }
        }

        protected IAssessmentService Service { get; }

        protected IRawDataCacheRepository RawDataCache { get; }

        protected int Offset { get; set; }

        protected string MediaBasePath { get; } = "https://asset.invite-apollo.app/assets/resized/";

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
        public string this[string key]
        {
            get
            {
                var localizedResource = Resources.Strings.Resources.ResourceManager.GetString($"{key}_{Culture.Name}");
                return localizedResource ?? Resources.Strings.Resources.ResourceManager.GetString(key) ?? string.Empty;
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            ParseQuery(query);
            await LoadDataAsync();
        }

        protected virtual void ParseQuery(IDictionary<string, object> query)
        {
            if (query.ContainsKey("Index"))
            {
                Offset = (int)query["Index"];
            }

            string culture = "de-DE";
            if (query.ContainsKey(nameof(Culture)))
            {
                culture = (string)query[nameof(Culture)];
            }

            Culture = new CultureInfo(culture);
            OnPropertyChanged(string.Empty);
        }

        protected virtual async Task LoadDataAsync()
        {
            Questions = new List<TU>(); // await Service.GetQuestionsByTypeAsync<TU>(Culture, CancellationToken.None);
            Question = CreateEntry(Questions.ToList()[Offset]);
        }

        protected abstract TV CreateEntry(TU data);

        [RelayCommand(AllowConcurrentExecutions = false)]
        protected virtual async Task Navigate(CancellationToken cancellationToken)
        {
            try
            {
                if (Offset == (Questions?.Count - 1 ?? Offset))
                {
                    await Shell.Current.GoToAsync(new ShellNavigationState(".."), true);
                }
                else
                {
                    var parameter = new Dictionary<string, object>
                    {
                        { "Index", Offset + 1 },
                        { "Culture", Culture.Name },
                    };

                    await Shell.Current.GoToAsync(new ShellNavigationState($"//{typeof(TU).Name}Filtered"), true, parameter);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
