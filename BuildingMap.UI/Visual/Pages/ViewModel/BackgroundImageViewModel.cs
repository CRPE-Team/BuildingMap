using System.IO;
using BuildingMap.Core;
using Microsoft.Win32;
using Point = System.Windows.Point;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class BackgroundImageViewModel : ObservableObject
	{
		public Floor _currentFloor;

		public BackgroundImageViewModel()
		{
			SelectImageCommand = new RelayCommand(SelectImage);
			RemoveImageCommand = new RelayCommand(RemoveImage);
		}

		public RelayCommand SelectImageCommand { get; }
		public RelayCommand RemoveImageCommand { get; }

		public bool HasImage => ImageData != null;

		public byte[] ImageData
		{
			get => _currentFloor.ImageInfo?.Data;
			set
			{
				if (_currentFloor.ImageInfo == null)
				{
					_currentFloor.ImageInfo = new ImageInfo();
					Scale = 0.4;
				}

				_currentFloor.ImageInfo.Data = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(HasImage));
			}
		}

		public Point Position
		{
			get => _currentFloor.ImageInfo?.Position.ToPoint() ?? default;
			set
			{
				if (_currentFloor.ImageInfo == null) return;

				_currentFloor.ImageInfo.Position = value.ToNumerics();
				OnPropertyChanged();
			}
		}

		public double Scale
		{
			get => _currentFloor.ImageInfo?.Scale ?? default;
			set
			{
				if (_currentFloor.ImageInfo == null) return;

				_currentFloor.ImageInfo.Scale = value;
				OnPropertyChanged();
			}
		}

		public bool Show
		{
			get => _currentFloor.ImageInfo?.Show ?? default;
			set
			{
				if (_currentFloor.ImageInfo == null) return;

				_currentFloor.ImageInfo.Show = value;
				OnPropertyChanged();
			}
		}

		public void Initialize(Floor floor)
		{
			_currentFloor = floor;
		}

		private void SelectImage()
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
	}
}
