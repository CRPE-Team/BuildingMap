using System.Windows;

namespace BuildingMap.UI.Pages.ViewModel
{
	public class MapPageViewModel : ObservableObject
    {
		private bool _allowEditMode = false;
		private Vector _offset = new Vector(10, 10);

		public MapPageViewModel()
		{

		}

		public bool AllowEdit
		{
			get => _allowEditMode;
			set
			{
				_allowEditMode = value;
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
