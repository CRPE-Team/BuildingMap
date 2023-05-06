using System;
using BuildingMap.UI.Logic;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class MapViewModel : ObservableObject
	{
		private readonly MapFloorViewModel _currentFloor;
		private readonly MapManager _mapManager;
		private readonly SettingsManager _settingsManager;

		public MapViewModel(
			MapFloorViewModel currentFloor, 
			MapManager mapManager, 
			SettingsManager settingsManager)
		{
			_currentFloor = currentFloor;
			_mapManager = mapManager;
			_settingsManager = settingsManager;

			mapManager.MapLoaded += (_, _) => UpdateFloor();
			settingsManager.SelectedFloorChanged += (_, _) => UpdateFloor();
		}

		public MapFloorViewModel CurrentFloor => _currentFloor;

		private void UpdateFloor()
		{
			CurrentFloor.Update();
		}
	}
}
