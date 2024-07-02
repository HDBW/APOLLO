using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Dialogs;

public partial class ImageZoomDialog
{
    public ImageZoomDialog(MessageDialogViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public MessageDialogViewModel? ViewModel
    {
        get
        {
            return BindingContext as MessageDialogViewModel;
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
