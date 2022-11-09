// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Selector
{
    public class SelectionAnswersTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        public DataTemplate? BooleanImageTemplate { get; set; }

        public DataTemplate? BooleanImageWithTextTemplate { get; set; }

        public DataTemplate? DefaultBooleanTemplate { get; set; }

        public DataTemplate? DefaultLocationTemplate { get; set; }

        public DataTemplate? DefaultStringTemplate { get; set; }

        public DataTemplate? DefaultIntegerTemplate { get; set; }

        public DataTemplate? DefaultLongTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var entry = item as AnswerEntry;
            if (entry != null)
            {
                switch (entry.AnswerType)
                {
                    case AnswerType.Boolean:

                        if (entry.HasImage && entry.HasText)
                        {
                            return BooleanImageWithTextTemplate;
                        }

                        if (entry.HasImage)
                        {
                            return BooleanImageTemplate;
                        }

                        return DefaultBooleanTemplate;
                    case AnswerType.Integer:
                        return DefaultIntegerTemplate;
                    case AnswerType.String:
                        return DefaultStringTemplate;
                    case AnswerType.Location:
                        return DefaultLocationTemplate;
                    case AnswerType.Long:
                        return DefaultLongTemplate;
                }
            }

            return DefaultTemplate;
        }
    }
}
