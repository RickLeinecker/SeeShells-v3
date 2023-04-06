using SeeShellsV3.Data;
using SeeShellsV3.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SeeShellsV3.UI
{
	class ModuleViewConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			System.Diagnostics.Debug.WriteLine("Yo!");
			return (value as IPdfModule)?.View() ?? null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value as FrameworkElement)?.DataContext ?? null;
		}
	}
}
