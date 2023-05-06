using System.Windows;

namespace BuildingMap.UI.Visual.Utils
{
	public static class DataContextHelper
	{
		public static bool TryGetDataContext(object obj, out object dataContext)
		{
			if (obj is FrameworkElement element)
			{
				dataContext = element.DataContext;
			}
			else
			{
				dataContext = null;
			}

			return dataContext != null;
		}

		public static bool TryGetDataContext<T>(object obj, out T dataContext)
		{
			if (!TryGetDataContext(obj, out object dc))
			{
				dataContext = default;
				return false;
			}

			if (dc is not T)
			{
				dataContext = default;
				return false;
			}

			dataContext = (T) dc;
			return true;
		}
	}
}
