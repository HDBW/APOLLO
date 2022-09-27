namespace De.HDBW.Apollo.Client.Views;

using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.ViewModels;
using Microsoft.Maui.Controls;
using SkiaSharp.Extended.UI.Controls;
using SkiaSharp.Extended.UI.Controls.Converters;

public partial class ExtendedSplashScreenView
{
    public ExtendedSplashScreenView(ExtendedSplashScreenViewModel model)
    {
        this.InitializeComponent();
        this.BindingContext = model;
    }

    public ExtendedSplashScreenViewModel? Viemodel
    {
        get
        {
            return this.BindingContext as ExtendedSplashScreenViewModel;
        }
    }
}