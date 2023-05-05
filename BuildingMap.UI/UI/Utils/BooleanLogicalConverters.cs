using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace BuildingMap.UI.Utils
{
	public class BooleanOrLogicalConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			return values.Cast<bool>().Any(val => val);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
