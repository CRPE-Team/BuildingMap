namespace BuildingMap.UI.Pages.ViewModel
{
	public class MapEditModeViewModel : ObservableObject
	{
		private bool _allowEditMode = true;

		public MapEditModeViewModel()
		{

		}

		public bool AllowEdit
		{
			get => _allowEditMode;
			set
			{
				_allowEditMode = value;
				OnPropertyChanged(ref _allowEditMode, value);
			}
		}
	}
}
