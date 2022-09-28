namespace De.HDBW.Apollo.Client.Selector
{
    public class MenuItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        public DataTemplate? TemplateWithSeperator { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var entry = item as BaseShellItem;
            if (entry?.Title == Resources.Strings.Resource.MainNavigation_FieldsOfInterest ||
                entry?.Title == Resources.Strings.Resource.MainNavigation_LearningPath ||
                entry?.Title == Resources.Strings.Resource.MainNavigation_CVGenerator)
            {
                return this.TemplateWithSeperator;
            }

            return this.DefaultTemplate;
        }
    }
}
