using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BuildingMap.UI.Pages.ViewModel
{
	public class MapPageViewModel : ObservableObject
	{
		private int _gridSize = 5;
		private Vector _offset;
		private Color _background;
		private double _zoom = 1;

		public MapPageViewModel(MapEditModeViewModel editModeViewModel)
		{
			MapEditModeViewModel = editModeViewModel;
		}

		public MapEditModeViewModel MapEditModeViewModel { get; }

		public ObservableCollection<MapItemViewModel> Items { get; } = new ObservableCollection<MapItemViewModel>();

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

		public Color Background
		{
			get => _background;
			set
			{
				_background = value;
				OnPropertyChanged();
			}
		}

		public double Zoom
		{
			get => _zoom;
			set
			{
				_zoom = value;
				OnPropertyChanged();
			}
		}
	}
}
