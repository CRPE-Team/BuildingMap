﻿using System;
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
			mapItem.Color = Color.FromRgb(0x00, 0xD3, 0xEA);
			mapItem.SelectedColor = Color.FromRgb(0x00, 0xF3, 0xEA);

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