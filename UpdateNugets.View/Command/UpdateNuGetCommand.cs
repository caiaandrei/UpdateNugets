using NuGet.Packaging;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UpdateNugets.Core;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.Command
{
    public class UpdateNuGetCommand : ICommand
    {
        private ProjectNuGet _nuGet;
        private ManageNugets _manageNuGets;
        private IEventAggregator _eventAggregator;

        public UpdateNuGetCommand(ProjectNuGet nuGet, ManageNugets manageNuGets, IEventAggregator eventAggregator)
        {
            _nuGet = nuGet;
            _manageNuGets = manageNuGets;
            _eventAggregator = eventAggregator;
            _nuGet.CurrentSelectedVersionChanged += CurrentSelectedVersionChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _nuGet.CurrentVersion != _nuGet.CurrentSelectedVersion;
        }

        public void Execute(object parameter)
        {
            _manageNuGets.UpdateNuGets(_nuGet.Name, _nuGet.CurrentSelectedVersion.NuGetVersion, _nuGet.CurrentVersion.Files);

            _nuGet.CurrentSelectedVersion.Files.AddRange(_nuGet.CurrentVersion.Files);
            _nuGet.CurrentSelectedVersion.IsTheCurrentVersion = true;

            _nuGet.CurrentVersion.IsTheCurrentVersion = false;
            _nuGet.CurrentVersion.Files.Clear();

            _nuGet.CurrentVersion = _nuGet.CurrentSelectedVersion;

            _eventAggregator.GetEvent<SelectedVersionChanged>().Publish(_nuGet.CurrentSelectedVersion);
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CurrentSelectedVersionChanged(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
