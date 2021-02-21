using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UpdateNugets.UI.View
{
    public partial class NuGetVersionFilesView : UserControl
    {
        public NuGetVersionFilesView()
        {
            InitializeComponent();
        }

        private void CtrlCCopyCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var listBox = sender as ListBox;
            var selected = listBox.SelectedItem;

            if (selected != null)
            {
                Clipboard.SetText(selected.ToString());
            }
        }

        private void RightClickCopyCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var mi = sender as MenuItem;
            var selected = mi.DataContext;

            if (selected != null)
            {
                Clipboard.SetText(selected.ToString());
            }
        }

        private void CtrlCCopyCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RightClickCopyCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
