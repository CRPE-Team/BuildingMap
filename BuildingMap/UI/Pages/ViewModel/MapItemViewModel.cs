using System.Windows;
using System.Windows.Media;
using BuildingMap.UI.Utils;

namespace BuildingMap.UI.Pages.ViewModel
{
	public class MapItemViewModel : ObservableObject
	{
		private readonly ClipboardManager _clipboardManager;

		private Point _position;
		private Size _size;
		private Color _color;
		private Color _selectedColor;
		private bool _isSelected;

		public MapItemViewModel(ClipboardManager clipboardManager)
		{
			_clipboardManager = clipboardManager;
		}

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

		public void CopyToClipboard()
		{
			_clipboardManager.SetData(this);
		}
	}
}
