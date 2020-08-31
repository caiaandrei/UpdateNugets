using System;
using System.Windows.Input;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.Command
{
    public class SearchCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var viewModel = parameter as NuGetsListViewModel;
            viewModel.Search();
        }
    }
}
