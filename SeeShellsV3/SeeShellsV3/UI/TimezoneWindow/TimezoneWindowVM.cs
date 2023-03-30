using System.Collections.ObjectModel;
using SeeShellsV3.Data;
using SeeShellsV3.Services;
using Unity;

namespace SeeShellsV3.UI;

public class TimezoneWindowVM: ViewModel, ITimezoneWindowVM
{
    [Dependency]
    public TimezoneManager TimezoneManager { get; }

    public Collection<Timezone> supportedTimezones { get; set; }
    public string Keyword { get; set; }

    public TimezoneWindowVM([Dependency] TimezoneManager TimeManager)
    {
        supportedTimezones = TimeManager.SupportedTimezones;
    }
}