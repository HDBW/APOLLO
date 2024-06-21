// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Selector
{
    public class AssessmentSectionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; } = new DataTemplate();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return DefaultTemplate;
        }
    }
}
