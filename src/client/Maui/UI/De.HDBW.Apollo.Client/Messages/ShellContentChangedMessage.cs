namespace De.HDBW.Apollo.Client.Messages
{
    public class ShellContentChangedMessage
    {
        public ShellContentChangedMessage(Type? newViewModelType)
        {
            NewViewModelType = newViewModelType;
        }

        public Type? NewViewModelType { get; }
    }
}
