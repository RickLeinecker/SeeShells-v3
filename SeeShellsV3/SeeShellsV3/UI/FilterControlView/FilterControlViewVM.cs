using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;
using SeeShellsV3.Data;
using SeeShellsV3.Repositories;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;

namespace SeeShellsV3.UI
{
    public class FilterControlViewVM : ViewModel, IFilterControlViewVM
    {
        [Dependency]
        public IUserCollection UserCollection { get; set; }

        [Dependency]
        public IRegistryHiveCollection RegistryHiveCollection { get; set; }

        public IShellEventCollection ShellEvents { get; private set; }

        public IShellItemCollection ShellItems { get; private set; }

        public ISelected Selected { get; private set; }

        public User User
        {
            get => user;
            set
            {
                User old = user;
                user = value;

                if (old != user)
                    ShellEvents.FilteredView.Refresh();

                NotifyPropertyChanged();
            }
        }

        public RegistryHive RegistryHive
        {
            get => registryHive;
            set
            {
                RegistryHive old = registryHive;
                registryHive = value;

                if (old != registryHive)
                    ShellEvents.FilteredView.Refresh();

                NotifyPropertyChanged();
            }
        }

        public Type Type
        {
            get => type;
            set
            {
                Type old = type;
                type = value;

                if (old != type)
                    ShellEvents.FilteredView.Refresh();

                NotifyPropertyChanged();
            }
        }

        public string Keyword {
            get => keyword;
            set
            {
                string old = keyword;
                keyword = value;

                if (old != keyword)
                    ShellEvents.FilteredView.Refresh();

                NotifyPropertyChanged();
            } }
        public string Path
        {
            get => path;
            set
            {
                string old = path;
                path = value;

                if (old != path)
                    ShellEvents.FilteredView.Refresh();

                NotifyPropertyChanged();
            }
        }

        public DateTime? Begin
        {
            get => begin;
            set
            {
                DateTime? old = begin;
                begin = value;

                if (old != begin)
                {
                    ShellEvents.FilteredView.Refresh();
                    ShellItems.FilteredView.Refresh();
                }

                NotifyPropertyChanged();
            }
        }

        public DateTime? End
        {
            get => end;
            set
            {
                DateTime? old = end;
                end = value;

                if (old != end)
                {
                    ShellEvents.FilteredView.Refresh();
                    ShellItems.FilteredView.Refresh();
                }

                NotifyPropertyChanged();
            }
        }

        private Type type = null;
        private User user = null;
        private RegistryHive registryHive = null;
        private DateTime? begin = null;
        private DateTime? end = null;
        private string path = null;
        private string keyword = null;

        public FilterControlViewVM([Dependency] IShellEventCollection shellEvents, [Dependency] IShellItemCollection shellItems ,[Dependency] ISelected selected)
        {
            ShellEvents = shellEvents;
            ShellItems = shellItems;
            Selected = selected;

            Selected.PropertyChanged += new PropertyChangedEventHandler(selectChange);
            
            ShellEvents.Filter += new FilterEventHandler(FilterType);
            ShellEvents.Filter += new FilterEventHandler(FilterPath);
            ShellEvents.Filter += new FilterEventHandler(FilterUser);
            ShellEvents.Filter += new FilterEventHandler(FilterRegistryHive);
            ShellEvents.Filter += new FilterEventHandler(FilterBeginDate);
            ShellEvents.Filter += new FilterEventHandler(FilterEndDate);
            ShellEvents.Filter += new FilterEventHandler(FilterKeyword);
            ShellItems.Filter += new FilterEventHandler(FilterItemBegin);
            ShellItems.Filter += new FilterEventHandler(FilterItemEnd);
        }

        void selectChange(object sender, PropertyChangedEventArgs e)
        {
            if (Selected.CurrentData != null && Selected.regView)
            {
                ShellItem temp = (ShellItem)Selected.CurrentData;


                if (temp.Place != null && temp.Place.PathName != null)
                {
                    if (temp.Place.PathName.EndsWith("\\"))
                        Path = (temp.Place.PathName ?? string.Empty) + (temp.Place.Name ?? string.Empty);
                    else
                        Path = (temp.Place.PathName ?? string.Empty) + "\\" + (temp.Place.Name ?? string.Empty);
                }

                else
                {
                    if (temp.Place.PathName == null && temp.Place.Name != null)
                        Path = temp.Place.Name;
                }
            }

        }

        void FilterKeyword(object sender, FilterEventArgs e)
        {
            if (Keyword == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item is IShellEvent se && se.Description.ToLower().Contains(Keyword.ToLower());
        }

        void FilterItemBegin(object o, FilterEventArgs e)
        {
            if (Begin == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item is IShellItem si && si.LastRegistryWriteDate >= Begin;
        }

        void FilterItemEnd(object o, FilterEventArgs e)
        {
            if (End == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item is IShellItem si && si.LastRegistryWriteDate <= End;
        }

        void FilterType(object o, FilterEventArgs e)
        {
            if (Type == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item.GetType() == Type;
        }

        void FilterPath(object o, FilterEventArgs e)
        {
            if (Path == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item is IShellEvent se && se.Place != null && ((se.Place.PathName ?? string.Empty) + (se.Place.Name ?? string.Empty)).ToLower().StartsWith(Path.ToLower());
        }

        void FilterUser(object o, FilterEventArgs e)
        {
            if (User == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item is IShellEvent se && se.User == User;
        }

        void FilterRegistryHive(object o, FilterEventArgs e)
        {
            if (RegistryHive == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item is IShellEvent se && se.Evidence.Any() && se.Evidence.First().RegistryHive == RegistryHive;
        }

        void FilterBeginDate(object o, FilterEventArgs e)
        {
            if (Begin == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item is IShellEvent se && se.TimeStamp >= Begin;
        }

        void FilterEndDate(object o, FilterEventArgs e)
        {
            if (End == null)
                e.Accepted = true;
            else
                e.Accepted = e.Item is IShellEvent se && se.TimeStamp <= End;
        }
    }

    internal class EventTypeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is IShellEventCollection shellEvents && values[1] is int count)
            {
                return shellEvents.Select(e => e.GetType()).Distinct();
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
