using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class TrainingsHeaderItem : ObservableObject
    {
        [ObservableProperty]
        private string? _trainingName;

        [NotifyPropertyChangedFor(nameof(HasSubTitle))]
        [ObservableProperty]
        private string? _subTitle;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasImage))]
        private string? _imagePath;

        [ObservableProperty]
        private string? _trainingType;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasIndividualStartDate))]
        private string? _individualStartDate;

        [ObservableProperty]
        private bool _accessibilityAvailable;

        private TrainingsHeaderItem(
            string? trainingName,
            string? subTitle,
            string imagePath,
            string? trainingType,
            bool? accessibilityAvailable,
            string? individualStartDate)
        {
            TrainingName = trainingName;
            SubTitle = subTitle;
            ImagePath = imagePath;
            TrainingType = trainingType;
            AccessibilityAvailable = accessibilityAvailable ?? false;
            IndividualStartDate = individualStartDate;
        }

        public bool HasIndividualStartDate
        {
            get
            {
                return !string.IsNullOrWhiteSpace(IndividualStartDate);
            }
        }

        public bool HasSubTitle
        {
            get
            {
                return !string.IsNullOrWhiteSpace(SubTitle);
            }
        }

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath);
            }
        }

        public static TrainingsHeaderItem Import(
            string? trainingName,
            string? subTitle,
            string imagePath,
            string? trainingType,
            bool? accessibilityAvailable,
            string? individualStartDate)
        {
            return new TrainingsHeaderItem(trainingName, subTitle, imagePath, trainingType, accessibilityAvailable, individualStartDate);
        }
    }
}
