using System.Windows;

namespace BuildingMap.UI.Pages.ViewModel
{
	public class MapPageViewModel : ObservableObject
	{
		private int _gridSize = 5;
		private Vector _offset;

		public MapPageViewModel(MapEditModeViewModel editMode)
		{
			EditMode = editMode;
		}

		public MapEditModeViewModel EditMode { get; set; }

		public int GridSize
		{
			get => _gridSize;
			set
			{
				_gridSize = value;
				OnPropertyChanged();
			}
		}

		public Vector Offset
		{
			get => _offset;
			set
			{
				_offset = value;
				OnPropertyChanged();
			}
		}
	}
}
