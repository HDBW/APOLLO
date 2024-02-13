// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;

namespace De.HDBW.Apollo.Client.Views.PropertyEditor
{
    public partial class DefaultComboboxPropertyEditor : ContentView, ITouchAwareView
    {
        public DefaultComboboxPropertyEditor()
        {
            InitializeComponent();
        }

        public void OnTouchDown(float x, float y)
        {
            var ho = X;
            var hi = Y;
            var xx = GetAbsolutePosition(this);
#if ANDROID
            var view = Handler?.PlatformView as Android.Views.View;
            var pos = new int[2] { 0, 0 };
            var rect = default(Rect);
            view?.GetGlobalVisibleRect(rect);
            var p = new Android.Graphics.Point();
            view?.GetHitRect(rect);
            var posV = new int[2] { (int)rect.X, (int)rect.Y };
            view?.GetLocationInWindow(pos);
            view?.GetLocationOnScreen(posV);
#endif
        }

        public void OnTouchUp(float x, float y)
        {
        }

        private void HandleLoaded(object sender, EventArgs e)
        {
            var touchService = IocServiceHelper.ServiceProvider?.GetService<ITouchService>();
            touchService?.RegisterView(this);
        }

        private void HandleUnloaded(object sender, EventArgs e)
        {
            var touchService = IocServiceHelper.ServiceProvider?.GetService<ITouchService>();
            touchService?.UnregisterView(this);
        }

        private Point GetAbsolutePosition(VisualElement visualElement)
        {
            var ancestors = Ancestors(visualElement);
            var x = ancestors.Sum(ancestor => ancestor.X);
            var y = ancestors.Sum(ancestor => ancestor.Y);

            return new Point(x, y);
        }

        private IEnumerable<VisualElement> Ancestors(VisualElement? element)
        {
            while (element != null)
            {
                yield return element;
                element = element.Parent as VisualElement;
            }
        }
    }
}
