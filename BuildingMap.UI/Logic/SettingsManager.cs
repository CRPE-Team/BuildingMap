using System;
using System.Collections.Generic;
using System.Linq;
using BuildingMap.Core;

namespace BuildingMap.UI.Logic
{
	public class SettingsManager
	{
		private readonly MapManager _mapManager;

		public IEnumerable<ItemStyle> DisplayStyles => _mapManager.Map.Styles.Values.Where(
			style => (style.Flags.HasFlag(StyleFlags.System) || style.Flags.HasFlag(StyleFlags.Static)) && !style.Flags.HasFlag(StyleFlags.Hidden));

		public SettingsManager(MapManager mapManager)
		{
			_mapManager = mapManager;

			mapManager.FloorsChanged += (_, _) => SelectFloor(mapManager.Map.Floors.Count);
			mapManager.MapLoaded += (_, _) => Load();
			Load();
		}

		public int SelectedFloor { get; private set; } = 1;

		public ItemStyle SelectedStyle { get; private set; }

		public ItemStyle ModifiedStyle { get; private set; }

		public event EventHandler SelectedFloorChanged;
		public event EventHandler SelectedStyleChanged;
		public event EventHandler ModifiedStyleChanged;

		public void SelectFloor(int number)
		{
			SelectedFloor = number;

			SelectedFloorChanged?.Invoke(this, new EventArgs());
		}

		public void SelectStyle(ItemStyle style)
		{
			ModifyStyle(style);
			if (style.Flags.HasFlag(StyleFlags.Hidden)) return;

			SelectedStyle = style;

			SelectedStyleChanged?.Invoke(this, new EventArgs());
		}

		public void ModifyStyle(ItemStyle style)
		{
			ModifiedStyle = style;

			ModifiedStyleChanged?.Invoke(this, new EventArgs());
		}

		private void Load()
		{
			SelectStyle(DisplayStyles.FirstOrDefault());
		}
	}
}
