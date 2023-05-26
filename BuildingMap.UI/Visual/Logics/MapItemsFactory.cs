using System;
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
		private readonly SettingsManager _settingsManager;
		private readonly Func<MapItemViewModel> _mapItemFactory;

		public MapItemsFactory(
			MapManager mapManager,
			ClipboardManager clipboardManager,
			SettingsManager settingsManager,
			Func<MapItemViewModel> mapItemFactory)
		{
			_mapManager = mapManager;
			_clipboardManager = clipboardManager;
			_settingsManager = settingsManager;
			_mapItemFactory = mapItemFactory;
		}

		public MapItemViewModel CreateNew()
		{
			return CreateNew(_settingsManager.SelectedStyle);
		}

		public MapItemViewModel CreateNew(ItemStyle style)
		{
			return Create(new MapItem() { Id = _mapManager.Map.GetNextItemId(), StyleId = style.Id });
		}

		public MapItemViewModel Create(MapItem mapItem)
		{
			var viewModel = _mapItemFactory();

			viewModel.Initialize(mapItem);

			return viewModel;
		}

		public bool CanCreateCopyFromClipboard()
		{
			return _clipboardManager.HasData<MapItem>();
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
