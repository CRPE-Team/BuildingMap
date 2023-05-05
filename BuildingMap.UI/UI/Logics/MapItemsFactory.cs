using System;
using System.Windows.Media;
using BuildingMap.UI.Pages.ViewModel;
using BuildingMap.UI.Utils;

namespace BuildingMap.UI.Logics
{
	public class MapItemsFactory
	{
		private readonly ClipboardManager _clipboardManager;
		private readonly Func<MapItemViewModel> _mapItemFactory;

		public MapItemsFactory(ClipboardManager clipboardManager, Func<MapItemViewModel> mapItemFactory)
		{
			_clipboardManager = clipboardManager;
			_mapItemFactory = mapItemFactory;
		}

		public MapItemViewModel CreateNew()
		{
			var mapItem = _mapItemFactory();

			mapItem.Radius = 10;
			mapItem.Color = Color.FromRgb(0x00, 0xD3, 0xEA);
			mapItem.SelectedColor = Color.FromRgb(0x00, 0xF3, 0xEA);
			mapItem.FontSize = 12;
			mapItem.ForegroundColor = Color.FromRgb(0x00, 0x00, 0x00);
			mapItem.ImageSource = "C:\\Users\\Grinev\\source\\repos\\CRPE-Team\\BuildingMap\\BuildingMap.UI\\UI\\Pages\\Izuku-Midoriya-Изуку-Мидория-Моя-Геройская-Академия-арты-картинки-12.jpg";
			mapItem.ImageScale = 0.8;

			return mapItem;
		}

		public bool TryCreateCopyFromClipboard(out MapItemViewModel copy)
		{
			_clipboardManager.TryGetData<MapItemViewModel>(out var cache);

			if (cache ==  null)
			{
				copy = null;
				return false;
			}

			copy = CreateNew();
			copy.Position = cache.Position;
			copy.Size = cache.Size;
			copy.Color = cache.Color;
			copy.SelectedColor = cache.SelectedColor;
			
			return true;
		}
	}
}
