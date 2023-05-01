using System.Windows;

namespace BuildingMap.UI.Pages.ViewModel
{
	public class MapItemViewModel : ObservableObject
	{
		private Point _position;
		private Size _size;

		public Point Position
		{
			get => _position;
			set
			{
				_position = value;
				OnPropertyChanged();
			}
		}

		public Size Size
		{
			get => _size;
			set
			{
				_size = value;
				OnPropertyChanged();
			}
		}
	}
}
