// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views
{
    public partial class AssessmentView
    {
        public AssessmentView(AssessmentViewModel model)
        {
#if DEBUG
            Debug.WriteLine($"Create {GetType()}");
#endif
            InitializeComponent();
            BindingContext = model;
        }

        ~AssessmentView()
        {
#if DEBUG
            Debug.WriteLine($"~{GetType()}");
#endif
        }

        public AssessmentViewModel? ViewModel
        {
            get
            {
                return BindingContext as AssessmentViewModel;
            }
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

        protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
        {
            try
            {
                // Remark: Work around for nullpointer during use of BackButtonBehaviour.
                var behaviour = Shell.GetBackButtonBehavior(this);
                if (behaviour == null)
                {
                    return;
                }

                behaviour.ClearValue(BackButtonBehavior.CommandProperty);
                behaviour.BindingContext = null;
                Shell.SetBackButtonBehavior(this, null);
            }
            finally
            {
                base.OnNavigatingFrom(args);
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
