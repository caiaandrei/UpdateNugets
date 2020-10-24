using System.Windows;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.View
{
    public partial class SelectProjectPathView
    {
        private SelectProjectPathViewModel _selectProjectPathViewModel;
        public SelectProjectPathView(SelectProjectPathViewModel selectProjectPathViewModel)
        {
            InitializeComponent();
            _selectProjectPathViewModel = selectProjectPathViewModel;
            DataContext = selectProjectPathViewModel;
        }

        private void DoneClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
