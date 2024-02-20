// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Training;

namespace De.HDBW.Apollo.Client.Selector
{
    public class AppointmentItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? LineItemTemplate { get; set; }

        public DataTemplate? HeaderItemTemplate { get; set; }

        public DataTemplate? HeaderedLineItemTemplate { get; set; }

        public DataTemplate? ButtonLineItemTemplate { get; set; }

        public DataTemplate? ContactItemTemplate { get; set; }

        public DataTemplate? OccurenceItemTemplate { get; set; }

        public DataTemplate? ExpandedOccurenceItemTemplate { get; set; }

        public DataTemplate? SeperatorItemTemplate { get; set; }

        private DataTemplate? DefaultTemplate { get; } = new DataTemplate();

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case HeaderItem _:
                    return HeaderItemTemplate;
                case ContactItem _:
                    return ContactItemTemplate;
                case ButtonLineItem _:
                    return ButtonLineItemTemplate;
                case HeaderedLineItem _:
                    return HeaderedLineItemTemplate;
                case LineItem _:
                    return LineItemTemplate;
                case OccurenceItem _:
                    return OccurenceItemTemplate;
                case ExpandedOccurenceItem _:
                    return ExpandedOccurenceItemTemplate;
                case SeperatorItem _:
                    return SeperatorItemTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
