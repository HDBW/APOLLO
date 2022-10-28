// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;

namespace De.HDBW.Apollo.Client.Selector
{
    public class IInteractiveEntryTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var entry = item as IInteractiveEntry;
            if (entry != null)
            {
                switch (entry.Interaction)
                {
                }
            }

            return DefaultTemplate;
        }
    }
}
