// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Selector
{
    public class QuestionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? UniformGridTemplate { get; set; }

        public DataTemplate? HorizontalListTemplate { get; set; }

        public DataTemplate? OverlayTemplate { get; set; }

        public DataTemplate? CompareTemplate { get; set; }

        public DataTemplate? DefaultTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case string:
                    return UniformGridTemplate;
                case int:
                    return HorizontalListTemplate;
                case Visibility:
                    return OverlayTemplate;
                case FlowDirection:
                    return CompareTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
