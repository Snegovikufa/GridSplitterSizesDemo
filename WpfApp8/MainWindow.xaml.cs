using System.Windows;
using System.Windows.Documents;

namespace WpfApp8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Поля и свойства

        private GridRowColumnAdornerSet _set;
        private SimpleCircleAdorner _simpleCircleAdorner;

        #endregion

        #region Конструкторы

        public MainWindow()
        {
            InitializeComponent();

            _set = new GridRowColumnAdornerSet(GridSplitter);

           // _simpleCircleAdorner = new SimpleCircleAdorner(Grid);
           //var layer =  AdornerLayer.GetAdornerLayer(Grid);
           //layer.Add(_simpleCircleAdorner);
        }

        #endregion
    }
}