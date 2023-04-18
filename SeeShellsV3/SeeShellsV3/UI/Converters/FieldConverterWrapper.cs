using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using SeeShellsV3.UI.Converters;

namespace SeeShellsV3.UI.Converters
{
    /// <summary>
    /// Wrapper class that allows for the use of multiple converters in a single binding
    /// </summary>
    public class FieldConverterWrapper: IValueConverter
    {
        /// <summary>
        /// List of converters to be wrapped.
        /// </summary>
        private IValueConverter[] Converters = new IValueConverter[]
        {
            new ByteArrayConverter(), 
            new FileSizeConverter()
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (IValueConverter converter in Converters)
            {
                value = converter.Convert(value, targetType, parameter, culture);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (IValueConverter converter in Converters)
            {
                value = converter.ConvertBack(value, targetType, parameter, culture);
            }

            return value;
        }
    }
}
