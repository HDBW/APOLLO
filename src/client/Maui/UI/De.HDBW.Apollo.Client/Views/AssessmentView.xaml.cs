// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;

public partial class AssessmentView
{
    public AssessmentView(AssessmentViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public AssessmentViewModel? Viemodel
    {
        get
        {
            return BindingContext as AssessmentViewModel;
        }
    }
}
