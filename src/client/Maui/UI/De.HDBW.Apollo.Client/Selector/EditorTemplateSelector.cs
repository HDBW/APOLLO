// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.PropertyEditors;

namespace De.HDBW.Apollo.Client.Selector
{
    public class EditorTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultEditorTemplate { get; set; }

        public DataTemplate? StringPropertyEditorTemplate { get; set; }

        public DataTemplate? BooleanPropertyEditorTemplate { get; set; }

        public DataTemplate? PickerPropertyEditorTemplate { get; set; }

        public DataTemplate? DatePropertyEditorTemplate { get; set; }

        public DataTemplate? RangePropertyEditorTemplate { get; set; }

        public DataTemplate? ListPropertyEditorTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case StringPropertyEditor:
                    return StringPropertyEditorTemplate;
                case BooleanPropertyEditor:
                    return BooleanPropertyEditorTemplate;
                case PickerPropertyEditor:
                    return PickerPropertyEditorTemplate;
                case DatePropertyEditor:
                    return DatePropertyEditorTemplate;
                case RangePropertyEditor:
                    return RangePropertyEditorTemplate;
                case ListPropertyEditor:
                    return ListPropertyEditorTemplate;
            }

            return DefaultEditorTemplate;
        }
    }
}
