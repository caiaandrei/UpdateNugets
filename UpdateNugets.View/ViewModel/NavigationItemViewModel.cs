using Prism.Commands;
using Prism.Events;
using System;
using System.Windows.Input;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _name;
        private IEventAggregator _eventAggregator;

        public NavigationItemViewModel(string name, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Name = name;
            OpenDetailViewCommand = new DelegateCommand(OnExecuteOpenDetailViewCommand);
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ICommand OpenDetailViewCommand { get; }

        private void OnExecuteOpenDetailViewCommand()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Publish(new OpenDetailViewEventArgs
            {
                Name = Name
            });
        }
    }
}
