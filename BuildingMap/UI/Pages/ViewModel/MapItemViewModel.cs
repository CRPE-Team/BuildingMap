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

		private string _text;
		private int _fontSize = 12;
		private Color _foregroundColor;
		private RotationAngle _rotationAngle;

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

		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				OnPropertyChanged();
			}
		}

		public int FontSize
		{
			get => _fontSize;
			set
			{
				_fontSize = value;
				OnPropertyChanged();
			}
		}

		public Color ForegroundColor
		{
			get => _foregroundColor;
			set
			{
				_foregroundColor = value;
				OnPropertyChanged();
			}
		}

		public RotationAngle RotationAngle
		{
			get => _rotationAngle;
			set
			{
				_rotationAngle = value;
				OnPropertyChanged();
			}
		}

		public void CopyToClipboard()
		{
			_clipboardManager.SetData(this);
		}
	}
}
