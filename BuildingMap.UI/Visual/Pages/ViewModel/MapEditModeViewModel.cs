using System;
using System.Windows;
using BuildingMap.UI.Logic;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class MapEditModeViewModel : ObservableObject
	{
		private bool _allowEdit = true;

		private BackgroundImageViewModel _backgroundImageViewModel;

		private readonly MapManager _mapManager;
		private readonly SettingsManager _settingsManager;
		private readonly Func<BackgroundImageViewModel> _backgroundImageViewModelFactory;	

		public MapEditModeViewModel(
			MapManager mapManager, 
			SettingsManager settingsManager, 
			Func<BackgroundImageViewModel> backgroundImageViewModelFactory)
		{
			_mapManager = mapManager;
			_settingsManager = settingsManager;
			_backgroundImageViewModelFactory = backgroundImageViewModelFactory;

			_settingsManager.SelectedFloorChanged += (_, _) => Update();

			Update();
		}

		public BackgroundImageViewModel BackgroundImageViewModel
		{
			get => _backgroundImageViewModel;
			private set
			{
				_backgroundImageViewModel = value;
				OnPropertyChanged();
			}
		}

		public bool AllowEdit
		{
			get => _allowEdit;
			set
			{
				_allowEdit = value;
				OnPropertyChanged();
			}
		}

		public void ChangeAllowEdit()
		{
			AllowEdit = !AllowEdit;
		}

		private void Update()
		{
			var backgroundImageViewModel = _backgroundImageViewModelFactory();
			backgroundImageViewModel.Initialize(_mapManager.Map.GetFloorByNumber(_settingsManager.SelectedFloor));
			BackgroundImageViewModel = backgroundImageViewModel;
		}
	}
}
