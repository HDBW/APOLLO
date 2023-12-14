// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Selector
{
    public class MenuItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        public DataTemplate? TemplateWithSeperator { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var entry = item as BaseShellItem;
            if (entry?.Title == Resources.Strings.Resources.MainNavigation_FieldsOfInterest ||
                entry?.Title == Resources.Strings.Resources.MainNavigation_LearningPath ||
                entry?.Title == Resources.Strings.Resources.MainNavigation_CVGenerator)
            {
                return TemplateWithSeperator;
            }

            return DefaultTemplate;
        }
    }
}
