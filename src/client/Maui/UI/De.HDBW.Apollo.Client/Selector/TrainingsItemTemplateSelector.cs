// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Training;

namespace De.HDBW.Apollo.Client.Selector
{
    public class TrainingsItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? TrainingsHeaderItemTemplate { get; set; }

        public DataTemplate? TrainingModeItemTemplate { get; set; }

        public DataTemplate? EduProviderItemTemplate { get; set; }

        public DataTemplate? EduProviderItemWithImageTemplate { get; set; }

        public DataTemplate? ExpandableItemTemplate { get; set; }

        public DataTemplate? ExpandableListItemTemplate { get; set; }

        public DataTemplate? ExpandedItemContentTemplate { get; set; }

        public DataTemplate? ExpandedListContentTemplate { get; set; }

        public DataTemplate? TagItemTemplate { get; set; }

        public DataTemplate? ContactItemTemplate { get; set; }

        public DataTemplate? ContactListItemTemplate { get; set; }

        public DataTemplate? NavigationItemTemplate { get; set; }

        public DataTemplate? SeperatorItemTemplate { get; set; }

        private DataTemplate? DefaultTemplate { get; } = new DataTemplate();

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case SeperatorItem _:
                    return SeperatorItemTemplate;
                case TrainingsHeaderItem _:
                    return TrainingsHeaderItemTemplate;
                case TrainingModeItem _:
                    return TrainingModeItemTemplate;
                case EduProviderItem provider:
                    return provider.HasProviderImage ? EduProviderItemWithImageTemplate : EduProviderItemTemplate;
                case TagItem _:
                    return TagItemTemplate;
                case ExpandableItem _:
                    return ExpandableItemTemplate;
                case ExpandableListItem _:
                    return ExpandableListItemTemplate;
                case ExpandedItemContent _:
                    return ExpandedItemContentTemplate;
                case ExpandedListContent _:
                    return ExpandedListContentTemplate;
                case ContactItem _:
                    return ContactItemTemplate;
                case ContactListItem _:
                    return ContactListItemTemplate;
                case NavigationItem _:
                    return NavigationItemTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
