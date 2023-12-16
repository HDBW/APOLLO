using De.HDBW.Apollo.Client.Models.Editors;
using De.HDBW.Apollo.Client.Views.PropertyEditor;

namespace De.HDBW.Apollo.Client.Selector
{
    public class EditorTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case DateRangePropertyEditor:
                    return new DataTemplate(() =>
                    {
                        return new DateRangePicker() { };
                    });
                case StringPropertyEditor:
                    return new DataTemplate(() =>
                    {
                        return new DefaultStringPropertyEditor() { };
                    });
                case BooleanPropertyEditor:
                    return new DataTemplate(() =>
                    {
                        return new DefaultBooleanPropertyEditor() { };
                    });
                case PickerPropertyEditor:
                    return new DataTemplate(() =>
                    {
                        return new DefaultPickerPropertyEditor() { };
                    });
            }

            return DefaultTemplate;
        }
    }
}
