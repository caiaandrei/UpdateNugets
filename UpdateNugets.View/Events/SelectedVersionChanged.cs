using Prism.Events;
using UpdateNugets.Core;
using UpdateNugets.UI.Model;

namespace UpdateNugets.UI.Events
{
    public class SelectedVersionChanged : PubSubEvent<NugetModel>
    {
    }
}
