using System;
using BuildingMap.UI.Logic;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class MapViewModel : ObservableObject
	{
		private readonly Func<MapFloorViewModel> _currentFloorFactory;
		private readonly MapManager _mapManager;
		private readonly SettingsManager _settingsManager;
		
		private MapFloorViewModel _currentFloor;

		public MapViewModel(
			Func<MapFloorViewModel> currentFloor, 
			MapManager mapManager, 
			SettingsManager settingsManager)
		{
			_currentFloorFactory = currentFloor;
			_mapManager = mapManager;
			_settingsManager = settingsManager;

			settingsManager.SelectedFloorChanged += (_, _) => UpdateFloor();

			UpdateFloor();
		}

		public MapFloorViewModel CurrentFloor
		{
			get => _currentFloor;
			set
			{
				_currentFloor = value;
				OnPropertyChanged();
			}
		}

		private void UpdateFloor()
		{
			CurrentFloor = _currentFloorFactory();
		}
	}
}
