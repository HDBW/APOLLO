using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models
{
    public partial class InstructionEntry : ObservableObject
    {
        [ObservableProperty]
        private string? _text;

        [ObservableProperty]
        private string? _image;

        [ObservableProperty]
        private string? _animation;

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
