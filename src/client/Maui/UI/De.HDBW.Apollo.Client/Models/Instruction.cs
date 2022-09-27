namespace De.HDBW.Apollo.Client.Models
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class Instruction : ObservableObject
    {
        [ObservableProperty]
        private string text;

        [ObservableProperty]
        private string image;

        [ObservableProperty]
        private string animation;

        private Instruction()
        {
        }

        public static Instruction Import(string image, string animation,  string text)
        {
            return new Instruction()
            {
                Animation = animation,
                Image = image,
                Text = text,
            };
        }
    }
}
