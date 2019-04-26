using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfApp8
{
    public class GridColumnWidthAdorner : Adorner
    {
        #region Поля и свойства

        private readonly AdornerLayer adornerLayer;
        private readonly ColumnDefinition columnDefinition;
        private readonly int _delta;
        private readonly UIElement grid;
        private readonly TextBlock textBlock;
        private Point mousePosition;

        #endregion

        #region Методы

        public void RefreshPosition(Point pos)
        {
            MousePosition = pos;
            adornerLayer.Update(AdornedElement);

            var gl = columnDefinition.Width;
            switch (gl.GridUnitType)
            {
                case GridUnitType.Auto:
                    textBlock.Text = columnDefinition.ActualWidth.ToString("N2");
                    break;
                case GridUnitType.Star:
                    textBlock.Text = gl.Value.ToString("N2") + "*";
                    break;
                default:
                    textBlock.Text = gl.Value.ToString("N2");
                    break;
            }
        }

        /// <summary>
        /// Удаление из слоя отображения.
        /// </summary>
        public void DeleteFromLayer()
        {
            adornerLayer.Remove(this);
        }

        #endregion

        #region Базовый класс

        protected override Visual GetVisualChild(int index)
        {
            return textBlock;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            textBlock.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override int VisualChildrenCount { get; } = 1;

        protected override Size MeasureOverride(Size constraint)
        {
            textBlock.Measure(constraint);
            return textBlock.DesiredSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(this.MousePosition.X + _delta, this.MousePosition.Y));

            return result;
        }

        /// <summary>
        /// Положение мыши, за которым следует предосмотр таскаемого элемента.
        /// </summary>
        public Point MousePosition
        {
            get
            {
                return this.mousePosition;
            }

            set
            {
                if (this.mousePosition != value)
                {
                    this.mousePosition = value;
                    //this.adornerLayer.Update(this.AdornedElement);
                }
            }
        }

        #endregion

        #region Конструкторы

        public GridColumnWidthAdorner(UIElement grid, ColumnDefinition columnDefinition, int delta)
            : base(grid)
        {
            adornerLayer = AdornerLayer.GetAdornerLayer(grid);
            adornerLayer.Add(this);

            this.grid = grid;
            this.columnDefinition = columnDefinition;
            _delta = delta;
            textBlock = new TextBlock()
            {
                FontSize = 14,
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromRgb(29, 29, 29)),
                Padding = new Thickness(5)
                
            };
        }

        #endregion
    }
}