namespace De.HDBW.Apollo.Client.Models
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class Instruction : ObservableObject
    {
        [ObservableProperty]
        private string text;

        [ObservableProperty]
        private string image;

        private Instruction()
        {
        }

        public static Instruction Import (string image, string text)
        {
            return new Instruction()
            {
                Image = image,
                Text = text,
            };
        }
    }
}
