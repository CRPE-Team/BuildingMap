using System;

namespace BuildingMap.UI.Logic
{
	public class SettingsManager
	{
		private readonly MapManager _mapManager;

		public SettingsManager(MapManager mapManager)
		{
			_mapManager = mapManager;

			mapManager.FloorAdded += (_, _) => SelectFloor(mapManager.Map.Floors.Count - 1);
		}

		public int SelectedFloor { get; private set; }

		public event EventHandler SelectedFloorChanged;

		public void SelectFloor(int number)
		{
			SelectedFloor = number;

			SelectedFloorChanged?.Invoke(this, new EventArgs());
		}
	}
}
