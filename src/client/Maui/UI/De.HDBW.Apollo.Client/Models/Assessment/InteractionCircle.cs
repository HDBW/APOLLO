// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class InteractionCircle : InteractionShape
    {
        [ObservableProperty]
        private Point _location;

        [ObservableProperty]
        private double _radius;

        public InteractionCircle(Point location, double radius, bool isSelected)
            : base(isSelected)
        {
            Location = location;
            Radius = radius;
        }
    }
}
