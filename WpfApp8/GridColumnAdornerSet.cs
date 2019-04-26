using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace GridSplitterSizesDemo
{
    internal class GridColumnAdornerSet
    {
        private GridColumnWidthAdorner[] _adorners;

        public GridColumnAdornerSet(GridSplitter gridSplitter)
        {
            GridSplitter = gridSplitter;
            RefreshGridInformation();
        }

        private IList<ColumnDefinition> ColumnDefinitions { get; set; }

        private int Columns
        {
            get { return ColumnDefinitions.Count; }
        }

        private Grid GridElement { get; set; }
        private GridSplitter GridSplitter { get; }

        private void RefreshGridInformation()
        {
            GridElement = GridSplitter.GetVisualAncestor<Grid>();

            if (GridElement != null)
            {
                ColumnDefinitions = GridElement.ColumnDefinitions;

                GridSplitter.DragStarted += GridSplitterOnDragStartedHandler;
                GridSplitter.DragDelta += GridSplitterOnDragDeltaHandler;
                GridSplitter.DragCompleted += GridSplitterOnDragCompletedHandler;
            }
            else
            {
                ColumnDefinitions = new List<ColumnDefinition>();
            }
        }

        private void GridSplitterOnDragDeltaHandler(object sender, DragDeltaEventArgs e)
        {
            UpdateAdornersPositions();
        }

        private void UpdateAdornersPositions()
        {
            var pos = Mouse.GetPosition(GridElement);

            foreach (var adorner in _adorners) adorner.SetPosition(pos);
        }

        private void GridSplitterOnDragCompletedHandler(object sender, DragCompletedEventArgs e)
        {
            HideColumnsAdorners();
        }

        private void HideColumnsAdorners()
        {
            foreach (var adorner in _adorners) adorner.DeleteFromLayer();
        }

        private void GridSplitterOnDragStartedHandler(object sender, DragStartedEventArgs e)
        {
            CreateColumnAdorners();
            ShowColumnsAdorners();
            UpdateAdornersPositions();
        }

        private void ShowColumnsAdorners()
        {
        }

        private void CreateColumnAdorners()
        {
            if (Columns > 1)
            {
                var column = (int) GridSplitter.GetValue(Grid.ColumnProperty);

                // not all cases
                var leftColumn = GridSplitter.ResizeBehavior == GridResizeBehavior.PreviousAndNext
                    ? column - 1
                    : column;
                var rightColumn = GridSplitter.ResizeBehavior == GridResizeBehavior.PreviousAndCurrent
                    ? column
                    : column + 1;

                var root = GridElement.FindRoot();

                _adorners = new[]
                {
                    new GridColumnWidthAdorner(root, GridElement, ColumnDefinitions[leftColumn], -50),
                    new GridColumnWidthAdorner(root, GridElement, ColumnDefinitions[rightColumn], +30)
                };
            }
        }
    }
}