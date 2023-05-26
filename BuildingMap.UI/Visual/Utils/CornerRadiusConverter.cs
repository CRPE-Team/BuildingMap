using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace BuildingMap.UI.Visual.Utils
{
	public class CornerRadiusConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var radiuses = values.Cast<double>().ToArray();

			return new CornerRadius(radiuses[0], radiuses[1], radiuses[2], radiuses[3]);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			var radius = (CornerRadius)value;

			return new object[]
			{
				radius.TopLeft,
				radius.TopRight,
				radius.BottomRight,
				radius.BottomLeft
			};
		}
	}
}
