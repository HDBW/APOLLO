// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Invite.Apollo.App.Graph.Common.Models.Trainings;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class TrainingModeItem : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> _modes = new ObservableCollection<string>();

        private TrainingModeItem(TrainingMode mode)
        {
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

        public static TrainingModeItem Import(
            TrainingMode mode)
        {
            return new TrainingModeItem(mode);
        }
    }
}
