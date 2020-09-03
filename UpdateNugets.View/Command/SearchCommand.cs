using System;
using System.Threading.Tasks;
using System.Windows.Input;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.Command
{
    public class SearchCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action _action;
        public SearchCommand()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var viewModel = parameter as NuGetsListViewModel;
            _action = new Action(async () => await viewModel.SearchAsync());
            _action.Invoke();
        }
    }
}
