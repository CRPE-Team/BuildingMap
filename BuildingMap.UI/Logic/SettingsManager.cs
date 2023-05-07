using System;

namespace BuildingMap.UI.Logic
{
	public class SettingsManager
	{
		private readonly MapManager _mapManager;

		public SettingsManager(MapManager mapManager)
		{
			_mapManager = mapManager;

			mapManager.FloorsChanged += (_, _) => SelectFloor(mapManager.Map.Floors.Count);
		}

		public int SelectedFloor { get; private set; } = 1;

		public event EventHandler SelectedFloorChanged;

		public void SelectFloor(int number)
		{
			SelectedFloor = number;

			SelectedFloorChanged?.Invoke(this, new EventArgs());
		}
	}
}
