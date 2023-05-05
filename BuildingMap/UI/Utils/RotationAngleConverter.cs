using System;
using System.Globalization;
using System.Windows.Data;

namespace BuildingMap.UI.Utils
{
	public class RotationAngleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (int) (RotationAngle) value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (RotationAngle) (int) value;
		}
	}
}
