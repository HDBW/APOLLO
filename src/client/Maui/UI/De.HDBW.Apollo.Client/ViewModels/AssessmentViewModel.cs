// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Graph.Apollo.Cloud.Common.Models.Assessment;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class AssessmentViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<QuestionItem> _questions = new ObservableCollection<QuestionItem>();

        [ObservableProperty]
        private QuestionItem? _currentQuestion;
        private long? _assessmentItemId;

        public AssessmentViewModel(
            IAssessmentItemRepository assessmentItemRepository,
            IQuestionItemRepository questiontItemRepository,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<AssessmentViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            AssessmentItemRepository = assessmentItemRepository;
            QuestiontItemRepository = questiontItemRepository;
        }

        private IAssessmentItemRepository AssessmentItemRepository { get; }

        private IQuestionItemRepository QuestiontItemRepository { get; }

        [RelayCommand]
        public void NextQuestion()
        {
            var currentIndex = Questions.IndexOf(CurrentQuestion);
            currentIndex = currentIndex + 1 >= Questions.Count ? 0 : currentIndex + 1;
            CurrentQuestion = Questions[currentIndex];
        }

        protected override async void OnPrepare(NavigationParameters navigationParameters)
        {
            _assessmentItemId = navigationParameters.GetValue<long?>(NavigationParameter.Id);
            await OnNavigatedTo();
        }

        public async override Task OnNavigatedTo()
        {
            if (!_assessmentItemId.HasValue)
            {
                return;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    var assessmentItem = AssessmentItemRepository.GetItemByIdAsync(_assessmentItemId.Value, worker.Token).ConfigureAwait(false);

                    // TODO: Continue loading data.
                }
                catch
                {

                }
                finally
                {
                    UnscheduleWork(worker);
                }
        }

        }
    }
}
