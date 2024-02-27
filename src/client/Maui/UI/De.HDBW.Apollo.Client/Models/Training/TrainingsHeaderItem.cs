﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using Invite.Apollo.App.Graph.Common.Models.Trainings;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class TrainingsHeaderItem : ObservableObject, IProvideImageData
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
        private string? _providerImage;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasIndividualStartDate))]
        private string? _individualStartDate;

        [ObservableProperty]
        private string? _providerName;

        [ObservableProperty]
        private bool _accessibilityAvailable;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasProviderImage))]
        private string? _imageData;

        [NotifyPropertyChangedFor(nameof(HasModes))]
        [ObservableProperty]
        private ObservableCollection<string> _modes = new ObservableCollection<string>();

        private TrainingsHeaderItem(
            string? trainingName,
            string? subTitle,
            string imagePath,
            string? trainingType,
            string? providerName,
            string? providerImage,
            bool? accessibilityAvailable,
            TrainingMode? mode,
            string? individualStartDate)
        {
            TrainingName = trainingName;
            SubTitle = subTitle;
            ImagePath = imagePath;
            TrainingType = trainingType;
            ProviderName = providerName;
            ProviderImage = providerImage;
            AccessibilityAvailable = accessibilityAvailable ?? false;
            IndividualStartDate = individualStartDate;
            if (mode == null)
            {
                return;
            }

            if ((mode & TrainingMode.Online) != 0)
            {
                Modes.Add(Resources.Strings.Resources.TrainingMode_Online);
            }

            if ((mode & TrainingMode.Offline) != 0)
            {
                Modes.Add(Resources.Strings.Resources.TrainingMode_Offline);
            }

            if ((mode & TrainingMode.Hybrid) != 0)
            {
                Modes.Add(Resources.Strings.Resources.TrainingMode_Hybrid);
            }

            if ((mode & TrainingMode.OnDemand) != 0)
            {
                Modes.Add(Resources.Strings.Resources.TrainingMode_OnDemand);
            }
        }

        public bool HasIndividualStartDate
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ProviderImage);
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

        public bool HasModes
        {
            get
            {
                return Modes.Any();
            }
        }

        public bool HasProviderImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImageData);
            }
        }

        public static TrainingsHeaderItem Import(
            string? trainingName,
            string? subTitle,
            string imagePath,
            string? trainingType,
            string? providerName,
            string? providerImage,
            bool? accessibilityAvailable,
            TrainingMode? mode,
            string? individualStartDate)
        {
            return new TrainingsHeaderItem(trainingName, subTitle, imagePath, trainingType, providerName, providerImage, accessibilityAvailable, mode, individualStartDate);
        }
    }
}