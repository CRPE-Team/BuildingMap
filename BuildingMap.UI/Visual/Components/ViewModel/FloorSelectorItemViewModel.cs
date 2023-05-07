using BuildingMap.UI.Logic;

namespace BuildingMap.UI.Visual.Components.ViewModel
{
	public class FloorSelectorItemViewModel : ObservableObject
	{
		private readonly SettingsManager _settingsManager;

		public int _number;
		public bool _isSelected;

		public FloorSelectorItemViewModel(SettingsManager settingsManager)
		{
			_settingsManager = settingsManager;

			SelectFloorCommand = new RelayCommand(Select);
		}

		public RelayCommand SelectFloorCommand { get; }

		public void Select()
		{
			_settingsManager.SelectFloor(_number);
		}	

		public int Number
		{
			get => _number;
			set
			{
				_number = value;
				OnPropertyChanged();
			}
		}

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;
				OnPropertyChanged();
			}
		}
	}
}
