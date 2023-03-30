// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Assessment;

namespace De.HDBW.Apollo.Client.Selector
{
    public class QuestionDetailTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        public DataTemplate? ImageTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var entry = item as QuestionDetailEntry;
            switch (entry?.HasImage)
            {
                case true:
                    return ImageTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
