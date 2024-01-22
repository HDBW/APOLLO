// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
#if DEBUG
using System.Diagnostics;
#endif
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views
{
    public partial class UseCaseSelectionView
    {
        public UseCaseSelectionView(UseCaseSelectionViewModel model)
        {
#if DEBUG
            Debug.WriteLine($"Create {GetType()}");
#endif
            InitializeComponent();
            BindingContext = model;
        }

        ~UseCaseSelectionView()
        {
#if DEBUG
            Debug.WriteLine($"~{GetType()}");
#endif
        }

        public UseCaseSelectionViewModel? ViewModel
        {
            get
            {
                return BindingContext as UseCaseSelectionViewModel;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
#if IOS
            var handler = Handler as Microsoft.Maui.Handlers.PageHandler;
            var renderer = handler?.ViewController as UIKit.UIViewController;
            var gesture = renderer?.NavigationController?.InteractivePopGestureRecognizer;
            if (gesture == null)
            {
                return;
            }

            gesture.Enabled = false;
#endif
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
                    case CollectionView _:
                        break;

                    default:
                        var size = child.Measure(grid.Width, grid.Height);
                        heightSum += size.Height;
                        break;
                }
            }

            var diff = height - grid.Padding.Top - grid.Padding.Bottom - heightSum;
            PART_UseCases.MaximumHeightRequest = diff <= 0 ? double.PositiveInfinity : diff;
        }
    }
}
