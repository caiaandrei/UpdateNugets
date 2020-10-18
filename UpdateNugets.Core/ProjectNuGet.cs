using System;
using System.Collections.Generic;
using System.Linq;

namespace UpdateNugets.Core
{
    public class ProjectNuGet
    {
        private Version _currentSelectedVersion;
        private Version _currentVersion;

        public ProjectNuGet(string name, IList<Version> versions)
        {
            Name = name;
            Versions = versions;
            CurrentVersion = Versions.FirstOrDefault(version => version.IsTheCurrentVersion);
        }

        public event EventHandler CurrentSelectedVersionChanged;

        public string Name { get; }

        public IList<Version> Versions { get; }

        public Version CurrentVersion
        {
            get => _currentVersion;
            set => _currentVersion = value;
        }

        public Version CurrentSelectedVersion 
        { 
            get => _currentSelectedVersion; 
            set
            {
                _currentSelectedVersion = value;
                CurrentSelectedVersionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
