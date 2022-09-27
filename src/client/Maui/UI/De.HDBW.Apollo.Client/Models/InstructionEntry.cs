namespace De.HDBW.Apollo.Client.Models
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class InstructionEntry : ObservableObject
    {
        [ObservableProperty]
        private string? text;

        [ObservableProperty]
        private string? image;

        [ObservableProperty]
        private string? animation;

        private InstructionEntry()
        {
        }

        public static InstructionEntry Import(string? image, string? animation, string? text)
        {
            return new InstructionEntry()
            {
                Animation = animation,
                Image = image,
                Text = text,
            };
        }
    }
}
