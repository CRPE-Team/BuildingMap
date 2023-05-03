using System.Windows;
using System.Windows.Media;

namespace BuildingMap.UI.Pages.ViewModel
{
	public class MapItemViewModel : ObservableObject
	{
		private Point _position;
		private Size _size;
		private Color _color;
		private Color _selectedColor;
		private bool _isSelected;

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

		public Color Color
		{
			get => _color;
			set
			{
				_color = value;
				OnPropertyChanged();
			}
		}

		public Color SelectedColor
		{
			get => _selectedColor;
			set
			{
				_selectedColor = value;
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
