using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UpdateNugets.UI.View
{
    public partial class NuGetDependeciesView : UserControl
    {
        public NuGetDependeciesView()
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
