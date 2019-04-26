using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace GridSplitterSizesDemo
{
    internal class GridColumnWidthAdorner : Adorner
    {
        private readonly AdornerLayer _adornerLayer;
        private readonly ColumnDefinition _columnDefinition;
        private readonly int _margin;
        private readonly Grid _grid;
        private Point _mousePosition;
        private readonly TextBlock _textBlock;

        public GridColumnWidthAdorner(UIElement adornedElement, Grid grid, ColumnDefinition columnDefinition, int margin)
            : base(adornedElement)
        {
            _adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            _adornerLayer.Add(this);

            _grid = grid;
            _columnDefinition = columnDefinition;
            _margin = margin;

            _textBlock = new TextBlock
            {
                FontSize = 14,
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromRgb(29, 29, 29)),
                Padding = new Thickness(5)
            };

            UpdateText();
        }

        private Point MousePosition
        {
            get { return _mousePosition; }
            set
            {
                if (_mousePosition != value)
                {
                    _mousePosition = value;
                    _adornerLayer.Update(AdornedElement);
                }
            }
        }

        protected override int VisualChildrenCount { get; } = 1;

        public void SetPosition(Point pos)
        {
            MousePosition = pos;
            UpdateText();
        }

        public void DeleteFromLayer()
        {
            _adornerLayer.Remove(this);
        }

        private void UpdateText()
        {
            var percent = Math.Ceiling(_grid.ActualWidth > 0
                ? 100.0 * _columnDefinition.ActualWidth / _grid.ActualWidth
                : 0.0);
            _textBlock.Text = percent.ToString("N0");
        }

        protected override Visual GetVisualChild(int index)
        {
            return _textBlock;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _textBlock.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _textBlock.Measure(constraint);
            return _textBlock.DesiredSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(MousePosition.X + _margin, MousePosition.Y));
            return result;
        }
    }
}