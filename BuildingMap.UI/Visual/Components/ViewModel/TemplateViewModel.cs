using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using BuildingMap.Core;
using BuildingMap.UI.Logic;
using Microsoft.Win32;

namespace BuildingMap.UI.Visual.Components.ViewModel
{
	public class TemplateViewModel : ObservableObject
	{
		private readonly MapManager _mapManager;
		private readonly SettingsManager _settingsManager;

		private ItemStyle _style;

		public TemplateViewModel(
			MapManager mapManager,
			SettingsManager settingsManager) 
		{
			_mapManager = mapManager;
			_settingsManager = settingsManager;

			AddImageCommand = new RelayCommand(AddImage);
			RemoveImageCommand = new RelayCommand(RemoveImage, () => HasImage);
			SelectTemplateCommand = new RelayCommand(Select);

			_mapManager.StyleChanged += TryUpdate;
			_settingsManager.SelectedStyleChanged += (_, _) => OnPropertyChanged(nameof(IsSelected));
		}

		public RelayCommand AddImageCommand { get; set; }
		public RelayCommand RemoveImageCommand { get; set; }
		public RelayCommand SelectTemplateCommand { get; }

		public void Select()
		{
			_settingsManager.SelectStyle(_style);
		}

		public bool HasImage => ImageData != null; 
		
		public bool IsSelected
		{
			get => _settingsManager.SelectedStyle == _style;
			set
			{
				if (value) Select();
			}
		}

		public Color Color
		{
			get => _style.Color.ToWindows();
			set
			{
				_style.Color = value.ToDrawing();
				NotifyUpdate();
			}
		}

		public Color SelectedColor
		{
			get => _style.SelectedColor.ToWindows();
			set
			{
				_style.SelectedColor = value.ToDrawing();
				NotifyUpdate();
			}
		}

		public double Radius
		{
			get => _style.Radius;
			set
			{
				_style.Radius = value;
				NotifyUpdate();
			}
		}

		public int FontSize
		{
			get => _style.FontSize;
			set
			{
				_style.FontSize = value;
				NotifyUpdate();
			}
		}

		public Color ForegroundColor
		{
			get => _style.ForegroundColor.ToWindows();
			set
			{
				_style.ForegroundColor = value.ToDrawing();
				NotifyUpdate();
			}
		}

		public byte[] ImageData
		{
			get
			{
				return _style.ImageId == null ? null : _mapManager.Map.ImagesData.GetValueOrDefault(_style.ImageId);
			}
			set
			{
				if (value == null)
				{
					if (_style.ImageId != null)
					{
						_mapManager.Map.TryRemoveImage(_style.ImageId);
						_style.ImageId = null;
					}
				}
				else
				{
					_style.ImageId = _mapManager.Map.AddNewImage(value);
				}

				NotifyUpdate();
				OnPropertyChanged(nameof(HasImage));
			}
		}

		public double ImageScale
		{
			get => _style.ImageScale;
			set
			{
				_style.ImageScale = value;
				NotifyUpdate();
			}
		}

		public void Initialize(ItemStyle itemStyle)
		{
			_style = itemStyle;
		}

		private void AddImage()
		{
			var openFileDialog = new OpenFileDialog();
			var result = openFileDialog.ShowDialog();

			if (!result.Value) return;

			ImageData = File.ReadAllBytes(openFileDialog.FileName);
		}

		private void RemoveImage()
		{
			ImageData = null;
		}

		private void NotifyUpdate([CallerMemberName] string propertyName = "")
		{
			OnPropertyChanged(propertyName);
			_mapManager.UpdateStyle(this, _style, propertyName);
		}

		private void TryUpdate(object sender, StyleUpdateEventArgs args)
		{
			if (sender == this || _style.Id != args.Style.Id) return;

			OnPropertyChanged(args.PropertyName);
		}
	}
}
