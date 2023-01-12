using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SeeShellsV3.UI
{
    public class UtcToLocalTimeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime d)
            {
                string s = parameter as string;
                switch (s)
                {
                    case "ShortDate":
                        return d.ToLocalTime().ToShortDateString();
                    case "ShortTime":
                        return d.ToLocalTime().ToShortTimeString();
                    case "LongDate":
                        return d.ToLocalTime().ToLongDateString();
                    case "LongTime":
                        return d.ToLocalTime().ToLongTimeString();
                    default:
                        return d.ToLocalTime().ToString();
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime d)
            {
                string s = parameter as string;
                switch (s)
                {
                    case "ShortDate":
                        return d.ToUniversalTime().ToShortDateString();
                    case "ShortTime":
                        return d.ToUniversalTime().ToShortTimeString();
                    case "LongDate":
                        return d.ToUniversalTime().ToLongDateString();
                    case "LongTime":
                        return d.ToUniversalTime().ToLongTimeString();
                    default:
                        return d.ToUniversalTime().ToString();
                }
            }

            return value;
        }
    }
}
