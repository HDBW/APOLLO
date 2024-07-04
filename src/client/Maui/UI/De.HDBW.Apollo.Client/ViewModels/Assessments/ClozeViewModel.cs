// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Questions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class ClozeViewModel : AbstractQuestionViewModel<Cloze, ClozeEntry>
    {
        [ObservableProperty]
        private List<string> _ids = new List<string>();

        public ClozeViewModel(
            IAssessmentService service,
            ILocalAssessmentSessionRepository sessionRepository,
            IRawDataCacheRepository repository,
            IUserSecretsService userSecretsService,
            IAudioPlayerService audioPlayerService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ClozeViewModel> logger)
            : base(service, sessionRepository, repository, userSecretsService, audioPlayerService, dispatcherService, navigationService, dialogService, logger)
        {
        }

        public bool IsWebViewLoaded { get; set; } = false;

        private Dictionary<string, string?> CurrentValues { get; } = new Dictionary<string, string?>();

        public override Task OnNavigatedToAsync()
        {
            WeakReferenceMessenger.Default.Register<SetValueMessage>(this, OnValueSet);
            return base.OnNavigatedToAsync();
        }

        public override Task OnNavigatingFromAsync()
        {
            WeakReferenceMessenger.Default.Unregister<SetValueMessage>(this);
            return base.OnNavigatingFromAsync();
        }

        protected override ClozeEntry CreateEntry(Cloze data)
        {
            return ClozeEntry.Import(data);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName != nameof(Question))
            {
                return;
            }

            foreach (var id in Question?.Ids ?? new List<string>())
            {
                OnValueSet(this, new SetValueMessage(id, null));
            }

            WeakReferenceMessenger.Default.Send(new ReloadMessage());
        }

        private void OnValueSet(object recipient, SetValueMessage message)
        {
            CurrentValues[message.Id] = message.Value;
        }
    }
}
