using De.HDBW.Apollo.Client.Models.Editors;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IPropertyEditor
    {
        BaseValue Data { get; }

        string Label { get; }
    }

    public interface IPropertyEditor<T> : IPropertyEditor
    {
        T Value { get; set; }
    }
}
