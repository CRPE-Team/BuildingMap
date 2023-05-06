namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class MapEditModeViewModel : ObservableObject
	{
		private bool _allowEdit = true;

		public MapEditModeViewModel(BackgroundImageViewModel backgroundImageViewModel)
		{
			BackgroundImageViewModel = backgroundImageViewModel;
		}

		public BackgroundImageViewModel BackgroundImageViewModel { get; }

		public bool AllowEdit
		{
			get => _allowEdit;
			set
			{
				_allowEdit = value;
				OnPropertyChanged();
			}
		}
	}
}
