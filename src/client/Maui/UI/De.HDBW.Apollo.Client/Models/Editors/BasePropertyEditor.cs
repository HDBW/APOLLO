using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public abstract partial class BasePropertyEditor : ObservableObject, IPropertyEditor
    {
        [ObservableProperty]
        private bool _hasChanges;

        protected BasePropertyEditor(
            BaseValue data)
        {
            Data = data;
        }

        public BaseValue Data { get; set; }

        public string Label
        {
            get
            {
                return Data.Label;
            }
        }
    }
}
