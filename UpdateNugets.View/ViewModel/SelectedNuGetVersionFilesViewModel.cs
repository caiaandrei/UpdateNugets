using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateNugets.Core;

namespace UpdateNugets.UI.ViewModel
{
    public class SelectedNuGetVersionFilesViewModel
    {
        private NuGet nuGet;

        public SelectedNuGetVersionFilesViewModel(NuGet nuGet)
        {
            this.nuGet = nuGet;
        }
    }
}
