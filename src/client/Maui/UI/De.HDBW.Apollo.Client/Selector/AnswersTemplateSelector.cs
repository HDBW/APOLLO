// <copyright file="AnswersTemplateSelector.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using De.HDBW.Apollo.Client.Models.Assessment;

namespace De.HDBW.Apollo.Client.Selector
{
    public class AnswersTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        public DataTemplate? ImageTemplate { get; set; }

        public DataTemplate? ImageWithTextTemplate { get; set; }

        public DataTemplate? PointTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var entry = item as AnswerEntry;
            if (entry != null)
            {
                if (entry.HasPoint)
                {
                    return PointTemplate;
                }

                if (entry.HasImage && entry.HasText)
                {
                    return ImageWithTextTemplate;
                }

                if (entry.HasImage)
                {
                    return ImageTemplate;
                }
            }

            return DefaultTemplate;
        }
    }
}