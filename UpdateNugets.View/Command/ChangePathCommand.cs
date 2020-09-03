using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UpdateNugets.UI.View;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.Command
{
    public class ChangePathCommand : ICommand
    {
        public ChangePathCommand()
        {
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var viewModel = parameter as MainViewModel;
            var browseCommand = new BrowsePathCommand();
            var selectProjectPathViewModel = new SelectProjectPathViewModel(browseCommand);
            var selectProjectPathView = new SelectProjectPathView(selectProjectPathViewModel);
            selectProjectPathView.Owner = App.Current.MainWindow;

            selectProjectPathView.ShowDialog();

            if (!string.IsNullOrEmpty(selectProjectPathViewModel.ProjectPath))
            {
                viewModel.ProjectPath = selectProjectPathViewModel.ProjectPath;
            }
        }
    }
}
