using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.View
{
    public partial class MainWindow
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}
