// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Enums;
using De.HDBW.Apollo.Client.Helper;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace De.HDBW.Apollo.Client.Models.Interactions
{
    public partial class StartViewInteractionEntry : InteractionEntry
    {
        [ObservableProperty]
        private Status _status;

        [ObservableProperty]
        private string? _subline;

        [ObservableProperty]
        private string? _info;

        [ObservableProperty]
        private string? _imagePath;

        [ObservableProperty]
        private string? _decoratorText;

        private StartViewInteractionEntry(string? text, string? subline, string? decoratorText, string? info, string imagePath, Status status, Type entityType, object? data, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle)
            : base(text, data, navigateHandler, canNavigateHandle)
        {
            Subline = subline;
            Info = info;
            Status = status;
            EntityType = entityType;
            ImagePath = imagePath?.ToUniformedName();
            DecoratorText = decoratorText;
        }

        public Type EntityType { get; }

        public bool HasDecorator
        {
            get
            {
                return !string.IsNullOrWhiteSpace(DecoratorText);
            }
        }

        public static InteractionEntry Import<TU>(string text, string subline, string decoratorText, string info, string imagePath, Status status, object? data, Func<InteractionEntry, Task> handleInteract, Func<InteractionEntry, bool> canHandleInteract)
        {
            return new StartViewInteractionEntry(text, subline, decoratorText, info,imagePath, status, typeof(TU), data, handleInteract, canHandleInteract);
        }
    }
}
