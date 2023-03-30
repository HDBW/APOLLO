// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Helper;

namespace De.HDBW.Apollo.Client.Models
{
    public partial class InstructionEntry : ObservableObject
    {
        [ObservableProperty]
        private string? _text;

        [ObservableProperty]
        private string? _subline;

        [ObservableProperty]
        private string? _imagePath;

        [ObservableProperty]
        private string? _animation;

        private InstructionEntry()
        {
        }

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath);
            }
        }

        public bool HasAnimation
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Animation);
            }
        }

        public static InstructionEntry Import(string? image, string? animation, string? text, string? subline)
        {
            return new InstructionEntry()
            {
                Animation = animation,
                ImagePath = image?.ToUniformedName(),
                Text = text,
                Subline = subline,
            };
        }
    }
}
