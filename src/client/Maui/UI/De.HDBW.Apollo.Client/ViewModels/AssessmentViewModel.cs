using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class AssessmentViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<object> _questions = new ObservableCollection<object>();

        [ObservableProperty]
        private object? _currentQuestion;

        public AssessmentViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<AssessmentViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            _questions = new ObservableCollection<object>()
            {
                "test",
                1,
                Visibility.Visible,
                FlowDirection.MatchParent,
                double.NaN
            };

            CurrentQuestion = Questions.FirstOrDefault();
        }

        [RelayCommand]
        public void NextQuestion()
        {
            var currentIndex = Questions.IndexOf(CurrentQuestion);
            currentIndex = currentIndex + 1 >= Questions.Count ? 0 : currentIndex + 1;
            CurrentQuestion = Questions[currentIndex];
        }
    }
}
