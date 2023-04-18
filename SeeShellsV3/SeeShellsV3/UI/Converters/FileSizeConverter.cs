using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SeeShellsV3.UI.Converters
{
    public class FileSizeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UInt32 originalValue)
            {
                if (originalValue == 0)
                {
                    return "N/A";
                }

                float convertedValue = originalValue;
                int divided = 0;

                while (convertedValue > BytesInKb)
                {
                    convertedValue /= BytesInKb;
                    divided++;
                }

                if (divided == 0)
                {
                    return originalValue.ToString() + "Bytes";
                }

                return OutputPattern
                    .Replace("{ORIGINAL_SIZE}", originalValue.ToString())
                    .Replace("{CONVERTED_SIZE}", convertedValue.ToString("0.00"))
                    .Replace("{UNITS}", sizeOutputUnits[divided]);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        internal virtual string OutputPattern => "{ORIGINAL_SIZE} ({CONVERTED_SIZE} {UNITS})";

        private string[] sizeOutputUnits = new string[9] {"B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};

        private const int BytesInKb = 1024;
    }
}
