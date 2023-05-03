using System.Windows.Media;
using BuildingMap.UI.Pages.ViewModel;

namespace BuildingMap.UI.Logics
{
	public class MapItemsFactory
	{
		public MapItemViewModel CreateNew()
		{
			return new MapItemViewModel()
			{
				Color = Color.FromRgb(0x00, 0xD3, 0xEA),
				SelectedColor = Color.FromRgb(0x00, 0xF3, 0xEA),
			};
		}

		public MapItemViewModel TryCreateCopyFromClipboard()
		{
			return null;
		}
	}
}
