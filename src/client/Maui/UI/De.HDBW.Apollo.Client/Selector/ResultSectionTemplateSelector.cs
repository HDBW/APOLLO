// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Assessment;

namespace De.HDBW.Apollo.Client.Selector
{
    public class ResultSectionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DecoTemplate { get; set; }

        public DataTemplate DefaultTemplate { get; } = new DataTemplate();

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case DecoEntry _:
                    return DecoTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
