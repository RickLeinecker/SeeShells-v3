using System;
using System.Collections.ObjectModel;
using System.Windows;
using SeeShellsV3.Data;
using SeeShellsV3.Services;
using Unity;

namespace SeeShellsV3.UI;

public class TimezoneWindowVM: ViewModel, ITimezoneWindowVM
{
    [Dependency] public ITimezoneManager TimezoneManager { get; set; }
    public Timezone SelectedTimeZone { get; set; }

    public string Keyword { get; set; }

    public string Status
    {
        get => _status;
        set { _status = value; NotifyPropertyChanged(); }
    }
    private string _status = string.Empty;

    public TimezoneWindowVM()
    {
        Status = "Select Timezone";
    }

    public void SelectTimezone(Timezone zone)
    {
        SelectedTimeZone = zone;
        Status = $"Change to {zone.Name}";
    }

    public bool UpdateTimezone()
    {
        if (Status is "Select Timezone") return false;

        TimezoneManager.TimezoneChangeHandler(SelectedTimeZone);
        NotifyPropertyChanged(nameof(TimezoneManager));
        return true;
    }
}