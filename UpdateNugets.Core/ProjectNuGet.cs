using System.Collections.Generic;

namespace UpdateNugets.Core
{
    public class ProjectNuGet
    {
        public ProjectNuGet(string name, IList<Version> versions)
        {
            Name = name;
            Versions = versions;
        }

        public string Name { get; }
        public IList<Version> Versions { get; }
    }
}
