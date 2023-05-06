using System;
using System.Threading;
using System.Windows.Media;
using BuildingMap.Core;
using BuildingMap.UI.Logic;
using BuildingMap.UI.Visual.Pages.ViewModel;
using BuildingMap.UI.Visual.Utils;

namespace BuildingMap.UI.Visual.Logics
{
	public class MapItemsFactory
	{
		private readonly MapManager _mapManager;
		private readonly ClipboardManager _clipboardManager;
		private readonly Func<MapItemViewModel> _mapItemFactory;

		public MapItemsFactory(
			MapManager mapManager, 
			ClipboardManager clipboardManager, 
			Func<MapItemViewModel> mapItemFactory)
		{
			_mapManager = mapManager;
			_clipboardManager = clipboardManager;
			_mapItemFactory = mapItemFactory;
		}

		public MapItemViewModel CreateNew()
		{
			var viewModel = Create(new MapItem() { Id = _mapManager.Map.GetNextItemId() });

			viewModel.Radius = 10;
			viewModel.Color = Color.FromRgb(0x00, 0xD3, 0xEA);
			viewModel.SelectedColor = Color.FromRgb(0x00, 0xF3, 0xEA);
			viewModel.FontSize = 12;
			viewModel.ForegroundColor = Color.FromRgb(0x00, 0x00, 0x00);
			viewModel.ImageScale = 0.8;

			return viewModel;
		}

		public MapItemViewModel Create(MapItem mapItem)
		{
			var viewModel = _mapItemFactory();

			viewModel.Initialize(mapItem);

			return viewModel;
		}

		public bool TryCreateCopyFromClipboard(out MapItemViewModel copy)
		{
			_clipboardManager.TryGetData<MapItem>(out var cache);

			if (cache ==  null)
			{
				copy = null;
				return false;
			}

			var clone = cache.Clone() as MapItem;
			clone.Id = _mapManager.Map.GetNextItemId();
			copy = Create(clone);
			
			return true;
		}
	}
}
