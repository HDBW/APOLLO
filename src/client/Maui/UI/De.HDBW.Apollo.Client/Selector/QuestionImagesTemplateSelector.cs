// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Assessment;

namespace De.HDBW.Apollo.Client.Selector
{
    public class QuestionImagesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? PageableImagesTemplate { get; set; }

        public DataTemplate? ZoomableImageTemplate { get; set; }

        private DataTemplate DefaultDataTemplate { get; set; } = new DataTemplate();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case PageableImagesEntry _:
                    return PageableImagesTemplate ?? DefaultDataTemplate;
                case ZoomableImageEntry _:
                    return ZoomableImageTemplate ?? DefaultDataTemplate;
                default:
                    return DefaultDataTemplate;
            }
        }
    }
}
