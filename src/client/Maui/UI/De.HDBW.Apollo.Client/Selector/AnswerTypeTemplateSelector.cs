// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Selector
{
    public class AnswerTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        public DataTemplate? BooleanImageTemplate { get; set; }

        public DataTemplate? BooleanImageWithTextTemplate { get; set; }

        public DataTemplate? DefaultBooleanTemplate { get; set; }

        public DataTemplate? DefaultLocationTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var entry = item as AnswerEntry;

            DataTemplate? result = null;
            if (entry != null)
            {
                switch (entry.AnswerType)
                {
                    case AnswerType.Boolean:

                        if (entry.HasImage && entry.HasText)
                        {
                            result = BooleanImageWithTextTemplate;
                        }
                        else if (entry.HasImage)
                        {
                            return BooleanImageTemplate;
                        }
                        else
                        {
                            result = DefaultBooleanTemplate;
                        }

                        break;
                    case AnswerType.Location:
                        result = DefaultLocationTemplate;
                        break;
                }
            }

            return result ?? DefaultTemplate;
        }
    }
}
