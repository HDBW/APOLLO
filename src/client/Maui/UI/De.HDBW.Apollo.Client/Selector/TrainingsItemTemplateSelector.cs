// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Training;

namespace De.HDBW.Apollo.Client.Selector
{
    public class TrainingsItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? HeaderItemTemplate { get; set; }

        public DataTemplate? ExpandableItemTemplate { get; set; }

        public DataTemplate? ExpandableListItemTemplate { get; set; }

        public DataTemplate? AppointmentItemTemplate { get; set; }

        public DataTemplate? TagItemTemplate { get; set; }

        private DataTemplate? DefaultTemplate { get; } = new DataTemplate();

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case HeaderItem _:
                    return HeaderItemTemplate;
                case TagItem _:
                    return TagItemTemplate;
                case ExpandableItem _:
                    return ExpandableItemTemplate;
                case ExpandableListItem _:
                    return ExpandableListItemTemplate;
                case AppointmentItem _:
                    return AppointmentItemTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
