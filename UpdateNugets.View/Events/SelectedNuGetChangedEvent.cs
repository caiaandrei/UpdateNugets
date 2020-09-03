using Prism.Events;
using UpdateNugets.Core;

namespace UpdateNugets.UI.Events
{
    public class SelectedNuGetChangedEvent : PubSubEvent<ProjectNuGet>
    {
    }
}
