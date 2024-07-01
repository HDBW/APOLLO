// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Questions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class RatingViewModel : AbstractQuestionViewModel<Rating, RatingEntry>
    {
        [ObservableProperty]
        private ObservableCollection<RatingEntry> _multipleQuestions = new ObservableCollection<RatingEntry>();

        public RatingViewModel(
            IAssessmentService service,
            IRawDataCacheRepository repository,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<RatingViewModel> logger)
            : base(service, repository, dispatcherService, navigationService, dialogService, logger)
        {
        }

        protected override async Task LoadDataAsync()
        {
            Questions = new List<Rating>();//await Service.GetQuestionsByTypeAsync<Rating>(Culture, CancellationToken.None);
            var items = new List<RatingEntry>
            {
                CreateEntry(Questions.ToList()[Offset]),
            };

            if (Offset < Questions.Count - 1)
            {
                Offset = Offset + 1;
                items.Add(CreateEntry(Questions.ToList()[Offset]));
            }

            MultipleQuestions = new ObservableCollection<RatingEntry>(items);
        }

        protected override RatingEntry CreateEntry(Rating data)
        {
            return RatingEntry.Import(data);
        }
    }
}
