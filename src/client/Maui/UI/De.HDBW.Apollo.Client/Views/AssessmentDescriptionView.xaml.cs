// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

#if DEBUG
using System.Diagnostics;
#endif
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views
{
    public partial class AssessmentDescriptionView
    {
        public AssessmentDescriptionView(AssessmentDescriptionViewModel model)
        {
#if DEBUG
            Debug.WriteLine($"Create {GetType()}");
#endif
            InitializeComponent();
            BindingContext = model;
        }

        ~AssessmentDescriptionView()
        {
#if DEBUG
            Debug.WriteLine($"~{GetType()}");
#endif
        }

        public AssessmentDescriptionViewModel? ViewModel
        {
            get
            {
                return BindingContext as AssessmentDescriptionViewModel;
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var grid = Content as Grid;
            if (grid == null)
            {
                return;
            }

            grid.Measure(width, height);

            if (double.IsNaN(height) || double.IsInfinity(height))
            {
                height = Window.Height;
            }

            if (double.IsNaN(width) || double.IsInfinity(width))
            {
                height = Window.Width;
            }

            var heightSum = 0d;
            foreach (var child in grid.Children ?? Array.Empty<IView>())
            {
                switch (child)
                {
                    case ScrollView _:
                        break;

                    default:
                        var size = child.Measure(grid.Width, grid.Height);
                        heightSum += size.Height;
                        break;
                }
            }

            var diff = height - heightSum;
            PART_ScrollHost.MaximumHeightRequest = diff <= 0 ? double.PositiveInfinity : diff;
        }
    }
}
