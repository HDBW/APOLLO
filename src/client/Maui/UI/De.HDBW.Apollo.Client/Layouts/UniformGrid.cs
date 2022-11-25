﻿using System.ComponentModel;
using Microsoft.Maui.Controls.Compatibility;

namespace De.HDBW.Apollo.Client.Layouts
{
    public class UniformGrid : Layout<View>
    {
        public static readonly BindableProperty ColumnCountProperty =
            BindableProperty.Create(nameof(ColumnCount), typeof(int), typeof(UniformGrid), 0);

        private double _childWidth;
        private double _childHeight;

        public int ColumnCount
        {
            get { return (int)GetValue(ColumnCountProperty); }
            set { SetValue(ColumnCountProperty, value); }
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            Measure(width, height, 0);
            int rows = GetRowsCount(Children.Count, ColumnCount);
            double boundsWidth = width / ColumnCount;
            double boundsHeight = _childHeight;
            Rect bounds = new Rect(0, 0, boundsWidth, boundsHeight);
            int count = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < ColumnCount && count < Children.Count; j++)
                {
                    View item = Children[count];
                    bounds.X = j * boundsWidth;
                    bounds.Y = i * boundsHeight;
                    item.Layout(bounds);
                    count++;
                }
            }
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            _childHeight = 0;
            foreach (View child in Children)
            {
                if (!child.IsVisible)
                {
                    continue;
                }

                SizeRequest sizeRequest = child.Measure(widthConstraint / ColumnCount, double.PositiveInfinity, 0);
                Size minimum = sizeRequest.Minimum;
                Size request = sizeRequest.Request;

                _childHeight = Math.Max(_childHeight, Math.Max(minimum.Height, request.Height));
                _childWidth = Math.Max(minimum.Width, request.Width);
            }

            int rows = GetRowsCount(Children.Count, ColumnCount);
            Size size = new Size(ColumnCount * _childWidth, rows * _childHeight);
            return new SizeRequest(size, size);
        }

        private int GetRowsCount(int visibleChildrenCount, int columnsCount)
        {
            return (int)Math.Ceiling((double)visibleChildrenCount / columnsCount);
        }
    }
}