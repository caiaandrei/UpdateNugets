using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows.Input;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.Command
{
    public class BrowsePathCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var viewModel = parameter as SelectProjectPathViewModel;

            var dialog = new CommonOpenFileDialog();
            try
            {
                dialog.IsFolderPicker = true;
                dialog.EnsurePathExists = true;
                dialog.ShowDialog();
                viewModel.ProjectPath = dialog.FileName;
            }
            catch
            {

            }
        }
    }
}
