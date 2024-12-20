// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Dialogs
{
    public partial class CancelAssessmentDialog : Dialog
    {
        public CancelAssessmentDialog(CancelAssessmentDialogViewModel model)
            : base()
        {
            InitializeComponent();
            BindingContext = model;
        }

        public CancelAssessmentDialogViewModel? ViewModel
        {
            get
            {
                return BindingContext as CancelAssessmentDialogViewModel;
            }
        }

        private void OnPointerReleased(object sender, PointerEventArgs e)
        {
            var position = e.GetPosition(PART_Root) ?? Point.Zero;
            if (!PART_Root.Frame.Contains(position))
            {
                ViewModel?.CancelCommand?.Execute(null);
            }
        }
    }
}
