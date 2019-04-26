namespace GridSplitterSizesDemo
{
    public partial class MainWindow
    {
        private GridColumnAdornerSet _set1;
        private GridColumnAdornerSet _set2;

        public MainWindow()
        {
            InitializeComponent();
            _set1 = new GridColumnAdornerSet(GridSplitter1);
            _set2 = new GridColumnAdornerSet(GridSplitter2);
        }
    }
}