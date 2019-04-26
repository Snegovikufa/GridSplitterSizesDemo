using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WpfApp8
{
    public class GridRowColumnAdornerSet
    {
        #region Поля и свойства

        private GridColumnWidthAdorner _leftAdorner;
        private GridColumnWidthAdorner _rightAdorner;

        private IList<ColumnDefinition> ColumnDefinitions { get; set; }

        private int Columns
        {
            get { return ColumnDefinitions.Count; }
        }

        private Grid GridElement { get; set; }
        private GridSplitter GridSplitter { get; }

        #endregion

        #region Методы

        private void RefreshGridInformation()
        {
            GridElement = GridSplitter.GetVisualAncestor<Grid>(null);

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
            _leftAdorner.RefreshPosition(pos);
            _rightAdorner.RefreshPosition(pos);
        }

        private void GridSplitterOnDragCompletedHandler(object sender, DragCompletedEventArgs e)
        {
            HideColumnsAdorners();
        }

        private void HideColumnsAdorners()
        {
            _leftAdorner.DeleteFromLayer();
            _rightAdorner.DeleteFromLayer();
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
                int column = (int)GridSplitter.GetValue(Grid.ColumnProperty);
                int leftColumn = column - 1;
                int rightColumn = column + 1;

                var root = GridElement.FindRoot();

                _leftAdorner = new GridColumnWidthAdorner(root, ColumnDefinitions[leftColumn], -50);
                _rightAdorner = new GridColumnWidthAdorner(root, ColumnDefinitions[rightColumn], +30);
            }
        }

        #endregion

        #region Конструкторы

        public GridRowColumnAdornerSet(GridSplitter gridSplitter)
        {
            GridSplitter = gridSplitter;
            RefreshGridInformation();
        }

        #endregion
    }
}