using System.Collections.Generic;

namespace UpdateNugets.UI.Model
{
    public class VersionModel
    {
        public string CurrentVersion { get; set; }
        public bool IsUsed { get; set; }
        public List<string> Files { get; set; }
    }
}
