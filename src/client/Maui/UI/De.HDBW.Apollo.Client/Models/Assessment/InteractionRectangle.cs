// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class InteractionRectangle : InteractionShape
    {
        [ObservableProperty]
        private Rect _rectangle;

        public InteractionRectangle(Rect rectangle, bool isSelected, Action interactedHandler)
            : base(isSelected, interactedHandler)
        {
            Rectangle = rectangle;
        }
    }
}
