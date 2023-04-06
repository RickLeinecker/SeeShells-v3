using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using SeeShellsV3.Data;
using SeeShellsV3.Services;
using Unity;

namespace SeeShellsV3.UI;

public class TimezoneWindowVM: ViewModel, ITimezoneWindowVM
{
    public ITimezoneManager TimezoneManager { get; set; }
    public Timezone SelectedTimeZone { get; set; }

    public string Theme
    {
        get => (Application.Current as App).currentTheme == "Dark" ? "White" : "Black";
    }

    public string Keyword
    {
        get => keyword;
        set
        {
            string old = keyword;
            keyword = value;

            if (old != keyword)
                TimezoneManager.SupportedTimezones.FilteredView.Refresh();

            NotifyPropertyChanged();
        }
    }
    private string keyword = null;

    public string Status
    {
        get => _status;
        set { _status = value; NotifyPropertyChanged(); }
    }
    private string _status = string.Empty;

    public TimezoneWindowVM([Dependency] ITimezoneManager timeMan)
    {
        TimezoneManager = timeMan;
        TimezoneManager.SupportedTimezones.Filter += new FilterEventHandler(FilterKeyword);

        Status = "Select Timezone";
    }

    public void SelectTimezone(Timezone zone)
    {
        if (zone == null)
        {
            Status = "Select Timezone";
            return;
        }

        SelectedTimeZone = zone;
        Status = $"Change to {zone.Name}";
    }

    public bool UpdateTimezone()
    {
        if (Status is "Select Timezone") return false;

        TimezoneManager.TimezoneChangeHandler(SelectedTimeZone);
        Keyword = null;

        return true;
    }

    void FilterKeyword(object o, FilterEventArgs e)
    {
        if (Keyword == null)
            e.Accepted = true;
        else if(e.Item is ITimezone)
        {
            ITimezone tz = e.Item as ITimezone;
            bool containsName = tz.Name.ToLower().Contains(Keyword.ToLower());
            bool containsOffset = tz.Offset.ToLower().Contains(Keyword.ToLower());
            bool containsLocale = tz.Locale.ToLower().Contains(Keyword.ToLower());
            bool containsDaylight = tz.DaylightStatus.ToLower().Contains(Keyword.ToLower()) ||
                                    (Keyword.ToLower() is "daylight" && (tz.DaylightStatus is "Yes" || tz.DaylightStatus is "No")) ||
                                    (Keyword.ToLower() is "no daylight" && tz.DaylightStatus is "N/A");


            e.Accepted = containsName || containsOffset || containsLocale || containsDaylight;
        }
        else
            e.Accepted = false;
    }
}