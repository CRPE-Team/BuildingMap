using System;
using System.Collections.Generic;
using System.Linq;
using BuildingMap.Core;

namespace BuildingMap.UI.Logic
{
	public class SettingsManager
	{
		private readonly MapManager _mapManager;

		private bool _showBackFloor = true;

		public SettingsManager(MapManager mapManager)
		{
			_mapManager = mapManager;

			mapManager.FloorsChanged += (_, _) => SelectFloor(mapManager.Map.Floors.Count);
			mapManager.MapLoaded += (_, _) => Load();
			Load();
		}

		public bool AllowEdit { get; private set; }

		public IEnumerable<ItemStyle> DisplayStyles => _mapManager.Map.Styles.Values.Where(
			style => (style.Flags.HasFlag(StyleFlags.System) || style.Flags.HasFlag(StyleFlags.Static)) && !style.Flags.HasFlag(StyleFlags.Hidden));

		public int SelectedFloor { get; private set; } = 1;
		public int BackFloor { get; private set; } = 1;
		public bool ShowBackFloor { get => _showBackFloor && AllowEdit; set => _showBackFloor = value; }

		public ItemStyle SelectedStyle { get; private set; }

		public ItemStyle ModifiedStyle { get; private set; }

		public event EventHandler AllowEditChanged;
		public event EventHandler SelectedFloorChanged;
		public event EventHandler BackFloorChanged;
		public event EventHandler SelectedStyleChanged;
		public event EventHandler ModifiedStyleChanged;

		public void SetAllowEdit(bool allowEdit)
		{
			if (_mapManager.Map.ReadOnly) return;

			AllowEdit = allowEdit;

			AllowEditChanged?.Invoke(this, new EventArgs());
		}

		public void SelectFloor(int number)
		{
			SelectedFloor = number;

			SelectedFloorChanged?.Invoke(this, new EventArgs());
		}

		public void SetBackFloor(int number)
		{
			BackFloor = number;

			BackFloorChanged?.Invoke(this, new EventArgs());
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
